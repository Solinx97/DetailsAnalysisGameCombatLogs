import { SetStateAction } from "react";
import { PersonalChat } from "../../../PersonalChat";

export interface GroupChatMessageInputProps {
    hubConnection: any;
    chat: PersonalChat;
    meId: string;
    setAreLoadingOldMessages: (value: SetStateAction<boolean>) => void;
    t: (key: string) => string;
}