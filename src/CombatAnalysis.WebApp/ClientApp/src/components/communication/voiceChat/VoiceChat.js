 import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';
import useRTCVoiceChat from '../../../hooks/useRTCVoiceChat';
import CommunicationMenu from '../CommunicationMenu';
import VoiceChatAudioDeviceSettings from './VoiceChatAudioDeviceSettings';
import VoiceChatMembers from './VoiceChatMembers';

import '../../../styles/communication/chats/voice.scss';

const VoiceChat = () => {
	const { t } = useTranslation("communication/chats/groupChat");

	const navigate = useNavigate();

	const me = useSelector((state) => state.user.value);

	const [openVideoSettings, setOpenVideoSettings] = useState(false);
	const [openAudioSettings, setOpenAudioSettings] = useState(false);

	const [turnOnMicrophone, setTurnOnMicrophone] = useState(true);
	const [turnOnCameraActive, setTurnOnCameraActive] = useState(false);
	const [cameraExecute, setCameraExecute] = useState(false);
	const [turnOnCamera, setTurnOnCamera] = useState(false);
	const [screenSharing, setScreenSharing] = useState(false);
	const [screenSharingActive, setScreenSharingActive] = useState(false);

	const { roomId, chatName } = useParams();

	const [connection, connectToChatAsync, cleanupAsync, switchMicrophoneStatusAsync, switchCameraStatusAsync] = useRTCVoiceChat(roomId);

	useEffect(() => {
		if (me === undefined) {
			return;
		}

		const connectToChat = async () => {
			const signalingAddress = "/voiceChatHub";
			await connectToChatAsync(signalingAddress, turnOnMicrophone);
		}

		connectToChat();
	}, [me]);

	useEffect(() => {
		return () => {
			cleanupAsync();
		}
	}, [connection]);

	useEffect(() => {
		if (connection === null) {
			return;
		}

		connection.on("Connected", async () => {
			setTurnOnCameraActive(true);
			setScreenSharingActive(true);
		});
	}, [connection]);

	useEffect(() => {
		if (connection === null) {
			return;
		}

		const switchMicrophoneStatus = async () => {
			//await switchMicrophoneStatusAsync(turnOnMicrophone);
		}

		switchMicrophoneStatus();
	}, [connection, turnOnMicrophone]);

	useEffect(() => {
		if (connection === null) {
			return;
		}

		const switchCameraStatus = async () => {
			//await switchCameraStatusAsync(turnOnCamera, setCameraExecute);
		}

		switchCameraStatus();

		connection.on("ReceiveRequestCameraStatus", async () => {
			//await switchCameraStatusAsync(turnOnCamera, setCameraExecute);
		});

		return () => {
			connection.off("ReceiveRequestCameraStatus", switchCameraStatus);
		};
	}, [connection, turnOnCamera]);

	const leaveFromCallAsync = async () => {
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
					<div>{chatName}</div>
					<div className="tools">
						{screenSharingActive &&
							<div className="device">
								<FontAwesomeIcon
									icon={screenSharing ? faDisplay : faLinkSlash}
									title={screenSharing ? t("TurnOffScreenSharing") : t("TurnOnScreenSharing")}
									className="device__screen"
									onClick={() => setScreenSharing(!screenSharing)}
								/>
							</div>
						}
						{turnOnCameraActive &&
							<div className="device" style={{ opacity: cameraExecute ? 0.4 : 1 }}>
								<FontAwesomeIcon
									icon={turnOnCamera ? faVideo : faVideoSlash}
									title={turnOnCamera ? t("TurnOffCamera") : t("TurnOnCamera")}
									className="device__camera"
									onClick={cameraExecute ? null : () => setTurnOnCamera(!turnOnCamera)}
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
						}
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
				{/*<VoiceChatMembers*/}
				{/*	roomId={roomId}*/}
				{/*	connection={connection}*/}
				{/*	micStatus={turnOnMicrophone}*/}
				{/*	cameraStatus={turnOnCamera}*/}
				{/*	stream={stream}*/}
				{/*	switchCameraStatusAsync={switchCameraStatusAsync}*/}
				{/*	setCameraExecute={setCameraExecute}*/}
				{/*/>*/}
			</div>
		</>
	);
}

export default memo(VoiceChat);