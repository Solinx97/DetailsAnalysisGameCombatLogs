import { useEffect, useState, useRef } from 'react';
import { useSelector } from 'react-redux';
import PersonalChat from './PersonalChat';
import GroupChat from './GroupChat';
import { faCommentMedical, faRightToBracket } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../styles/chats/chats.scss";

const Chats = () => {
    const user = useSelector((state) => state.user.value);

    const [myGroupChatsRender, setMyGroupChatsRender] = useState(<></>);
    const [personalChatsRender, setPersoanlChatsRender] = useState(<></>);
    const [allUsersRender, setAllUsersRender] = useState(<></>);
    const [newGroupChatsRender, setNewGroupChatsRender] = useState(<></>);
    const [selectedGroupChat, setSelectedGroupChat] = useState(null);
    const [selectedPersonalChat, setSelectedPersonalChat] = useState(null);
    const [allUsers, setAllUsers] = useState(null);
    const [allMyGroupChats, setAllMyGroupChats] = useState(null);
    const [allMyPersonalChats, setAllMyPersonalChats] = useState(null);
    const [newGroupChats, setNewGroupChats] = useState(null);
    const [chatIsLeft, setChatIsLeft] = useState(false);
    const chatsUpdateInterval = 200;

    const usersDatalist = useRef(null);
    const usernameInput = useRef(null);
    const groupChatsDatalist = useRef(null);
    const groupChatNameInput = useRef(null);

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

    useEffect(() => {
        async function getAllUsers() {
            await getAllUsersAsync();
        };

        if (user !== null) {
            getAllUsers();
        }

        let groupChatUpdateInterval = setInterval(async () => {
            await getMyGroupChatUsersAsync();
        }, chatsUpdateInterval);
        let personalChatUpdateInterval = setInterval(async () => {
            await getMyPersonalChatsAsync();
        }, chatsUpdateInterval);

        return () => {
            clearInterval(groupChatUpdateInterval);
            clearInterval(personalChatUpdateInterval);
        };
    }, [user])

    useEffect(() => {
        async function getNewGroupChats() {
            await getNewGroupChatsAsync();
        }

        if (allMyGroupChats !== null) {
            getNewGroupChats();
        }
    }, [allMyGroupChats]);

    const getMyGroupChatUsersAsync = async () => {
        const response = await fetch(`/api/v1/GroupChatUser/${user.id}`);
        const status = response.status;
        let myGroupChats = [];

        if (status === 200) {
            const myGroupChatUsers = await response.json();
            
            for (let i = 0; i < myGroupChatUsers.length; i++) {
                let myGroupChat = await getMyGroupChatsAsync(myGroupChatUsers[i].groupChatId);
                myGroupChats.push(myGroupChat);
            }

            setAllMyGroupChats(myGroupChats);
            fillMyGroupChatList(myGroupChats);
        }
    }

    const getMyGroupChatsAsync = async (groupChatId) => {
        const response = await fetch(`/api/v1/GroupChat/${groupChatId}`);
        const status = response.status;

        if (status === 200) {
            var chat = await response.json();
            return chat;
        }

        return null;
    }

    const fillMyGroupChatList = (chats) => {
        const list = chats.map((element) => myGroupChats(element));

        setMyGroupChatsRender(list);
    }

    const myGroupChats = (element) => {
        return (<li key={element.id} onClick={() => setSelectedGroupChat(element)}>
            <div><strong>{element.name}</strong></div>
            <div className="last-message">{element.lastMessage}</div>
        </li>);
    }

    const getMyPersonalChatsAsync = async () => {
        const response = await fetch(`/api/v1/PersonalChat/${user.id}`);
        const status = response.status;

        if (status === 200) {
            const chats = await response.json();

            setAllMyPersonalChats(chats);
            fillMyPersonalChatList(chats);
        }
    }

    const fillMyPersonalChatList = (chats) => {
        const list = chats.map((element) => myPersonalChats(element));

        setPersoanlChatsRender(list);
    }

    const myPersonalChats = (element) => {
        return (<li key={element.id} onClick={() => setSelectedPersonalChat(element)}>
            <div><strong>{element.companionUsername}</strong></div>
            <div className="last-message">{element.lastMessage}</div>
        </li>);
    }

    const getAllUsersAsync = async () => {
        const response = await fetch("/api/v1/Account");
        const status = response.status;

        if (status === 200) {
            const users = await response.json();
            const otherUsers = users.filter((oneUser) => oneUser.id !== user.id);
            setAllUsers(otherUsers);
        }
    }

    const allUsersHandler = (e) => {
        const val = e.currentTarget.value;

        var filterdUsers = allUsers.filter((user) => user.email.startsWith(val));
        fillOtherUser(filterdUsers)
    }

    const fillOtherUser = (users) => {
        const list = users.map((element) => setUpOtherUsers(element));

        setAllUsersRender(list);
    }

    const setUpOtherUsers = (element) => {
        return (<option key={element.id} data-content={element.id}>{element.email}</option>);
    }

    const getNewGroupChatsAsync = async () => {
        const response = await fetch("/api/v1/GroupChat");
        const status = response.status;

        if (status === 200) {
            const allGroupChats = await response.json();
            const allNewGroupChats = allGroupChats.filter((chat) => {
                let macthes = allMyGroupChats.filter((myChat) => myChat.id === chat.id);
                return macthes.length === 0;
            })

            setNewGroupChats(allNewGroupChats);
        }
    }

    const newGroupsHandler = (e) => {
        const val = e.currentTarget.value;

        var filteredNewGroupChats = newGroupChats.filter((chat) => chat.name.startsWith(val));
        fillNewGroupChats(filteredNewGroupChats)
    }

    const fillNewGroupChats = (chats) => {
        const list = chats.map((element) => setUpNewGroupChats(element));

        setNewGroupChatsRender(list);
    }

    const setUpNewGroupChats = (element) => {
        return (<option key={element.id} data-content={element.id}>{element.name}</option>);
    }

    const startNewChatHandler = async () => {
        const selectedUserId = usersDatalist.current.options[0].getAttribute("data-content");
        const selectedUserEmail = usersDatalist.current.options[0].innerText;
        const duplicateOfPersonalChats = allMyPersonalChats.filter((chat) => chat.initiatorId === selectedUserId || chat.companionId == selectedUserId);

        if (duplicateOfPersonalChats.length == 0) {
            await createNewPersonalChatAsync(selectedUserId, selectedUserEmail);
        }

        usernameInput.current.value = "";
    }

    const createNewPersonalChatAsync = async (companionId, companionEmail) => {
        let newPersonalChat = {
            id: 0,
            initiatorId: user.id,
            initiatorUsername: user.email,
            companionId: companionId,
            companionUsername: companionEmail,
            lastMessage: " ",
        };

        await fetch("/api/v1/PersonalChat", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newPersonalChat)
        });
    }

    const joinToGroupChatHandler = async () => {
        const selectedGroupChatId = groupChatsDatalist.current.options[0].getAttribute("data-content");
        const duplicateOfGroupChats = allMyGroupChats.filter((chat) => chat.id === selectedGroupChatId);

        if (duplicateOfGroupChats.length == 0) {
            await joinToGroupChatAsync(selectedGroupChatId);
        }

        groupChatNameInput.current.value = "";
    }

    const joinToGroupChatAsync = async (groupChatId) => {
        let groupChatUser = {
            id: 0,
            groupChatId: groupChatId,
            userId: user.id,
        };

        await fetch("/api/v1/GroupChatUser", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(groupChatUser)
        });
    }

    const render = () => {
        let chat = <div className="select-chat">Select chat</div>;
        if (!chatIsLeft && selectedPersonalChat !== null) {
            chat = <PersonalChat chat={selectedPersonalChat} setChatIsLeaft={setChatIsLeft} />;
        }
        else if (!chatIsLeft && selectedGroupChat !== null) {
            chat = <GroupChat chat={selectedGroupChat} setChatIsLeaft={setChatIsLeft} />;
        }

        return (<div className="communication">
            <div className="search">
                <div className="search__search-people">
                    <div className="search__search-people_name">Search people</div>
                    <input type="text" list="users" className="form-control" onChange={allUsersHandler} ref={usernameInput} />
                    <datalist id="users" ref={usersDatalist}>
                        {allUsersRender}
                    </datalist>
                    <FontAwesomeIcon icon={faCommentMedical} title="Start chat" onClick={startNewChatHandler} />
                </div>
                <div className="search__search-groups">
                    <div className="search__search-people_name">Search groups</div>
                    <input type="text" list="groups" className="form-control" onChange={newGroupsHandler} ref={groupChatNameInput} />
                    <datalist id="groups" ref={groupChatsDatalist}>
                        {newGroupChatsRender}
                    </datalist>
                    <FontAwesomeIcon icon={faRightToBracket} title="Join to group" onClick={joinToGroupChatHandler} />
                </div>
            </div>
            <div className="chats">
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
            </div>
         </div>);
    }

    return render();
}

export default Chats;