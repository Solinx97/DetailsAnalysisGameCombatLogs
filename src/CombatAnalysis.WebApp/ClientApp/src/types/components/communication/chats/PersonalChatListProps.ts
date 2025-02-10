import { SetStateAction } from "react";
import { SelectedChat } from "../SelectedChat";

export interface PersonalChatListProps {
    meId: string;
    selectedChat: any;
    setSelectedChat: (value: SetStateAction<SelectedChat>) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    t: (key: string) => string;
}