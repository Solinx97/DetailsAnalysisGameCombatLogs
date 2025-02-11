import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { GroupChat } from "../../../GroupChat";

export interface GroupChatTitleProps {
    me: AppUser;
    chat: GroupChat;
    settingsIsShow: boolean;
    setSettingsIsShow: (value: SetStateAction<boolean>) => void;
    haveMoreMessages: boolean;
    setHaveMoreMessage: (value: SetStateAction<boolean>) => void;
    loadMoreMessagesAsync: () => Promise<void>;
    t: (key: string) => string;
}