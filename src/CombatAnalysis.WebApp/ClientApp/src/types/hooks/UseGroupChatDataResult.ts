import { GroupChat } from '../GroupChat';
import { GroupChatData } from './GroupChatData';

export interface UseGroupChatDataResult {
    groupChatData: GroupChatData;
    getMoreMessagesAsync: (offset: number) => Promise<GroupChat[]>;
}