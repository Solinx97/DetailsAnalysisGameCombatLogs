
const SystemChatMessage = ({ message }) => {
    const getMessageTime = () => {
        const getDate = new Date(message?.time);
        const time = `${getDate.getHours()}:${getDate.getMinutes()}`;

        return time;
    }

    return (
        <div className="chat-messages__content">
            <div className="system-message">
                <div className="system-message__time">{getMessageTime()}</div>
                <div className="system-message__message">{message?.message}</div>
            </div>
        </div>
    );
}

export default SystemChatMessage;