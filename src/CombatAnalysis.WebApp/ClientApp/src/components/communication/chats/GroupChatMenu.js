import { faMinus, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useRemoveGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import {
    useRemoveGroupChatUserAsyncMutation
} from '../../../store/api/communication/chats/GroupChatUser.api';
import User from '../User';

const GroupChatMenu = ({ me, setUserInformation, setSelectedChat, setShowAddPeople, groupChatUsers, meInChat, chat }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [peopleInspectionModeOn, setPeopleInspectionMode] = useState(false);
    const [peopleIdToRemove, setPeopleToRemove] = useState([]);
    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState();

    const [removeGroupChatAsyncMut] = useRemoveGroupChatAsyncMutation();
    const [removeGroupChatUserAsyncMut] = useRemoveGroupChatUserAsyncMutation();

    const addPeopleForRemove = (id) => {
        const people = peopleIdToRemove;
        people.push(id);

        setPeopleToRemove(people);
    }

    const removePeopleFromForRemove = (id) => {
        const people = peopleIdToRemove.filter((item) => item !== id);

        setPeopleToRemove(people);
    }

    const removeGroupChatUserAsync = async () => {
        for (var i = 0; i < peopleIdToRemove.length; i++) {
            await removeGroupChatUserAsyncMut(peopleIdToRemove[i]);
        }

        setPeopleInspectionMode(false);
    }

    const leaveFromChatAsync = async (id) => {
        const deletedItem = await removeGroupChatUserAsyncMut(id);
        if (deletedItem.data !== undefined) {
            setSelectedChat(null);
        }
    }

    const removeChatAsync = async () => {
        const deletedItem = await removeGroupChatAsyncMut(chat.id);
        if (deletedItem.data !== undefined) {
            setSelectedChat(null);
        }
    }

    return (
        <>
            <div className="settings__content">
                <div className="main-settings">
                    <input type="button" value={t("Members")} className="btn btn-light" onClick={() => setPeopleInspectionMode((item) => !item)} />
                    <input type="button" value={t("Invite")} className="btn btn-light" onClick={() => setShowAddPeople((item) => !item)} />
                    <input type="button" value={t("Documents")} className="btn btn-light" disabled />
                </div>
                <div className="danger-settings">
                    {me?.id === chat.customerId &&
                        <input type="button" value={t("RemoveChat")} className="btn btn-danger" onClick={() => setShowRemoveChatAlert((item) => !item)} />
                    }
                    <input type="button" value={t("Leave")} className="btn btn-warning" onClick={async () => await leaveFromChatAsync(meInChat?.id)} />
                </div>
            </div>
            <div className={`settings__people-inspection${peopleInspectionModeOn ? "_active" : ""}`}>
                <div>{t("Members")}</div>
                <ul className="list">
                    {groupChatUsers.map((item) => (
                            <li className="group-chat-user" key={item.id}>
                                <User
                                    me={me}
                                    targetCustomerId={item.customerId}
                                    setUserInformation={setUserInformation}
                                    allowRemoveFriend={false}
                                />
                                {(me?.id === chat.customerId && item.customerId !== chat.customerId)
                                    ? peopleIdToRemove.includes(item.id)
                                        ? <FontAwesomeIcon
                                            icon={faRightFromBracket}
                                            title={t("RomeFromChat")}
                                            onClick={() => removePeopleFromForRemove(item.id)}
                                        />
                                        : <FontAwesomeIcon
                                            icon={faMinus}
                                            title={t("RomeFromChat")}
                                            onClick={() => addPeopleForRemove(item.id)}
                                        />
                                    : null
                                }
                            </li>
                        ))
                    }
                </ul>
                <div className="item-result">
                    <input type="button" value={t("Accept")} className="btn btn-success" onClick={async () => await removeGroupChatUserAsync()} />
                    <input type="button" value={t("Close")} className="btn btn-secondary" onClick={() => setPeopleInspectionMode((item) => !item)} />
                </div>

            </div>
            {showRemoveChatAlert &&
                <div className="remove-chat-alert">
                    <p>{t("AreYouSureRemoveChat")}</p>
                    <p>{t("ThatWillBeRemoveChat")}</p>
                    <div className="remove-chat-alert__actions">
                        <input type="button" value={t("Remove")} className="btn btn-warning" onClick={async  () => await removeChatAsync()} />
                        <input type="button" value={t("Cancel")} className="btn btn-light" onClick={() => setShowRemoveChatAlert((item) => !item)} />
                    </div>
                </div>
            }
        </>
    );
}

export default GroupChatMenu;