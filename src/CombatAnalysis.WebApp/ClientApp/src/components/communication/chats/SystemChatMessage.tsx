import { SystemChatMessageProps } from '../../../types/components/communication/chats/SystemChatMessageProps';

const SystemChatMessage: React.FC<SystemChatMessageProps> = ({ message }) => {
    return (
        <div className="chat-messages__content">
            <div className="system-message">
                <div className="system-message__message">{message}</div>
            </div>
        </div>
    );
}

export default SystemChatMessage;