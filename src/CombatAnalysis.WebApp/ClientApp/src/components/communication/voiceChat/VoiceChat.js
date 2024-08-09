 import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import useWebSocket from '../../../hooks/useWebSocket';
//import useVoice from '../../../hooks/useVoice';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatAudioDeviceSettings from './VoiceChatAudioDeviceSettings';
import VoiceChatContentSharing from './VoiceChatContentSharing';

import '../../../styles/communication/chats/voice.scss';

const VoiceChat = () => {
	const { t } = useTranslation("communication/chats/groupChat");

	const navigate = useNavigate();

	const me = useSelector((state) => state.user.value);

	const [openVideoSettings, setOpenVideoSettings] = useState(false);
	const [openAudioSettings, setOpenAudioSettings] = useState(false);

	const [turnOnMicrophone, setTurnOnMicrophone] = useState(true);
	const [turnOnCamera, setTurnOnCamera] = useState(false);
	const [screenSharing, setScreenSharing] = useState(false);

	const [socketRef, connectToChat, cleanupAudioResources, switchMicrophoneStatusAsync] = useWebSocket(turnOnMicrophone);
	//const voice = useVoice(me?.id);

	useEffect(() => {
		if (me === undefined) {
			return;
		}

		connectToChat(me?.id);

		return () => {
			cleanupAudioResources();
		}
	}, [me]);

	useEffect(() => {
		if (socketRef.current === null) {
			return;
		}

		const switchMicrophoneStatus = async () => {
			await switchMicrophoneStatusAsync();
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
					{/*<div>{voice.data.renderChatName}</div>*/}
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
							{/*{openAudioSettings &&*/}
							{/*	<VoiceChatAudioDeviceSettings*/}
							{/*		setMicrophoneDeviceId={voice.func.setMicrophoneDeviceId}*/}
							{/*		switchMicrophoneDevice={voice.func.switchMicrophoneDevice}*/}
							{/*		switchAudioOutputDevice={voice.func.switchAudioOutputDevice}*/}
							{/*		microphoneIsOn={voice.data.turnOnMicrophone}*/}
							{/*	/>*/}
							{/*}*/}
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
					me={me}
					socketRef={socketRef}
					micStatus={turnOnMicrophone}
				/>
			</div>
		</>
	);
}

export default memo(VoiceChat);