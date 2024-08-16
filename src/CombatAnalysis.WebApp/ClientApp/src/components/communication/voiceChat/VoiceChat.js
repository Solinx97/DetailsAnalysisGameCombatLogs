 import { faAngleDown, faAngleUp, faDisplay, faLinkSlash, faMicrophone, faMicrophoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from 'react';
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

	const [canLeave, setCanLeave] = useState(false);
	const [turnOnMicrophone, setTurnOnMicrophone] = useState(true);
	const [turnOnCameraActive, setTurnOnCameraActive] = useState(false);
	const [cameraExecute, setCameraExecute] = useState(false);
	const [turnOnCamera, setTurnOnCamera] = useState(false);
	const [screenSharing, setScreenSharing] = useState(false);
	const [screenSharingActive, setScreenSharingActive] = useState(false);

	const audioInputDeviceIdRef = useRef(null);
	const audioOutputDeviceIdRef = useRef(null);

	const { roomId, chatName } = useParams();

	const [connection, peerConnectionsRef, stream, connectToChatAsync, stopMediaData, cleanupAsync, switchMicrophoneStatusAsync, switchCameraStatusAsync] = useRTCVoiceChat(roomId);

	useEffect(() => {
		if (!me) {
			return;
		}

		const connectToChat = async () => {
			const signalingAddress = "/voiceChatHub";
			await connectToChatAsync(signalingAddress, setCanLeave);
		}

		connectToChat();
	}, [me]);

	useEffect(() => {
		if (connection === null) {
			return;
		}

		connection.on("Connected", async () => {
			setTurnOnCameraActive(true);
			setScreenSharingActive(true);
		});

		return () => {
			const cleanup = async () => {
				stopMediaData();
				await cleanupAsync();
			}

			cleanup();
		}
	}, [connection]);

	useEffect(() => {
		if (connection === null) {
			return;
		}

		const switchMicrophoneStatus = async () => {
			await switchMicrophoneStatusAsync(turnOnMicrophone);
		}

		switchMicrophoneStatus();
	}, [connection, stream, turnOnMicrophone]);

	useEffect(() => {
		if (connection === null) {
			return;
		}

		const switchCameraStatus = async () => {
			await switchCameraStatusAsync(turnOnCamera, setCameraExecute);
		}

		switchCameraStatus();
	}, [connection, stream, turnOnCamera]);

	const leaveFromCall = () => {
		stopMediaData();

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

	if (!canLeave) {
		return (
			<>
				<CommunicationMenu
					currentMenuItem={1}
				/>
				<div className="voice">
					<div className="voice__title">
						<div>{chatName}</div>
						<div className="tools">{t("Connecting")}</div>
					</div>
					<VoiceChatMembers
						roomId={roomId}
						connection={connection}
						peerConnectionsRef={peerConnectionsRef}
						micStatus={turnOnMicrophone}
						cameraStatus={turnOnCamera}
						stream={stream}
					/>
				</div>
			</>
		);
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
						{/*{screenSharingActive &&*/}
						{/*	<div className="device">*/}
						{/*		<FontAwesomeIcon*/}
						{/*			icon={screenSharing ? faDisplay : faLinkSlash}*/}
						{/*			title={screenSharing ? t("TurnOffScreenSharing") : t("TurnOnScreenSharing")}*/}
						{/*			className="device__screen"*/}
						{/*			onClick={() => setScreenSharing(!screenSharing)}*/}
						{/*		/>*/}
						{/*	</div>*/}
						{/*}*/}
						{turnOnCameraActive &&
							<div className="device" style={{ opacity: cameraExecute ? 0.4 : 1 }}>
								<FontAwesomeIcon
									icon={turnOnCamera ? faVideo : faVideoSlash}
									title={turnOnCamera ? t("TurnOffCamera") : t("TurnOnCamera")}
									className="device__camera"
									onClick={cameraExecute ? null : () => setTurnOnCamera(!turnOnCamera)}
								/>
								{/*<FontAwesomeIcon*/}
								{/*	icon={openVideoSettings ? faAngleDown : faAngleUp}*/}
								{/*	title={t("Setting")}*/}
								{/*	className="device__settings"*/}
								{/*	onClick={handleOpenVideoSettings}*/}
								{/*/>*/}
								{/*{openVideoSettings &&*/}
								{/*	<VoiceChatAudioDeviceSettings />*/}
								{/*}*/}
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
							{openAudioSettings &&
								<VoiceChatAudioDeviceSettings
									t={t}
									peerConnectionsRef={peerConnectionsRef}
									turnOnMicrophone={turnOnMicrophone}
									stream={stream}
									audioInputDeviceIdRef={audioInputDeviceIdRef}
									audioOutputDeviceIdRef={audioOutputDeviceIdRef}
								/>
							}
						</div>
						<div className="btn-shadow" title={t("Leave")} onClick={leaveFromCall}>
							<FontAwesomeIcon
								icon={faRightFromBracket}
							/>
							<div>{t("Leave")}</div>
						</div>
					</div>
				</div>
				<VoiceChatMembers
					roomId={roomId}
					connection={connection}
					peerConnectionsRef={peerConnectionsRef}
					micStatus={turnOnMicrophone}
					cameraStatus={turnOnCamera}
					stream={stream}
				/>
			</div>
		</>
	);
}

export default memo(VoiceChat);