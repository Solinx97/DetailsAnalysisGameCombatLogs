import { faCloudArrowUp, faGear, faPen, faPhone } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/chat/GroupChat.api';
import { GroupChatTitleProps } from '../../../types/components/communication/chats/GroupChatTitleProps';

const GroupChatTitle: React.FC<GroupChatTitleProps> = ({ me, chat, settingsIsShow, setSettingsIsShow, haveMoreMessages, setHaveMoreMessage, loadMoreMessagesAsync, t }) => {
    const navigate = useNavigate();

    const [editNameOn, setEditNameOn] = useState(false);
    const [chatName, setChatName] = useState("");

    const chatNameInput = useRef<any>(null);

    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();

    useEffect(() => {
        if (chat) {
            setChatName(chat.name);
        }
    }, [chat]);


    const updateGroupChatNameAsync = async () => {
        const unblockedObject = Object.assign({}, chat);
        unblockedObject.name = chatNameInput.current.value;

        const response: any = await updateGroupChatAsyncMut(unblockedObject);
        if (!response.error) {
            setChatName(chatNameInput.current.value);

            setEditNameOn(false);
        }
    }

    const call = () => {
        navigate(`/chats/voice/${chat.id}/${chat.name}`);
    }

    const handleLoadMoreMessagesAsync = async () => {
        setHaveMoreMessage(false);

        await loadMoreMessagesAsync();
    }

    return (
        <>
            <div className="title">
                <div className="title__content">
                    {chat?.appUserId === me?.id &&
                        <FontAwesomeIcon
                            icon={faPen}
                            title={t("EditName")}
                            className={`settings-handler${editNameOn ? "_active" : ""}`}
                            onClick={() => setEditNameOn((item) => !item)}
                        />
                    }
                    {editNameOn
                        ? <>
                            <input className="form-control" type="text" defaultValue={chatName} ref={chatNameInput} />
                            <FontAwesomeIcon
                                icon={faCloudArrowUp}
                                title={t("Save")}
                                className={`settings-handler${settingsIsShow ? "_active" : ""}`}
                                onClick={updateGroupChatNameAsync}
                            />
                        </>
                        : <div className="name" title={chatName}>{chatName}</div>
                    }
                </div>
                <div className="title__menu">
                    <FontAwesomeIcon
                        icon={faPhone}
                        title={t("Call")}
                        className="call"
                        onClick={call}
                    />
                    <FontAwesomeIcon
                        icon={faGear}
                        title={t("Settings")}
                        className={`settings-handler${settingsIsShow ? "_active" : ""}`}
                        onClick={() => setSettingsIsShow(!settingsIsShow)}
                    />
                </div>
            </div>
            {haveMoreMessages &&
                <div className="load-more" onClick={handleLoadMoreMessagesAsync}>Load more...</div>
            }
        </>
    );
}

export default GroupChatTitle;