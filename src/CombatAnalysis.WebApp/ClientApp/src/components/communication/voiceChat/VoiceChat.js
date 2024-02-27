import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import useVoice from '../../../hooks/useVoice';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatContentSharing from './VoiceChatContentSharing';
import VoiceChatDeviceSettings from './VoiceChatDeviceSettings';

import '../../../styles/communication/chats/voice.scss';

const VoiceChat = ({ callMinimazedData, setUseMinimaze }) => {
	const { t } = useTranslation("communication/chats/groupChat");

	const navigate = useNavigate();

	const me = useSelector((state) => state.customer.value);

	const [openVideoSettings, setOpenVideoSettings] = useState(false);
	const [openAudioSettings, setOpenAudioSettings] = useState(false);
	const [microphoneDeviceId, setMicrophoneDeviceId] = useState("");

	const audioRef = useRef(null);

	const voice = useVoice(me, callMinimazedData, microphoneDeviceId, setUseMinimaze);

	useEffect(() => {
		if (!isCallStarted()) {
			navigate(`/chats`);
		}

		voice.func.initConnection();

		return () => {
			if (!isCallStarted()) {
				voice.func.leave();
			}
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
									<VoiceChatDeviceSettings />
								}
							</div>
							: <div className="device">
								<FontAwesomeIcon
									icon={faVideoSlash}
									title={t("TurnOnCamera")}
									className="device__camera"
									onClick={() => voice.func.switchCamera(true)}
								/>
								{openVideoSettings
									? <FontAwesomeIcon
										icon={faAngleUp}
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
									<VoiceChatDeviceSettings />
								}
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
									<VoiceChatDeviceSettings
										isAudio={true}
										setMicrophoneDeviceId={setMicrophoneDeviceId}
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
									<VoiceChatDeviceSettings
										isAudio={true}
										setMicrophoneDeviceId={setMicrophoneDeviceId}
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