import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { GroupChatMessage } from "../../../GroupChatMessage";
import { PersonalChatMessage } from "../../../PersonalChatMessage";

export interface ChatMessageTitleProps {
    me: AppUser;
    itIsMe: boolean;
    message: PersonalChatMessage | GroupChatMessage;
    setEditModeIsOn: (value: SetStateAction<boolean>) => void;
    openMessageMenu: boolean;
    editModeIsOn: boolean;
    deleteMessageAsync: (messageId: number) => Promise<void>;
    meInChatId: string;
}