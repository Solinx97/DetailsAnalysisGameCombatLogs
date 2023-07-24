import { faCloudArrowUp, faFileWaveform, faFolderOpen, faGear, faPaperPlane, faPen, faPerson, faRightFromBracket, faRightToBracket, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useRef, useState } from 'react';
import { useFindGroupChatMessageByChatIdQuery, useGetGroupChatUserByUserIdQuery } from '../../../store/api/ChatApi';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/GroupChat.api';
import {
    useCreateGroupChatMessageAsyncMutation, useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/GroupChatMessage.api';
import { useGetGroupChatUserByChatIdQuery, useRemoveGroupChatUserAsyncMutation } from '../../../store/api/GroupChatUser.api';

import "../../../styles/communication/groupChat.scss";

const GroupChat = ({ chat, customer, setChatIsLeaft }) => {
    const [selectedMessageId, setSelectedMessageId] = useState(0);
    const [editModeIsOn, setEditMode] = useState(false);
    const [deleteModeIsOn, setDeleteMode] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionMode] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);

    const messageInput = useRef(null);
    const editMessageInput = useRef(null);

    const { data: messages, isLoading } = useFindGroupChatMessageByChatIdQuery(chat.id);
    const { data: muGroupChatUsers } = useGetGroupChatUserByUserIdQuery(chat.id);
    const { data: groupChatUsers, isLoading: usersIsLoading } = useGetGroupChatUserByChatIdQuery(chat.id);
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [removeGroupChatUserAsync] = useRemoveGroupChatUserAsyncMutation();

    const getChatUserByMyIdAsync = async () => {
        const currentGroupChatUser = muGroupChatUsers?.filter((chatUser) => chatUser.groupChatId === chat.id)[0];
        await leaveFromChatAsync(currentGroupChatUser.id);
    }

    const sendMessageAsync = async () => {
        if (messageInput.current.value.length === 0) {
            return;
        }

        await createChatMessageAsync(messageInput.current.value);
        messageInput.current.value = "";
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const newMessage = {
            userName: customer.username,
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            groupChatId: chat.id
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined) {
            await updateGroupChatAsync(message);
        }
    }

    const updateGroupChatAsync = async (message) => {
        chat.lastMessage = message;

        await updateGroupChatAsyncMut(chat);
    }

    const updateMessageAsync = async (myMessage) => {
        setEditMode(false);
        myMessage.message = editMessageInput.current.value;

        await updateGroupChatMessageAsync(myMessage);
    }

    const deleteMessageAsync = async (messageId) => {
        setDeleteMode(false);
        await removeGroupChatMessageAsync(messageId);
    }

    const leaveFromChatAsync = async (id) => {
        const deletedItem = await removeGroupChatUserAsync(id);
        if (deletedItem.data !== undefined) {
            setChatIsLeaft(true);
        }
    }

    const peopleInspectionHandler = async () => {
        setPeopleInspectionMode((item) => !item);
    }

    if (isLoading || usersIsLoading) {
        return <></>;
    }

    return (
        <div className="chats__messages">
            <div className="title">
                <div className="title__container">
                    <div className="title__companion">{chat.name}</div>
                    <FontAwesomeIcon
                        icon={faGear} title="Settings"
                        className={`settings-handler${settingsIsShow ? "-active" : ""}`}
                        onClick={() => setSettingsIsShow(!settingsIsShow)}
                    />
                </div>
                <div className={`title__message-panel${selectedMessageId > 0 ? "-active" : ""}`}>
                    <FontAwesomeIcon
                        icon={faPen}
                        title="Edit"
                        className={`edit-message-handler${editModeIsOn ? "-active" : ""}`}
                        onClick={() => setEditMode(!editModeIsOn)}
                    />
                    <FontAwesomeIcon
                        icon={faTrash}
                        title="Delete"
                        className={`delete-message-handler${deleteModeIsOn ? "-active" : ""}`}
                        onClick={() => setDeleteMode(!deleteModeIsOn)}
                    />
                </div>
            </div>
            <div className={`settings${settingsIsShow ? "-active" : ""}`}>
                <div>
                    <FontAwesomeIcon
                        icon={faPerson}
                        title="People inspection"
                        className={`people-inspection-handler${peopleInspectionModeOn ? "-active" : ""}`}
                        onClick={peopleInspectionHandler}
                    />
                    <FontAwesomeIcon
                        icon={faRightToBracket}
                        title="Invite a new person"
                        onClick={() => setEditMode(!editModeIsOn)}
                    />
                    <FontAwesomeIcon
                        icon={faFileWaveform}
                        title="Show description"
                        onClick={() => setEditMode(!editModeIsOn)}
                    />
                    <FontAwesomeIcon
                        icon={faFolderOpen}
                        title="Show documents"
                        onClick={() => setEditMode(!editModeIsOn)}
                    />
                    <FontAwesomeIcon
                        icon={faRightFromBracket}
                        title="Leave from chat"
                        className="leave-from-chat"
                        onClick={getChatUserByMyIdAsync}
                    />
                </div>
                <ul className={`people-inspection${peopleInspectionModeOn ? "-active" : ""}`}>
                    {
                        groupChatUsers.map((item) => (
                            <li key={item.id}>{item.email}</li>
                        ))
                    }
                </ul>
            </div>
            <ul className="group-chat-messages">
                {
                    messages.map((item) => (
                        <li key={item.id} className={`group-chat-messages__${customer.username === item.username ? "right" : "left"}`} onClick={() => setSelectedMessageId(item.id)}>
                            {customer?.username !== item.username &&
                                <div className="username">{item.username}</div>
                            }
                            {editModeIsOn && customer.username === item.username && item.id === selectedMessageId
                                ? <div className="edit-message">
                                    <input className="form-control" defaultValue={item.message} ref={editMessageInput} />
                                    <FontAwesomeIcon
                                        icon={faCloudArrowUp}
                                        title="Save"
                                        onClick={async () => await updateMessageAsync(item)}
                                    />
                                </div>
                                : <div className="message">{item.message}</div>
                            }
                            {deleteModeIsOn && customer.username === item.username && item.id === selectedMessageId &&
                                <FontAwesomeIcon
                                    icon={faTrash}
                                    title="Save"
                                    onClick={async () => await deleteMessageAsync(item.id)}
                                />
                            }
                        </li>
                    ))
                }
            </ul>
            <div className="form-group chats__messages_input-message">
                <input type="text" className="form-control" placeholder="Type your message" ref={messageInput} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title="Send message"
                    onClick={async () => await sendMessageAsync()}
                />
            </div>
        </div>
    );
}

export default memo(GroupChat);