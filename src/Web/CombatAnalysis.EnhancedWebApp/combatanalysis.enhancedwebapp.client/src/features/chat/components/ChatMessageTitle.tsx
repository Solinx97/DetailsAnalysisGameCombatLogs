import { useState, type JSX } from 'react';
import User from '../../user/components/User';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';

interface ChatMessageTitleProps {
    itIsMe: boolean;
    message: PersonalChatMessageModel | GroupChatMessageModel;
}

const ChatMessageTitle: React.FC<ChatMessageTitleProps> = ({ itIsMe, message }) => {
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const getMessageTime = () => {
        const getDate = new Date(message?.time);
        const time = `${getDate.getHours()}:${getDate.getMinutes() }`;

        return time;
    }

    return (
        <>
            <div className={`message-title ${itIsMe ? 'me' : 'another'}`}>
                <div className="content">
                    <div className="message-time">
                        <div>{getMessageTime()}</div>
                    </div>
                    <User
                        targetUserId={'appUserId' in message ? message.appUserId : message.groupChatUserId}
                        targetUsername={message?.username}
                        setUserInformation={setUserInformation}
                    />
                </div>
                <div className="chat-user-information">
                    {userInformation}
                </div>
            </div>
        </>
    );
}

export default ChatMessageTitle;