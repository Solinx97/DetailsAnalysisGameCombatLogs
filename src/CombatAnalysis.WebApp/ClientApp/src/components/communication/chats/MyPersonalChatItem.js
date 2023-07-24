
const MyPersonalChatItem = ({ personalChat, selectedPersonalChat, setSelectedPersonalChat }) => {
    return (
        <span className={selectedPersonalChat !== null && selectedPersonalChat.id === personalChat.id ? "active" : ""}
            onClick={() => setSelectedPersonalChat(personalChat)}>
            <div><strong>{personalChat.companionUsername}</strong></div>
            <div className="last-message" title={personalChat.lastMessage}>{personalChat.lastMessage}</div>
        </span>
    );
}

export default MyPersonalChatItem;