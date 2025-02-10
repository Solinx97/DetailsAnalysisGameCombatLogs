import { SetStateAction } from "react";
import { SelectedChat } from "../SelectedChat";

export interface GroupChatListItemProps {
    chatId: number;
    meInChatId: string;
    setSelectedGroupChat: (value: SetStateAction<SelectedChat>) => void;
    subscribeToUnreadGroupMessagesUpdated: (meInChatId: string, callback: any) => void;
}