import ChatProvider from '@/providers/ChatProvider';
import Chats from './Chats';

const ChatApp = () => {
    return (
        <ChatProvider>
            <Chats />
        </ChatProvider>
    );
}

export default ChatApp;