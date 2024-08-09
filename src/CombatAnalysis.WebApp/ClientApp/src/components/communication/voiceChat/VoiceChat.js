 import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useCallback, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import useVoice from '../../../hooks/useVoice';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatAudioDeviceSettings from './VoiceChatAudioDeviceSettings';
import VoiceChatContentSharing from './VoiceChatContentSharing';

import '../../../styles/communication/chats/voice.scss';

const VoiceChat = ({ callMinimazedData, setUseMinimaze }) => {
	const { t } = useTranslation("communication/chats/groupChat");

	const navigate = useNavigate();

	const audioContextRef = useRef(new (window.AudioContext || window.webkitAudioContext)());
	const audioBufferQueueRef = useRef([]);
	const scriptProcessorRef = useRef(null);
	const socketRef = useRef(null);
	const streamRef = useRef(null);

	const me = useSelector((state) => state.customer.value);

	const [openVideoSettings, setOpenVideoSettings] = useState(false);
	const [openAudioSettings, setOpenAudioSettings] = useState(false);

	const [turnOnMicrophone, setTurnOnMicrophone] = useState(true);
	const [turnOnCamera, setTurnOnCamera] = useState(false);
	const [screenSharing, setScreenSharing] = useState(false);

	const voice = useVoice(me, callMinimazedData, setUseMinimaze);

	useEffect(() => {
        try {
			const socket = new WebSocket(`https://localhost:5007/ws?userId=${me?.id}`);
			socketRef.current = socket;

			socket.addEventListener("open", async () => {
				console.log('WebSocket connection established');
			});

			socket.addEventListener("message", async (event) => {
				try {
					if (event.data instanceof Blob) {
						const arrayBuffer = await event.data.arrayBuffer();
						const audioBuffer = await audioContextRef.current.decodeAudioData(arrayBuffer);
						audioBufferQueueRef.current.push(audioBuffer);

						playAudioQueue();
					} else {
						console.warn('Received data is not a Blob:', event.data);
					}
				} catch (error) {
					console.error('Error decoding audio data:', error);
				}
			})

			socket.onerror = (error) => {
				console.error('WebSocket error:', error);
			}

			socket.onclose = () => {
				console.log('WebSocket connection closed');
			}
		} catch (e) {
			console.log("error 12333");
        }

		return () => {
			cleanupAudioResources();
		}
	}, [socketRef]);

	useEffect(() => {
		if (socketRef.current === null) {
			return;
		}

		const switchMicrophoneStatus = async () => {
			if (turnOnMicrophone) {
				await turnOnRecordingAsync();

				return;
			}

			if (streamRef.current) {
				streamRef.current.getTracks().forEach(track => track.stop());
				streamRef.current = null;
			}

			if (scriptProcessorRef.current) {
				scriptProcessorRef.current.disconnect();
				scriptProcessorRef.current.onaudioprocess = null;
				scriptProcessorRef.current = null;
			}

			audioBufferQueueRef.current = [];
			audioContextRef.current = new (window.AudioContext || window.webkitAudioContext)();
		}

		switchMicrophoneStatus();
		sendMicrophoneStatus();
	}, [turnOnMicrophone]);

	const sendMicrophoneStatus = useCallback(() => {
		const message = `MIC_STATUS;${turnOnMicrophone ? "on" : "off"}`;

		// Check if the WebSocket is open before sending the messageЦ
		if (socketRef.current.readyState === WebSocket.OPEN) {
			socketRef.current.send(message);
		} else {
			// Wait for the WebSocket to open before sending the message
			socketRef.current.addEventListener("open", () => {
				socketRef.current.send(message);
			}, { once: true });
		}
	}, [turnOnMicrophone]);
	
	const leaveFromCallAsync = async () => {
		cleanupAudioResources();

		navigate("/chats");
	}

	const cleanupAudioResources = () => {
		// Stop the scriptProcessor
		if (scriptProcessorRef.current) {
			scriptProcessorRef.current.disconnect();
			scriptProcessorRef.current.onaudioprocess = null;
			scriptProcessorRef.current = null;
		}

		// Stop the media stream tracks
		if (streamRef.current) {
			streamRef.current.getTracks().forEach(track => track.stop());
			streamRef.current = null;
		}

		// Close the audio context
		if (audioContextRef.current && audioContextRef.current.state !== 'closed') {
			audioContextRef.current.close().catch(error => console.error('Error closing audio context:', error));
		}

		// Close the WebSocket connection
		if (socketRef.current) {
			socketRef.current.close();
			socketRef.current = null;
		}

		audioBufferQueueRef.current = [];
		audioContextRef.current = new (window.AudioContext || window.webkitAudioContext)();
	}

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

	const turnOnRecordingAsync = async () => {
		if (socketRef.current === null) {
			return;
		}

		const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
		streamRef.current = stream;

		startRecording(socketRef.current, stream);
	}

	const startRecording = (socket, stream) => {
		try {
			const audioContext = audioContextRef.current;
			const source = audioContext.createMediaStreamSource(stream);
			const scriptProcessor = audioContext.createScriptProcessor(4096, 1, 1);

			scriptProcessor.addEventListener("audioprocess", (event) => {
				const inputBuffer = event.inputBuffer;

				// Convert to WAV format
				const wavData = convertToWav(inputBuffer);

				// Send audio data to WebSocket
				if (socket && socket.readyState === WebSocket.OPEN) {
					socket.send(wavData);
				}
			});

			source.connect(scriptProcessor);
			scriptProcessor.connect(audioContext.destination);

			scriptProcessorRef.current = scriptProcessor;
		} catch (error) {
			console.error('Error starting audio recording:', error);
		}
	}

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
						<div className="device">
							<FontAwesomeIcon
								icon={screenSharing ? faDisplay : faLinkSlash}
								title={screenSharing ? t("TurnOffScreenSharing") : t("TurnOnScreenSharing")}
								className="device__screen"
								onClick={() => setScreenSharing(!screenSharing)}
							/>
						</div>
						<div className="device">
							<FontAwesomeIcon
								icon={turnOnCamera ? faVideo : faVideoSlash}
								title={turnOnCamera ? t("TurnOffCamera") : t("TurnOnCamera")}
								className="device__camera"
								onClick={() => setTurnOnCamera(!turnOnCamera)}
							/>
							<FontAwesomeIcon
								icon={openVideoSettings ? faAngleDown : faAngleUp}
								title={t("Setting")}
								className="device__settings"
								onClick={handleOpenVideoSettings}
							/>
							{openVideoSettings &&
								<VoiceChatAudioDeviceSettings />
							}
						</div>
						<div className="device">
							<FontAwesomeIcon
								icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
								title={turnOnMicrophone ? t("TurnOffMicrophone") : t("TurnOnMicrophone")}
								className="device__microphone"
								onClick={() => setTurnOnMicrophone(!turnOnMicrophone)}
							/>
							<FontAwesomeIcon
								icon={openAudioSettings ? faAngleDown : faAngleUp}
								title={t("Setting")}
								className="device__settings"
								onClick={handleOpenAudioSettings}
							/>
							{openAudioSettings &&
								<VoiceChatAudioDeviceSettings
									setMicrophoneDeviceId={voice.func.setMicrophoneDeviceId}
									switchMicrophoneDevice={voice.func.switchMicrophoneDevice}
									switchAudioOutputDevice={voice.func.switchAudioOutputDevice}
									microphoneIsOn={voice.data.turnOnMicrophone}
								/>
							}
						</div>
						<div className="btn-shadow" title={t("Leave")} onClick={async () => await leaveFromCallAsync()}>
							<FontAwesomeIcon
								icon={faRightFromBracket}
							/>
							<div>{t("Leave")}</div>
						</div>
					</div>
				</div>
				<VoiceChatContentSharing
					socket={socketRef.current}
				/>
			</div>
		</>
	);
}

export default memo(WithVoiceContext(VoiceChat));