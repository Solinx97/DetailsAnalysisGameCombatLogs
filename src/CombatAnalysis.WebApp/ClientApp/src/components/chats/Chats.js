import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import PersonalChat from './PersonalChat';
import GroupChat from './GroupChat';

import "../../styles/chats/chats.scss";

const Chats = () => {
    const user = useSelector((state) => state.user.value);

    const [myGroupChatsRender, setMyGroupChatsRender] = useState(<></>);
    const [personalChatsRender, setPersoanlChatsRender] = useState(<></>);
    const [selectedGroupChat, setSelectedGroupChat] = useState(null);
    const [selectedPersonalChat, setSelectedPersonalChat] = useState(null);
    const chatsUpdateInterval = 200;

    useEffect(() => {
        async function getMyGroupChatUsers(userId) {
            await getMyGroupChatUsersAsync(userId);
        };

        async function getPersonalChats(userId) {
            await getPersonalChatsAsync(userId);
        };

        let groupChatUpdateInterval = setInterval(() => {
            getMyGroupChatUsers(user.id);
        }, chatsUpdateInterval);
        let personalChatUpdateInterval = setInterval(() => {
            getPersonalChats(user.id);
        }, chatsUpdateInterval);

        return () => {
            clearInterval(groupChatUpdateInterval);
            clearInterval(personalChatUpdateInterval);
        };
    }, []);

    useEffect(() => {
        if (selectedGroupChat !== null) {
            setSelectedPersonalChat(null);
        }
    }, [selectedGroupChat]);

    useEffect(() => {
        if (selectedPersonalChat !== null) {
            setSelectedGroupChat(null);
        }
    }, [selectedPersonalChat]);

    const getMyGroupChatUsersAsync = async (userId) => {
        const response = await fetch(`/api/v1/GroupChatUser/${userId}`);
        const status = response.status;
        let myGroupChats = [];
        if (status === 200) {
            const myGroupChatUsers = await response.json();

            for (let i = 0; i < myGroupChatUsers.length; i++) {
                let myGroupChat = await getMyGroupChatsAsync(myGroupChatUsers[i].groupChatId);
                myGroupChats.push(myGroupChat);
            }

            fillGroupChatList(myGroupChats);
        }
    }

    const getMyGroupChatsAsync = async (groupChatId) => {
        const response = await fetch(`/api/v1/GroupChat/${groupChatId}`);
        const status = response.status;
        if (status === 200) {
            var chat = await response.json();
            return chat;
        }
        else {
            return [];
        }
    }

    const fillGroupChatList = (chats) => {
        const list = chats.map((element) => groupChats(element));

        setMyGroupChatsRender(list);
    }

    const groupChats = (element) => {
        return (<li key={element.id} onClick={() => setSelectedGroupChat(element)}>
            <div><strong>{element.name}</strong></div>
            <div className="last-message">{element.lastMessage}</div>
        </li>);
    }

    const getPersonalChatsAsync = async (userId) => {
        const response = await fetch(`/api/v1/PersonalChat/${userId}`);
        const status = response.status;
        if (status === 200) {
            var chats = await response.json();

            fillPersonalChatList(chats);
        }
    }

    const fillPersonalChatList = (chats) => {
        const list = chats.map((element) => personalChats(element));

        setPersoanlChatsRender(list);
    }

    const personalChats = (element) => {
        return (<li key={element.id} onClick={() => setSelectedPersonalChat(element)}>
            <div><strong>{element.companionUsername}</strong></div>
            <div className="last-message">{element.lastMessage}</div>
        </li>);
    }

    const render = () => {
        let chat = <div>Select chat</div>;
        if (selectedPersonalChat !== null) {
            chat = <PersonalChat chat={selectedPersonalChat} />;
        }
        else if (selectedGroupChat !== null) {
            chat = <GroupChat chat={selectedGroupChat} />;
        }

        return (<div className="chats">
            <div className="chats__my-chats">
                <div>Group chats</div>
                <ul className="chats__my-chats__group-chats">
                    {myGroupChatsRender}
                </ul>
                <div>Personal chats</div>
                <ul className="chats__my-chats__personal-chats">
                    {personalChatsRender}
                </ul>
            </div>
            {chat}
        </div>);
    }

    return render();
}

export default Chats;