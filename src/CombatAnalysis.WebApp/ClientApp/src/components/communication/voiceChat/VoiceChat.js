 import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';
import useRTCVoiceChat from '../../../hooks/useRTCVoiceChat';
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

	const videosRef = useRef(null);

	const { roomId, chatName } = useParams();

	const [connection, peerConnection, connectToChatAsync, cleanupAudioResources, switchMicrophoneStatusAsync, switchCameraStatusAsync] = useRTCVoiceChat(roomId, turnOnMicrophone);

	useEffect(() => {
		if (me === undefined) {
			return;
		}

		const connectToChat = async () => {
			const signalingAddress = "/voiceChatHub";
			await connectToChatAsync(signalingAddress, videosRef.current);
		}

		connectToChat();

		return () => {
			cleanupAudioResources();
		}
	}, [me]);

	useEffect(() => {
		const switchMicrophoneStatus = async () => {
			await switchMicrophoneStatusAsync(turnOnMicrophone);
		}

		switchMicrophoneStatus();
	}, [turnOnMicrophone]);

	useEffect(() => {
		const switchCameraStatus = async () => {
			await switchCameraStatusAsync(turnOnCamera);
		}

		switchCameraStatus();
	}, [turnOnCamera]);

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
						<div ref={videosRef}></div>
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
					connection={connection}
					micStatus={turnOnMicrophone}
					roomId={roomId}
					peerConnection={peerConnection}
				/>
			</div>
		</>
	);
}

export default memo(VoiceChat);