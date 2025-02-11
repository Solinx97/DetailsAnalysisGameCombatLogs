import { GroupChatMessage } from '../GroupChatMessage';
import { GroupChatUser } from '../GroupChatUser';

export interface GroupChatData {
    messages: GroupChatMessage[];
    count: number;
    meInChat: GroupChatUser;
    groupChatUsers: GroupChatUser[];
    isLoading: boolean;
}