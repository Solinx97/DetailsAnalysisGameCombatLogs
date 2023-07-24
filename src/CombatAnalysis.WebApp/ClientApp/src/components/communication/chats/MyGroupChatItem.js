import { useGetGroupChatByIdQuery } from '../../../store/api/ChatApi';

const MyGroupChatItem = ({ groupChatId, selectedGroupChat, setSelectedGroupChat }) => {
    const { data: groupChat, isLoading } = useGetGroupChatByIdQuery(groupChatId);

    if (isLoading) {
        return <></>;
    }

    return (
        <span className={selectedGroupChat !== null && selectedGroupChat?.id === groupChat?.id ? `active` : ``}
            onClick={() => setSelectedGroupChat(groupChat)}>
            <div><strong>{groupChat?.name}</strong></div>
            <div className="last-message" title={groupChat?.lastMessage}>{groupChat?.lastMessage}</div>
        </span>
    );
}

export default MyGroupChatItem;