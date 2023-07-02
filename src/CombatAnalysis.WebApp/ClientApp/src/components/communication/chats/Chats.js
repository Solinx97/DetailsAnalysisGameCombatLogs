import { faArrowDown, faArrowUp, faSquarePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import GroupChat from './GroupChat';
import PersonalChat from './PersonalChat';

import "../../../styles/communication/chats.scss";

const Chats = ({ isOpenChat, initiatorId, companionId }) => {
    const customer = useSelector((state) => state.customer.value);

    const [myGroupChatsRender, setMyGroupChatsRender] = useState(<></>);
    const [personalChatsRender, setPersoanlChatsRender] = useState(<></>);
    const [selectedGroupChat, setSelectedGroupChat] = useState(null);
    const [selectedPersonalChat, setSelectedPersonalChat] = useState(null);
    const [chatIsLeft, setChatIsLeft] = useState(false);
    const [groupChatsHidden, setGroupChatsHidden] = useState(false);
    const [personalChatsHidden, setPersonalChatsHidden] = useState(false);
    const [createGroupChatIsActive, setCreateGroupChatIsActive] = useState(false);
    const [personalChats, setPersonalChats] = useState([]);
    const [groupChats, setGroupChats] = useState([]);
    const [isOpenPersonalChat, setIsOpenPersonalChat] = useState(isOpenChat);
    const [chatPolicyType, setChatPolicyType] = useState(0);

    const nameInput = useRef(null);
    const shortNameInput = useRef(null);
    const memberNumberSelect = useRef(null);

    const chatsUpdateInterval = 200;

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
    }, [customer])

    useEffect(() => {
        if (isOpenPersonalChat && personalChats.length > 0) {
            openChatFromCommunication(initiatorId, companionId);
        }

        fillMyPersonalChatList(personalChats);
    }, [personalChats])

    useEffect(() => {
        fillMyGroupChatList(groupChats);
    }, [groupChats])

    const openChatFromCommunication = (initiatorId, companionId) => {
        let selectedChat = personalChats.filter((chat) => chat.initiatorId === initiatorId && chat.companionId === companionId);
        if (selectedChat.length === 0) {
            selectedChat = personalChats.filter((chat) => chat.initiatorId === companionId && chat.companionId === initiatorId);
        }

        setSelectedPersonalChat(selectedChat[0]);
        setIsOpenPersonalChat(false);
    }

    const getMyGroupChatUsersAsync = async () => {
        const response = await fetch(`/api/v1/GroupChatUser/${customer.id}`);
        const status = response.status;
        let myGroupChats = [];

        if (status === 200) {
            const myGroupChatUsers = await response.json();
            
            for (let i = 0; i < myGroupChatUsers.length; i++) {
                let myGroupChat = await getMyGroupChatsAsync(myGroupChatUsers[i].groupChatId);
                myGroupChats.push(myGroupChat);
            }

            setGroupChats(myGroupChats);
        }
    }

    const getMyGroupChatsAsync = async (groupChatId) => {
        const response = await fetch(`/api/v1/GroupChat/${groupChatId}`);
        const status = response.status;

        if (status === 200) {
            let chat = await response.json();
            return chat;
        }

        return null;
    }

    const fillMyGroupChatList = (chats) => {
        if (chats.length === 0) {
            setMyGroupChatsRender(<div className="chats-empty">You don't hav any group chats</div>);
            return;
        }

        const list = chats.map((element) => myGroupChats(element));
        setMyGroupChatsRender(list);
    }

    const myGroupChats = (element) => {
        return (<li key={element.id} className={selectedGroupChat !== null && selectedGroupChat.id === element.id ? `active` : ``}
            onClick={() => setSelectedGroupChat(element)}>
            <div><strong>{element.name}</strong></div>
            <div className="last-message" title={element.lastMessage}>{element.lastMessage}</div>
        </li>);
    }

    const getMyPersonalChatsAsync = async () => {
        const response = await fetch(`/api/v1/PersonalChat/${customer.id}`);
        const status = response.status;

        if (status === 200) {
            const chats = await response.json();

            setPersonalChats(chats);
        }
    }

    const fillMyPersonalChatList = (chats) => {
        if (chats.length === 0) {
            setPersoanlChatsRender(<div className="chats-empty">You don't have any chats</div>);
            return;
        }

        const list = chats.map((element) => myPersonalChats(element));
        setPersoanlChatsRender(list);
    }

    const myPersonalChats = (element) => {
        return (<li className={selectedPersonalChat !== null && selectedPersonalChat.id === element.id ? "active" : ""}
            key={element.id} onClick={() => setSelectedPersonalChat(element)}>
            <div><strong>{element.companionUsername}</strong></div>
            <div className="last-message" title={element.lastMessage}>{element.lastMessage}</div>
        </li>);
    }

    const createNewGroupChatAsync = async (event) => {
        event.preventDefault();

        const groupChat = {
            id: 0,
            name: nameInput.current.value,
            shortName: shortNameInput.current.value,
            lastMessage: " ",
            memberNumber: +memberNumberSelect.current.value,
            chatPolicyType: chatPolicyType,
            ownerId: customer.id
        };

        const response = await fetch("/api/v1/GroupChat", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(groupChat)
        });

        if (response.status === 200) {
            const createdGroupChat = await response.json();
            let groupChatUser = {
                id: 0,
                userId: customer.id,
                groupChatId: createdGroupChat.id,
            };

            await fetch("/api/v1/GroupChatUser", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(groupChatUser)
            });
        }

        setCreateGroupChatIsActive(false);
    }

    const render = () => {
        let chat = <div className="select-chat">Select chat</div>;
        if (!chatIsLeft && selectedPersonalChat !== null) {
            chat = <PersonalChat chat={selectedPersonalChat} setChatIsLeaft={setChatIsLeft} />;
        }
        else if (!chatIsLeft && selectedGroupChat !== null) {
            chat = <GroupChat chat={selectedGroupChat} setChatIsLeaft={setChatIsLeft} />;
        }

        return (<div>
            <div className="chats">
                <div className="chats__my-chats">
                    <div className="chats__my-chats_title">
                        <div>Group chats</div>
                        {groupChatsHidden
                            ? <FontAwesomeIcon icon={faArrowDown} title="Show chats" onClick={() => setGroupChatsHidden(!groupChatsHidden)} />
                            : <FontAwesomeIcon icon={faArrowUp} title="Hide chats" onClick={() => setGroupChatsHidden(!groupChatsHidden)} />
                        }
                        <FontAwesomeIcon className="create" icon={faSquarePlus} title="Create a new group chat" onClick={() => setCreateGroupChatIsActive(!createGroupChatIsActive)} />
                    </div>
                    <ul className="chats__my-chats__group-chats">
                        {groupChatsHidden ? null : myGroupChatsRender}
                    </ul>
                    <div className="chats__my-chats_title">
                        <div>Personal chats</div>
                        {personalChatsHidden
                            ? <FontAwesomeIcon icon={faArrowDown} title="Show chats" onClick={() => setPersonalChatsHidden(!personalChatsHidden)} />
                            : <FontAwesomeIcon icon={faArrowUp} title="Hide chats" onClick={() => setPersonalChatsHidden(!personalChatsHidden)} />
                        }
                    </div>
                    <ul className="chats__my-chats__personal-chats">
                        {personalChatsHidden ? null : personalChatsRender}
                    </ul>
                </div>
                {chat}
            </div>
            <div className={`create-group-chat${createGroupChatIsActive ? "_active" : ""}`}>
                <p className="create-group-chat__title">Create a new group chat</p>
                <form onSubmit={createNewGroupChatAsync}>
                    <div className="form-group">
                        <label htmlFor="group-chat-name">Name</label>
                        <input type="text" className="form-control" name="name" id="group-chat-name" ref={nameInput} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="short-group-chat-name">Short name</label>
                        <input type="text" className="form-control" name="shortName" id="short-group-chat-name" ref={shortNameInput} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="exampleFormControlSelect1">Member number</label>
                        <select className="form-control" name="memberNumber" ref={memberNumberSelect} id="exampleFormControlSelect1">
                            <option>100</option>
                            <option>500</option>
                            <option>1000</option>
                            <option>2500</option>
                        </select>
                    </div>
                    <div className="chat-policy">
                        <p>Chat policy</p>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="chat-policy" id="public" value="0" defaultChecked onChange={() => setChatPolicyType(0)} />
                            <label className="form-check-label" htmlFor="public">Public</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="chat-policy" id="private" value="1" onChange={() => setChatPolicyType(1)} />
                            <label className="form-check-label" htmlFor="private">Private</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="chat-policy" id="privatelinks" value="2" disabled onChange={() => setChatPolicyType(2)} />
                            <label className="form-check-label" htmlFor="privatelinks">Private with link</label>
                        </div>
                    </div>
                    <div className="create-group-chat__accept">
                        <input type="submit" value="Create" className="btn btn-success" />
                        <input type="button" value="Close" className="btn btn-light" onClick={() => setCreateGroupChatIsActive(false)} />
                    </div>
                </form>
            </div>
         </div>);
    }

    return render();
}

export default Chats;