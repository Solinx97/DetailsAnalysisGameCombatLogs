import { MessageStatus } from '@/shared/helpers/EnumHelper';
import logger from '@/utils/Logger';
import { useCreateGroupChatMessageMutation } from '../api/GroupChatMessage.api';
import { useCreatePersonalChatMessageMutation } from '../api/PersonalChatMessage.api';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';

const useMessageCreate = () => {
    const [creatPersonalMessage] = useCreatePersonalChatMessageMutation();
    const [creatGroupMessage] = useCreateGroupChatMessageMutation();
    
    const createPersonalMessageAsync = async (initiatorId: string, initiatorUsername: string, message: string, chatId: number) => {
        try {
            const personalChatMessage: PersonalChatMessageModel = {
                id: "",
                username: initiatorUsername,
                message: message,
                time: new Date(),
                status: MessageStatus["SENDING"],
                type: 0,
                markedType: 0,
                isEdited: false,
                personalChatId: chatId,
                appUserId: initiatorId
            };

            await creatPersonalMessage(personalChatMessage).unwrap();
        } catch (error) {
            logger.error("Failed to create personal chat message:", error);
        }
    }

    const createGrouplMessageAsync = async (initiatorId: string, initiatorUsername: string, message: string, chatId: number) => {
        try {
            const groupChatMessage: GroupChatMessageModel = {
                id: 0,
                username: initiatorUsername,
                message: message,
                time: new Date(),
                status: MessageStatus["SENDING"],
                type: 0,
                markedType: 0,
                isEdited: false,
                groupChatId: chatId,
                groupChatUserId: initiatorId
            };

            await creatGroupMessage(groupChatMessage).unwrap();
        } catch (error) {
            logger.error("Failed to create group chat message:", error);
        }
    }

    return { createPersonalMessageAsync, createGrouplMessageAsync };
}

export default useMessageCreate;