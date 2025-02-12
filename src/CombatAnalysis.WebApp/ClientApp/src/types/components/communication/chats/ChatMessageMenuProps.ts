import { SetStateAction } from "react";
import { GroupChatMessage } from "../../../GroupChatMessage";
import { PersonalChatMessage } from "../../../PersonalChatMessage";

export interface ChatMessageMenuProps {
    editModeIsOn: boolean;
    setEditModeIsOn: (value: SetStateAction<boolean>) => void;
    deleteMessageAsync: (messageId: number) => Promise<void>;
    message: PersonalChatMessage | GroupChatMessage;
}