 import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import useVoice from '../../../hooks/useVoice';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatAudioDeviceSettings from './VoiceChatAudioDeviceSettings';
import VoiceChatContentSharing from './VoiceChatContentSharing';

import '../../../styles/communication/chats/voice.scss';

const VoiceChat = ({ callMinimazedData, setUseMinimaze }) => {
	const { t } = useTranslation("communication/chats/groupChat");

	const mediaRecorderRef = useRef(null);
	const audioContextRef = useRef(new (window.AudioContext || window.webkitAudioContext)());
	const audioBufferQueueRef = useRef([]);
	const scriptProcessorRef = useRef(null);

	const me = useSelector((state) => state.customer.value);

	const [openVideoSettings, setOpenVideoSettings] = useState(false);
	const [openAudioSettings, setOpenAudioSettings] = useState(false);

	const audioRef = useRef(null);

	const voice = useVoice(me, callMinimazedData, setUseMinimaze);

	useEffect(() => {
		const socket = new WebSocket('https://localhost:5007/ws');

		socket.onopen = () => {
			console.log('WebSocket connection established');
			startRecording(socket);
		}

		socket.onmessage = async (event) => {
			try {
				const arrayBuffer = await event.data.arrayBuffer();
				const audioBuffer = await audioContextRef.current.decodeAudioData(arrayBuffer);
				audioBufferQueueRef.current.push(audioBuffer);

				playAudioQueue();
			} catch (error) {
				console.error('Error decoding audio data:', error);
			}
		}

		socket.onerror = (error) => {
			console.error('WebSocket error:', error);
		}

		socket.onclose = () => {
			console.log('WebSocket connection closed');
			if (mediaRecorderRef.current) {
				mediaRecorderRef.current.stop();
			}
		}

		return () => {
			socket.close();
		}
	}, []);

	useEffect(() => {
		if (me === null || callMinimazedData.current.roomId === 0) {
			return;
		}

		voice.func.joinToRoom();

		const beforeunload = (event) => {
			voice.func.leave();
			removeCookie();
		}

		window.addEventListener("beforeunload", beforeunload);

		return () => {
			window.removeEventListener("beforeunload", beforeunload);

			if (callMinimazedData.current.roomId > 0) {
				setUseMinimaze(true);
			}
		}
	}, [me, callMinimazedData.current.roomId]);

	const convertToWav = (buffer) => {
		const numOfChannels = buffer.numberOfChannels;
		const length = buffer.length * numOfChannels * 2 + 44;
		const result = new ArrayBuffer(length);
		const view = new DataView(result);
		const channels = [];
		let offset = 0;
		let pos = 0;

		// Write WAV header
		setUint32(0x46464952); // "RIFF"
		setUint32(length - 8); // file length - 8
		setUint32(0x45564157); // "WAVE"

		setUint32(0x20746d66); // "fmt " chunk
		setUint32(16); // length = 16
		setUint16(1); // PCM (uncompressed)
		setUint16(numOfChannels);
		setUint32(buffer.sampleRate);
		setUint32(buffer.sampleRate * 2 * numOfChannels); // avg. bytes/sec
		setUint16(numOfChannels * 2); // block-align
		setUint16(16); // 16-bit (hardcoded in this demo)

		setUint32(0x61746164); // "data" - chunk
		setUint32(length - pos - 4); // chunk length

		// Write interleaved data
		for (let i = 0; i < buffer.numberOfChannels; i++) {
			channels.push(buffer.getChannelData(i));
		}

		while (pos < length) {
			for (let i = 0; i < numOfChannels; i++) {
				const sample = Math.max(-1, Math.min(1, channels[i][offset])); // clamp
				view.setInt16(pos, sample < 0 ? sample * 0x8000 : sample * 0x7FFF, true); // convert to PCM
				pos += 2;
			}
			offset++;
		}

		return result;

		function setUint16(data) {
			view.setUint16(pos, data, true);
			pos += 2;
		}

		function setUint32(data) {
			view.setUint32(pos, data, true);
			pos += 4;
		}
	}

	const startRecording = async (socket) => {
		try {
			const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
			const audioContext = audioContextRef.current;
			const source = audioContext.createMediaStreamSource(stream);
			const scriptProcessor = audioContext.createScriptProcessor(4096, 1, 1);

			scriptProcessor.onaudioprocess = (event) => {
				const inputBuffer = event.inputBuffer;

				// Convert to WAV format
				const wavData = convertToWav(inputBuffer);

				// Send audio data to WebSocket
				if (socket && socket.readyState === WebSocket.OPEN) {
					socket.send(wavData);
				}
			};

			source.connect(scriptProcessor);
			scriptProcessor.connect(audioContext.destination);

			scriptProcessorRef.current = scriptProcessor;
		} catch (error) {
			console.error('Error starting audio recording:', error);
		}
	};

	const playAudioQueue = () => {
		if (audioBufferQueueRef.current.length === 0) {
			return;
		}

		const audioBuffer = audioBufferQueueRef.current.shift();
		const source = audioContextRef.current.createBufferSource();
		source.buffer = audioBuffer;
		source.connect(audioContextRef.current.destination);
		source.start();
		source.onended = playAudioQueue;
	};

	const isCallStarted = () => {
		const allCokie = document.cookie.split(";");
		const calStartedCookie = allCokie.filter(cookie => cookie.includes("callAlreadyStarted"));
		return calStartedCookie.length > 0;
	}

	const removeCookie = () => {
		document.cookie = "callAlreadyStarted=true;expires=Thu, 01 Jan 1970 00:00:00 UTC";
	}

	const handleOpenVideoSettings = () => {
		setOpenAudioSettings(false);
		setOpenVideoSettings(!openVideoSettings);
	}

	const handleOpenAudioSettings = () => {
		setOpenVideoSettings(false);
		setOpenAudioSettings(!openAudioSettings);
	}

	return (
		<>
			<CommunicationMenu
				currentMenuItem={1}
			/>
			<div className="voice">
				<div className="voice__title">
					<div>{voice.data.renderChatName}</div>
					<div className="tools">
						{callMinimazedData.current.screenSharing
							? <div className="device">
								<FontAwesomeIcon
									icon={faLinkSlash}
									title={t("TurnOffScreenSharing")}
									className="device__screen"
									onClick={() => voice.func.shareScreen(false)}
								/>
							</div>
							: <div className="device">
								<FontAwesomeIcon
									icon={faDisplay}
									title={t("TurnOnScreenSharing")}
									className="device__screen"
									onClick={() => voice.func.shareScreen(true)}
								/>
							</div>
						}
						{callMinimazedData.current.turnOnCamera
							? <div className="device">
								<FontAwesomeIcon
									icon={faVideo}
									title={t("TurnOffCamera")}
									className="device__camera"
									onClick={() => voice.func.switchCamera(false)}
								/>
								{openVideoSettings
									? <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenVideoSettings}
									/>
									: <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenVideoSettings}
									/>
								}
								{openVideoSettings &&
									<VoiceChatAudioDeviceSettings />
								}
							</div>
							: <div className="device">
								<FontAwesomeIcon
									icon={faVideoSlash}
									title={t("TurnOnCamera")}
									className="device__camera"
									onClick={() => voice.func.switchCamera(true)}
								/>
							</div>
						}
						{callMinimazedData.current.turnOnMicrophone
							? <div className="device">
								<FontAwesomeIcon
									icon={faMicrophone}
									title={t("TurnOffMicrophone")}
									className="device__microphone"
									onClick={() => voice.func.switchMicrophone(false)}
								/>
								{openAudioSettings
									? <FontAwesomeIcon
										icon={faAngleUp}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
									: <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
								}
								{openAudioSettings &&
									<VoiceChatAudioDeviceSettings
										setMicrophoneDeviceId={voice.func.setMicrophoneDeviceId}
										switchMicrophoneDevice={voice.func.switchMicrophoneDevice}
										switchAudioOutputDevice={voice.func.switchAudioOutputDevice}
										microphoneIsOn={voice.data.turnOnMicrophone}
									/>
								}
							</div>
							: <div className="device">
								<FontAwesomeIcon
									icon={faMicrophoneSlash}
									title={t("TurnOnMicrophone")}
									className="device__microphone"
									onClick={() => voice.func.switchMicrophone(true)}
								/>
								{openAudioSettings
									? <FontAwesomeIcon
										icon={faAngleUp}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
									: <FontAwesomeIcon
										icon={faAngleDown}
										title={t("Setting")}
										className="device__settings"
										onClick={handleOpenAudioSettings}
									/>
								}
								{openAudioSettings &&
									<VoiceChatAudioDeviceSettings
										isAudio={true}
										setMicrophoneDeviceId={voice.func.setMicrophoneDeviceId}
										switchMicrophoneDevice={voice.func.switchMicrophoneDevice}
										switchAudioOutputDevice={voice.func.switchAudioOutputDevice}
										microphoneIsOn={voice.data.turnOnMicrophone}
									/>
								}
							</div>
						}
						<div className="btn-shadow" title={t("Leave")} onClick={() => voice.func.leave(true)}>
							<FontAwesomeIcon
								icon={faRightFromBracket}
							/>
							<div>{t("Leave")}</div>
						</div>
					</div>
				</div>
				<audio ref={audioRef} />
				<VoiceChatContentSharing
					voice={voice}
				/>
			</div>
		</>
	);
}

export default memo(WithVoiceContext(VoiceChat));