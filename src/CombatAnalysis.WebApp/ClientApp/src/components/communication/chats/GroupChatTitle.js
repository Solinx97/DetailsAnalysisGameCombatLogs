import { faCloudArrowUp, faGear, faPen, faPhone } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';

const GroupChatTitle = ({ chat, me, settingsIsShow, setSettingsIsShow, t }) => {
    const navigate = useNavigate();

    const [editNameOn, setEditNameOn] = useState(false);

    const chatNameInput = useRef(null);

    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();

    const updateGroupChatNameAsync = async () => {
        setEditNameOn(false);

        const unblockedObject = Object.assign({}, chat);
        unblockedObject.name = chatNameInput.current.value;

        await updateGroupChatAsyncMut(unblockedObject);
    }

    const call = () => {
        document.cookie = "callAlreadyStarted=true";

        navigate(`/chats/voice/${chat.id}/${chat.name}`);
    }

    return (
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
                        <input className="form-control" type="text" defaultValue={chat.name} ref={chatNameInput} />
                        <FontAwesomeIcon
                            icon={faCloudArrowUp}
                            title={t("Save")}
                            className={`settings-handler${settingsIsShow ? "_active" : ""}`}
                            onClick={async () => await updateGroupChatNameAsync()}
                        />
                    </>
                    : <div className="name" title={chat.name}>{chat.name}</div>
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
    );
}

export default GroupChatTitle;