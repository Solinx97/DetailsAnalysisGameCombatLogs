import { SetStateAction } from "react";
import { PersonalChat } from "../../../PersonalChat";
import { SelectedChat } from "../SelectedChat";

export interface PersonalChatTitleProps {
    chat: PersonalChat;
    companionUsername: string;
    setSelectedChat: (value: SetStateAction<SelectedChat>) => void;
    haveMoreMessages: boolean;
    setHaveMoreMessage: (value: SetStateAction<boolean>) => void;
    loadMoreMessagesAsync: () => Promise<void>;
    t: (key: string) => string;
}