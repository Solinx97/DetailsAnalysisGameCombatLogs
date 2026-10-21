import { APP_CONFIG } from '@/config/appConfig';
import type { IVoiceChatHubEvents } from '@/shared/interfaces/IVoiceChatHubEvents';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import { type RefObject } from 'react';

const useVoiceChatHub = (
	vocieChatHubConnectionRef: RefObject<signalR.HubConnection | null>
): IVoiceChatHubEvents => {
	const voiceHubURL = APP_CONFIG.chatApiHubs.voiceChat;
	const useLongPoll = import.meta.env.VITE_USE_HUB_LOONG_POLL === "true" ? true : false;
	
	const createHubConnection = (url: string): signalR.HubConnection => {
		const hubConnection = new signalR.HubConnectionBuilder()
			.withUrl(url, {
				withCredentials: true,
				transport: useLongPoll
					? signalR.HttpTransportType.LongPolling
					: signalR.HttpTransportType.LongPolling | signalR.HttpTransportType.WebSockets
			})
			.withAutomaticReconnect()
			.build();

		return hubConnection;
	}

	const connectToChatAsync = async (roomId: string) => {
		try {
			const connection = createHubConnection(voiceHubURL);

			await connection.start();
			await connection.invoke("JoinToRoom", roomId);
			
			vocieChatHubConnectionRef.current = connection;
		} catch (e) {
			logger.error("Failed to connect to voice chat", e);
		}
	}

	const leaveFromChatAsync = async (roomId: string) => {
		await vocieChatHubConnectionRef.current?.invoke("LeaveFromRoom", roomId);
	}

	const getEvents = () => {
		const events: IVoiceChatHubEvents = {
			connectToChatAsync, leaveFromChatAsync
		}

		return events;
	}

	return getEvents();
}

export default useVoiceChatHub;