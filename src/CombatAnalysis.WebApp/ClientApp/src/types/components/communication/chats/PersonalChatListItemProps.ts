import { SetStateAction } from "react";
import { PersonalChat } from "../../../PersonalChat";
import { SelectedChat } from "../SelectedChat";

export interface PersonalChatListItemProps {
    chat: PersonalChat;
    setSelectedChat: (value: SetStateAction<SelectedChat>) => void;
    companionId: string;
    meId: string;
    subscribeToUnreadPersonalMessagesUpdated: (meId: string, callback: any) => void;
}