import { useState } from 'react';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import User from '../User';
import ChatMessageMenu from './ChatMessageMenu';

const ChatMessageTitle = ({ me, itIsMe, setEditModeIsOn, openMessageMenu, editModeIsOn, deleteMessageAsync, message, messageOwnerId }) => {
    const [userInformation, setUserInformation] = useState(null);

    const { data: user, isLoading } = useGetUserByIdQuery(messageOwnerId);

    const getMessageTime = () => {
        const getDate = new Date(message?.time);
        const time = `${getDate.getHours()}:${getDate.getMinutes() }`;

        return time;
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            <div className={`message-title ${itIsMe ? 'me' : 'another'}`}>
                {openMessageMenu &&
                    <ChatMessageMenu
                        editModeIsOn={editModeIsOn}
                        setEditModeIsOn={setEditModeIsOn}
                        deleteMessageAsync={deleteMessageAsync}
                        message={message}
                    />
                }
                <div className="message-time">
                    <div>{getMessageTime()}</div>
                </div>
                <User
                    me={me}
                    itIsMe={itIsMe}
                    targetUserId={user?.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                />
            </div>
            <div className="chat-user-information">
                {userInformation}
            </div>
        </>
    );
}

export default ChatMessageTitle;