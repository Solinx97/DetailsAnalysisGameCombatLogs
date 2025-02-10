import { SelectedChat } from "../Chat";

export interface PersonalChatListProps {
    meId: string;
    selectedChat: any;
    setSelectedChat: (key: SelectedChat) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    t: (key: string) => string;
}