
const SystemChatMessage = ({ message }) => {
    const getMessageTime = () => {
        const timeItems = message?.time.split(":");
        const time = `${timeItems[0]}:${timeItems[1]}`;

        return time;
    }

    return (
        <div className="chat-messages__content">
            <div className="system-message">
                <div className="system-message__message">{message?.message}</div>
                <div className="system-message__time">{getMessageTime()}</div>
            </div>
        </div>
    );
}

export default SystemChatMessage;