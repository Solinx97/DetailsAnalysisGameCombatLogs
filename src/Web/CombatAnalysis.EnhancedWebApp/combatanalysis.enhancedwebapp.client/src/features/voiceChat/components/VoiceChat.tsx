import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import logger from '@/utils/Logger';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import { useIsVoiceChatExistQuery, useCreateCallMutation } from '../api/VoiceChat.api';
import useVoiceChatHub from '../hooks/useVoiceChatHub';
import type { CreateVoiceChatModel } from '../types/CreateVoiceChatModel';
import VoiceChatContent from './VoiceChatContent';
import VoiceChatToolsBar from './VoiceChatToolsBar';

import './VoiceChat.scss';

const VoiceChat: React.FC = () => {
	const { t } = useTranslation('communication/chats/voiceChat');

	const voiceHubURL = `${APP_CONFIG.chatApiHubs.url}${APP_CONFIG.chatApiHubs.voiceChat}`;

	const myself = useSelector((state: RootState) => state.user.value);

	const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
	const [turnOnCamera, setTurnOnCamera] = useState(false);
	const [audioOutputDeviceId, setAudioOutputDeviceId] = useState("");

	const [screenSharing, setScreenSharing] = useState(false);
	const [screenSharingIsActivated, setScreenSharingIsActivated] = useState(false);
	const [haveControllBar, setHaveControllBar] = useState(false);

	const screenSharingVideoRef = useRef<HTMLVideoElement | null>(null);

	const { roomId, chatName } = useParams();

	const { properties, methods } = useVoiceChatHub(roomId ?? "0");

	const { data: isChatEixst } = useIsVoiceChatExistQuery(parseInt(roomId ?? "0"));
	const [createVoiceChat] = useCreateCallMutation();

	useEffect(() => {
		if (isChatEixst === undefined || !myself) {
			return;
		}
		
		const createAsync = async () => {
			try {
				const voiceChat: CreateVoiceChatModel = {
					groupChatId: parseInt(roomId ?? "0"),
					appUserId: myself.id
				}

				await createVoiceChat(voiceChat).unwrap();
			} catch (e) {
            	logger.error("Failed to create voice chat", e);
			}
		}

		if (isChatEixst === false) {
			createAsync();
		}
	}, [isChatEixst]);

	useEffect(() => {
		if (!properties.hubConnection) {
			return;
		}

		return () => {
			(async () => {
				await methods.leaveFromChatAsync();
			})();
		}
	}, [properties.hubConnection]);

	useEffect(() => {
		if (!myself) {
			return;
		}

		(async () => {
			await methods.connectToChatAsync(voiceHubURL, setHaveControllBar);
		})();
	}, [myself]);

	useEffect(() => {
		if (!properties.hubConnection) {
			return;
		}

		(async () => {
			await methods.startScreenSharingAsync(screenSharing, setScreenSharingIsActivated);
		})();
	}, [properties.hubConnection, screenSharing]);

	useEffect(() => {
		if (!screenSharing || !screenSharingVideoRef.current) {
			return;
		}

		screenSharingVideoRef.current.srcObject = properties.localStreamRef.current;
		screenSharingVideoRef.current.muted = true;
		screenSharingVideoRef.current.autoplay = true;
	}, [screenSharing]);

	useEffect(() => {
		if (screenSharingIsActivated) {
			return;
		}

		screenSharingVideoRef.current = null;
		setScreenSharing(false);
	}, [screenSharingIsActivated]);

	const renderVoiceChatContent = () => (
		<VoiceChatContent
			roomId={roomId ?? ""}
			hubConnection={properties.hubConnection}
			peerConnections={properties.peerConnectionsRef.current}
			stream={properties.localStreamRef.current}
			mediaRequestsAsync={methods.mediaRequestsAsync}
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
					{renderVoiceChatContent()}
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
					{renderVoiceChatToolsBar()}
				</div>
				{renderVoiceChatContent()}
			</div>
		</>
	);
}

export default memo(VoiceChat);