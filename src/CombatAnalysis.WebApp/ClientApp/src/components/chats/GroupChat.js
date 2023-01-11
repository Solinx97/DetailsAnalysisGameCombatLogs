import { memo, useEffect, useRef, useState } from 'react';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const GroupChat = ({ chat }) => {
    const [chatMessagesRender, setChatMessagesRender] = useState(<></>);
    const messageInput = useRef(null);
    const chatMessageUpdateInterval = 200;

    useEffect(() => {
        async function getChatMessages() {
            await getChatMessagesAsync();
        };

        let interval = setInterval(() => {
            getChatMessages();
        }, chatMessageUpdateInterval);

        return () => {
            clearInterval(interval);
        };
    }, [chat])

    const getChatMessagesAsync = async () => {
        const response = await fetch(`/api/v1/GroupChatMessage/findByChatId/${chat.id}`);
        const status = response.status;
        if (status === 200) {
            const messages = await response.json();

            fillChatMessageList(messages);
        }
    }

    const fillChatMessageList = (messages) => {
        const list = messages.map((element) => groupChatMessages(element));

        setChatMessagesRender(list);
    }

    const groupChatMessages = (element) => {
        return (<li key={element.id}>
            <div>{element.message}</div>
        </li>);
    }

    const sendMessageAsync = async () => {
        await createChatMessageAsync(messageInput.current.value);
        messageInput.current.value = "";
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const data = {
            id: 0,
            userName: "temp@yandex.by",
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            groupChatId: chat.id
        };

        const response = await fetch("/api/v1/GroupChatMessage", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.status == 200) {
            await updateGroupChatAsync(message);
        }
    }

    const updateGroupChatAsync = async (message) => {
        chat.lastMessage = message;

        await fetch("/api/v1/GroupChat", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(chat)
        });
    }

    const render = () => {
        return (
            <div className="chats__messages">
                <div>{chat.name}</div>
                <ul>
                    {chatMessagesRender}
                </ul>
                <div className="form-group chats__messages_input-message">
                    <input type="text" className="form-control" placeholder="Type your message" ref={messageInput} />
                    <FontAwesomeIcon icon={faPaperPlane} title="Send message" onClick={async () => await sendMessageAsync()} />
                </div>
            </div>);
    }

    return render();
}

export default memo(GroupChat);