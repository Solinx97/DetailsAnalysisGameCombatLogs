import ChatMessageMenu from './ChatMessageMenu';
import { useState } from 'react';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import User from '../User';

const ChatMessageTitle = ({ me, itIsMe, setEditModeIsOn, openMessageMenu, editModeIsOn, deleteMessageAsync, message }) => {
    const [userInformation, setUserInformation] = useState(null);

    const { data: user, isLoading } = useGetCustomerByIdQuery(message?.customerId);

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
                    targetCustomerId={user.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                />
            </div>
            {userInformation}
        </>
    );
}

export default ChatMessageTitle;