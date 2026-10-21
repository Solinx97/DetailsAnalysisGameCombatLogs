import type { RootState } from '@/app/Store';
import ChatHubContext from '@/context/ChatHubContext';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import logger from '@/utils/Logger';
import {
	LiveKitRoom,
} from '@livekit/components-react';
import { memo, useContext, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';
import { useCreateCallMutation, useGetVoiceChatTokenMutation, useIsVoiceChatExistQuery } from '../api/VoiceChat.api';
import type { CreateVoiceChatModel } from '../types/CreateVoiceChatModel';
import type { VoiceChatTokenResponse } from '../types/VoiceChatTokenResponse';
import AudioConferenceRoom from './AudioConferenceRoom';

import '@livekit/components-styles';

import './VoiceChat.scss';

const VoiceChat: React.FC = () => {
	const { t } = useTranslation('communication/chats/voiceChat');

	const myself = useSelector((state: RootState) => state.user.value);
	const navigate = useNavigate();

	const [tokenResponse, setTokenResponse] = useState<VoiceChatTokenResponse | null>(null);

	const { roomId, chatName } = useParams();

	const context = useContext(ChatHubContext);
	if (!context) {
		throw new Error("Child must be inside ChatHubContext.Provider");
	}

	const { data: isChatEixst } = useIsVoiceChatExistQuery(parseInt(roomId ?? "0"));
	const [createVoiceChat] = useCreateCallMutation();
	const [getToken] = useGetVoiceChatTokenMutation();

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

		const getTokenAsync = async () => {
			try {
				const recToken = await getToken({ voiceChatId: parseInt(roomId ?? "0"), participantName: myself?.username }).unwrap();
				setTokenResponse(recToken);
			} catch (e) {
				logger.error("Failed to connect to voice chat", e);
			}
		}

		if (isChatEixst === false) {
			createAsync();
		}

		getTokenAsync();
	}, [isChatEixst]);

	useEffect(() => {
		let isCancelled = false;

		const connect = async () => {
			if (isCancelled || !roomId) {
				return;
			}

			await context.vocieChatEvents.connectToChatAsync(roomId);
		}

		connect();

		return () => {
			isCancelled = true;

			context.vocieChatEvents.leaveFromChatAsync(roomId ?? "0");
		};
	}, []);

	if (!tokenResponse) {
		return (
			<>
				<CommunicationMenu currentMenuItem={1} />
				<div className="voice">
					<div className="voice__title">
						<div className="tools">{t("Connecting")}</div>
					</div>
				</div>
			</>
		);
	}

	return (
		<>
			<CommunicationMenu currentMenuItem={1} />
			<div className="voice-chat">
				<div className="voice-chat__title">
					<div>{t("ChatName")}:</div>
					<div className="name">{chatName}</div>
				</div>
				<LiveKitRoom
					serverUrl={tokenResponse?.serverUrl}
					token={tokenResponse?.token}
					connect={true}
					audio={true}
					video={false}
					onError={(error) => {
						console.error("LiveKit error:", error);
					}}
					onDisconnected={() => {
						navigate('/chats');
					}}
				>
					<AudioConferenceRoom />
				</LiveKitRoom>
			</div>
		</>
	);
}

export default memo(VoiceChat);