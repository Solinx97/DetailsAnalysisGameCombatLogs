import React, { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import useRTCVoiceChat from '../../../../hooks/useRTCVoiceChat';
import CommunicationMenu from '../../CommunicationMenu';
import VoiceChatContent from './VoiceChatContent';
import VoiceChatToolsBar from './VoiceChatToolsBar';

import '../../../../styles/communication/chats/voice.scss';

const VoiceChat = () => {
	const { t } = useTranslation("communication/chats/voiceChat");

	const me = useSelector((state) => state.user.value);

	const [haveControllBar, setHaveControllBar] = useState(false);
	const [turnOnMicrophone, setTurnOnMicrophone] = useState(true);
	const [turnOnCamera, setTurnOnCamera] = useState(false);

	const [screenSharing, setScreenSharing] = useState(false);
	const [screenSharingActive, setScreenSharingActive] = useState(false);

	const screenSharingVideoRef = useRef(null);

	const { roomId, chatName } = useParams();

	const { properties, methods } = useRTCVoiceChat(roomId);

	useEffect(() => {
		if (!properties.connection) {
			return;
		}

		return () => {
			const cleanup = async () => {
				methods.stopMediaData();
				await methods.cleanupAsync();
			}

			cleanup();
		}
	}, [properties.connection]);

	useEffect(() => {
		if (!me) {
			return;
		}

		const connectToChat = async () => {
			const signalingAddress = "/voiceChatHub";
			await methods.connectToChatAsync(signalingAddress, setHaveControllBar);
		}

		connectToChat();
	}, [me]);

	useEffect(() => {
		setScreenSharingActive(screenSharing);

		if (!properties.connection || screenSharingActive) {
			return;
		}

		const handleScreenSharing = async () => {
			if (screenSharing) {
				const screenSharingStream = await methods.startScreenSharingAsync();
				if (!screenSharingStream) {
					screenSharingVideoRef.current = null;
					setScreenSharing(false);

					return;
				}

				screenSharingVideoRef.current.srcObject = screenSharingStream;
				screenSharingVideoRef.current.autoplay = true;

				screenSharingStream.getVideoTracks()[0].addEventListener("ended", () => {
					screenSharingVideoRef.current = null;
					setScreenSharing(false);
				});
			} else {
				await methods.stopScreenSharingAsync();
			}
		}

		handleScreenSharing();
	}, [properties.connection, screenSharing, screenSharingActive]);

	const renderVoiceChatContent = () => (
		<VoiceChatContent
			roomId={roomId}
			connection={properties.connection}
			peerConnectionsRef={properties.peerConnectionsRef}
			stream={properties.stream}
			micStatus={turnOnMicrophone}
			cameraStatus={turnOnCamera}
			screenSharing={screenSharing}
			setScreenSharing={setScreenSharing}
			screenSharingVideoRef={screenSharingVideoRef}
		/>
	)

	const renderVoiceChatToolsBar = () => (
		<VoiceChatToolsBar
			t={t}
			properties={properties}
			methods={methods}
			screenSharing={screenSharing}
			setScreenSharing={setScreenSharing}
			turnOnCamera={turnOnCamera}
			setTurnOnCamera={setTurnOnCamera}
			turnOnMicrophone={turnOnMicrophone}
			setTurnOnMicrophone={setTurnOnMicrophone}
		/>
	)

	if (!haveControllBar) {
		return (
			<>
				<CommunicationMenu currentMenuItem={1} />
				<div className="voice">
					<div className="voice__title">
						<div>{chatName}</div>
						<div className="tools">{t("Connecting")}</div>
					</div>
					{
						renderVoiceChatContent()
					}
				</div>
			</>
		);
	}

	return (
		<>
			<CommunicationMenu currentMenuItem={1} />
			<div className="voice">
				<div className="voice__title">
					<div>{chatName}</div>
					{
						renderVoiceChatToolsBar()
					}
				</div>
				{
					renderVoiceChatContent()
				}
			</div>
		</>
	);
}

export default memo(VoiceChat);