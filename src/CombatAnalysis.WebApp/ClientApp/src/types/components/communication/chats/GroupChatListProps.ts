import { SetStateAction } from "react";
import { SelectedChat } from "../SelectedChat";

export interface GroupChatListProps {
    meId: string;
    selectedChat: any;
    setSelectedChat: (value: SetStateAction<SelectedChat>) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    t: (key: string) => string;
    setShowCreateGroupChat: (value: SetStateAction<boolean>) => void;
}