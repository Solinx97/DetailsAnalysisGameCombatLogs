import React, { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import useVoiceChatHub from '../../../../hooks/useVoiceChatHub';
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
	const [audioOutputDeviceId, setAudioOutputDeviceId] = useState("");

	const [screenSharing, setScreenSharing] = useState(false);

	const screenSharingVideoRef = useRef(null);

	const { roomId, chatName } = useParams();

	const { properties, methods } = useVoiceChatHub(roomId);

	useEffect(() => {
		if (!properties.hubConnection) {
			return;
		}

		return () => {
			const cleanup = async () => {
				await methods.stopMediaDataAsync();
			}

			cleanup();
		}
	}, [properties.hubConnection]);

	useEffect(() => {
		if (!me) {
			return;
		}

		const connectToChat = async () => {
			const signalingAddress = "/voiceChatHub";
			await methods.connectToChatAsync(me?.id, signalingAddress, haveControllBar, setHaveControllBar);
		}

		connectToChat();
	}, [me]);

	useEffect(() => {
		if (!properties.hubConnection || !properties.stream) {
			return;
		}

		if (screenSharing) {
			setTurnOnCamera(false);
		}

		const handleScreenSharing = async () => {
			const screenSharingStream = await methods.startScreenSharingAsync(screenSharing);
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
		}

		handleScreenSharing();
	}, [properties.stream, screenSharing]);

	const renderVoiceChatContent = () => (
		<VoiceChatContent
			roomId={roomId}
			hubConnection={properties.hubConnection}
			peerConnections={properties.peerConnectionsRef.current}
			stream={properties.stream}
			micStatus={turnOnMicrophone}
			cameraStatus={turnOnCamera}
			screenSharing={screenSharing}
			setScreenSharing={setScreenSharing}
			screenSharingVideoRef={screenSharingVideoRef}
			audioOutputDeviceId={audioOutputDeviceId}
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
			audioOutputDeviceId={audioOutputDeviceId}
			setAudioOutputDeviceId={setAudioOutputDeviceId}
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