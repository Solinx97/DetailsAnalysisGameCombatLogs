import { SelectedChat } from "../Chat";

export interface GroupChatListProps {
    meId: string;
    selectedChat: any;
    setSelectedChat: (key: SelectedChat) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    t: (key: string) => string;
    setShowCreateGroupChat: (key: boolean) => void;
}