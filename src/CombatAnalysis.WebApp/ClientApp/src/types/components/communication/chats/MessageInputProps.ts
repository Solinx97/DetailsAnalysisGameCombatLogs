import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { GroupChat } from "../../../GroupChat";
import { GroupChatUser } from "../../../GroupChatUser";
import { PersonalChat } from "../../../PersonalChat";

export interface MessageInputProps {
    chat: PersonalChat | GroupChat;
    meInChat: GroupChatUser | AppUser;
    setAreLoadingOldMessages: (value: SetStateAction<boolean>) => void;
    targetChatType: number;
    t: (key: string) => string;
}