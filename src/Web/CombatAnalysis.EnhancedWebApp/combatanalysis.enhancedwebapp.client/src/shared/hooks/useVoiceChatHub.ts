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

	const connectToVoiceChatUsersCount = async (roomId: string) => {
		try {
			const connection = createHubConnection(voiceHubURL);

			await connection.start();
			const count = await connection.invoke("SubscribeToVoiceChatUsersCount", roomId);

			vocieChatHubConnectionRef.current = connection;

			return count;
		} catch (e) {
			logger.error("Failed to subscribe to voice chat users count", e);
		}
	}

	const subscribeToVoiceChatUsersCount = (callback: (count: number) => void) => {
		vocieChatHubConnectionRef.current?.on("ConnectedUsersCount", callback);
	}

	const unsubscribeToVoiceChatUsersCount = (callback: (count: number) => void) => {
		vocieChatHubConnectionRef.current?.off("ConnectedUsersCount", callback);
	}

	const leaveFromChatAsync = async (roomId: string) => {
		await vocieChatHubConnectionRef.current?.invoke("LeaveFromRoom", roomId);
	}

	const leaveFromChatUsersCountAsync = async (roomId: string) => {
		await vocieChatHubConnectionRef.current?.invoke("UnsubscribeFromVoiceChatUsersCount", roomId);
	}

	const getEvents = () => {
		const events: IVoiceChatHubEvents = {
			connectToChatAsync, connectToVoiceChatUsersCount, subscribeToVoiceChatUsersCount, unsubscribeToVoiceChatUsersCount, leaveFromChatAsync, leaveFromChatUsersCountAsync
		}

		return events;
	}

	return getEvents();
}

export default useVoiceChatHub;