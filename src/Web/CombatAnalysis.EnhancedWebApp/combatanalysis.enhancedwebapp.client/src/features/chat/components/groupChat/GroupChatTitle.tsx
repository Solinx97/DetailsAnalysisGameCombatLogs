import type { RootState } from '@/app/Store';
import logger from '@/utils/Logger';
import { faCloudArrowUp, faGear, faPen, faPhone } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { usePartialUpdateGroupChatMutation } from '../../api/GroupChat.api';
import type { GroupChatModel } from '../../types/GroupChatModel';

interface GroupChatTitleProps {
    chat: GroupChatModel;
    settingsIsShow: boolean;
    setSettingsIsShow: (value: SetStateAction<boolean>) => void;
    t: (key: string) => string;
}

const GroupChatTitle: React.FC<GroupChatTitleProps> = ({ chat, settingsIsShow, setSettingsIsShow, t }) => {
    const myself = useSelector((state: RootState) => state.user.value);

    const navigate = useNavigate();

    const [editNameOn, setEditNameOn] = useState(false);
    const [chatName, setChatName] = useState("");

    const chatNameInput = useRef<HTMLInputElement | null>(null);

    const [partialUpdateGroupChat] = usePartialUpdateGroupChatMutation();

    useEffect(() => {
        if (chat) {
            setChatName(chat.name);
        }
    }, [chat]);


    const updateGroupChatNameAsync = async () => {
        try {
            if (!chatNameInput.current) {
                return;
            }

            const updatedChat = {
                id: chat.id,
                name: chatNameInput.current.value,
            }

            await partialUpdateGroupChat({ id: updatedChat.id, groupChat: updatedChat }).unwrap();
            setChatName(chatNameInput.current.value);

            setEditNameOn(false);
        } catch (e) {
            logger.error("Failed to update group chat name", e);
        }
    }

    const call = () => {
        navigate(`/chats/voice/${chat.id}/${chat.name}`);
    }

    return (
        <>
            <div className="title">
                <div className="title__content">
                    {chat?.ownerId === myself?.id &&
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
        </>
    );
}

export default GroupChatTitle;