import ChatProvider from '@/providers/ChatProvider';
import VoiceChat from './VoiceChat';

const VoiceChatApp = () => {
    return (
        <ChatProvider>
            <VoiceChat />
        </ChatProvider>
    );
}

export default VoiceChatApp;