import { useGetGroupChatByIdQuery } from '../../../store/api/ChatApi';

const MyGroupChat = ({ groupChatId, setSelectedGroupChat }) => {
    const { data: groupChat, isLoading } = useGetGroupChatByIdQuery(groupChatId);

    if (isLoading) {
        return <></>;
    }

    return (
        <span onClick={() => setSelectedGroupChat(groupChat)}>
            <div><strong>{groupChat?.name}</strong></div>
            <div className="last-message" title={groupChat?.lastMessage}>{groupChat?.lastMessage}</div>
        </span>
    );
}

export default MyGroupChat;