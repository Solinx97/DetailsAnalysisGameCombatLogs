import { faArrowDown, faArrowUp, faSquarePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useGetGroupChatUserByUserIdQuery, useGetPersonalChatsByUserIdQuery } from '../../../store/api/ChatApi';
import CreateGroupChat from './CreateGroupChat';
import GroupChat from './GroupChat';
import MyGroupChatItem from './MyGroupChatItem';
import MyPersonalChatItem from './MyPersonalChatItem';
import PersonalChat from './PersonalChat';

import "../../../styles/communication/chats.scss";

const Chats = () => {
    const [, customer] = useAuthentificationAsync();

    const { data: personalChats, isLoading } = useGetPersonalChatsByUserIdQuery(customer?.id);
    const { data: groupChatUsers, isLoading: chatUserIsLoading } = useGetGroupChatUserByUserIdQuery(customer?.id);

    const [selectedGroupChat, setSelectedGroupChat] = useState(null);
    const [selectedPersonalChat, setSelectedPersonalChat] = useState(null);
    const [chatIsLeft, setChatIsLeft] = useState(false);
    const [groupChatsHidden, setGroupChatsHidden] = useState(false);
    const [personalChatsHidden, setPersonalChatsHidden] = useState(false);
    const [createGroupChatIsActive, setCreateGroupChatIsActive] = useState(false);

    useEffect(() => {
        if (selectedGroupChat !== null) {
            setSelectedPersonalChat(null);
            setChatIsLeft(false);
        }
    }, [selectedGroupChat]);

    useEffect(() => {
        if (selectedPersonalChat !== null) {
            setSelectedGroupChat(null);
            setChatIsLeft(false);
        }
    }, [selectedPersonalChat]);

    if (isLoading || chatUserIsLoading) {
        return <></>;
    }

    let chat = <div className="select-chat">Select chat</div>;
    if (!chatIsLeft && selectedPersonalChat !== null) {
        chat = <PersonalChat
            chat={Object.assign({}, selectedPersonalChat)}
            customer={customer}
            setChatIsLeaft={setChatIsLeft}
        />;
    }
    else if (!chatIsLeft && selectedGroupChat !== null) {
        chat = <GroupChat
            chat={Object.assign({}, selectedGroupChat)}
            customer={customer}
            setChatIsLeaft={setChatIsLeft}
        />;
    }

    return (
        <div>
            <div className="chats">
                <div className="chats__my-chats">
                    <div className="chats__my-chats_title">
                        <div>Group chats</div>
                        {groupChatsHidden
                            ? <FontAwesomeIcon
                                icon={faArrowDown}
                                title="Show chats"
                                onClick={() => setGroupChatsHidden(!groupChatsHidden)}
                            />
                            : <FontAwesomeIcon
                                icon={faArrowUp}
                                title="Hide chats"
                                onClick={() => setGroupChatsHidden(!groupChatsHidden)}
                            />
                        }
                        <FontAwesomeIcon
                            className="create"
                            icon={faSquarePlus}
                            title="Create a new group chat"
                            onClick={() => setCreateGroupChatIsActive(!createGroupChatIsActive)}
                        />
                    </div>
                    <ul className="chats__my-chats__group-chats">
                        {
                            groupChatUsers.map((item) => (
                                <li key={item.id}>
                                    <MyGroupChatItem
                                        groupChatId={item.groupChatId}
                                        selectedGroupChat={selectedGroupChat}
                                        setSelectedGroupChat={setSelectedGroupChat}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                    <div className="chats__my-chats_title">
                        <div>Personal chats</div>
                        {personalChatsHidden
                            ? <FontAwesomeIcon
                                icon={faArrowDown}
                                title="Show chats"
                                onClick={() => setPersonalChatsHidden(!personalChatsHidden)}
                            />
                            : <FontAwesomeIcon
                                icon={faArrowUp}
                                title="Hide chats"
                                onClick={() => setPersonalChatsHidden(!personalChatsHidden)}
                            />
                        }
                    </div>
                    <ul className="chats__my-chats__personal-chats">
                        {
                            personalChats?.map((item) => (
                                <li key={item.id}>
                                    <MyPersonalChatItem
                                        personalChat={item}
                                        selectedPersonalChat={selectedPersonalChat}
                                        setSelectedPersonalChat={setSelectedPersonalChat}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                </div>
                {chat}
            </div>
            <CreateGroupChat
                setCreateGroupChatIsActive={setCreateGroupChatIsActive}
                customer={customer}
                createGroupChatIsActive={createGroupChatIsActive}
            />
        </div>
    );
}

export default Chats;