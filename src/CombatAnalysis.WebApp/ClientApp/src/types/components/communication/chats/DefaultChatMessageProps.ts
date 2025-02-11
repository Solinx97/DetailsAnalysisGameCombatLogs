import { AppUser } from '../../../AppUser';
import { GroupChatMessage } from '../../../GroupChatMessage';
import { PersonalChatMessage } from '../../../PersonalChatMessage';

export interface DefaultChatMessageProps {
    me: AppUser;
    meInChatId: string;
    reviewerId: string;
    messageOwnerId: string;
    message: PersonalChatMessage | GroupChatMessage;
    updateMessageAsync: (message: PersonalChatMessage | GroupChatMessage) => Promise<void>;
    deleteMessageAsync: (messageId: number) => Promise<void>;
    chatMessagesHubConnection: any;
    subscribeToMessageHasBeenRead: any;
}