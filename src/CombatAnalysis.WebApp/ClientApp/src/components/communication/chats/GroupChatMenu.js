import { faUserXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useRemoveGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import { useCreateGroupChatMessageAsyncMutation } from '../../../store/api/communication/chats/GroupChatMessage.api';
import {
    useRemoveGroupChatUserAsyncMutation
} from '../../../store/api/communication/chats/GroupChatUser.api';
import User from '../User';

const GroupChatMenu = ({ me, setUserInformation, setSelectedChat, setShowAddPeople, groupChatUsers, meInChat, chat }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [peopleInspectionModeOn, setPeopleInspectionMode] = useState(false);
    const [peopleToRemove, setPeopleToRemove] = useState([]);
    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);
    const [showRemoveUser, setShowRemoveUser] = useState(false);

    const [removeGroupChatAsyncMut] = useRemoveGroupChatAsyncMutation();
    const [removeGroupChatUserAsyncMut] = useRemoveGroupChatUserAsyncMutation();
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [getCustomerByIdAsync] = useLazyGetCustomerByIdQuery();

    const addPeopleForRemove = (user) => {
        const people = peopleToRemove;
        people.push(user);

        setPeopleToRemove(people);
    }

    const removePeopleFromForRemove = (user) => {
        const people = peopleToRemove.filter((item) => item.id !== user.id);

        setPeopleToRemove(people);
    }

    const removeGroupChatUserAsync = async () => {
        for (let i = 0; i < peopleToRemove.length; i++) {
            const removed = await removeGroupChatUserAsyncMut(peopleToRemove[i].id);

            if (removed.data !== undefined) {
                const customer = await getCustomerByIdAsync(peopleToRemove[i].customerId);

                if (customer.data !== undefined) {
                    const systemMessage = `'${me?.username}' removed '${customer.data.username}' from chat`;
                    await createMessageAsync(chat?.id, systemMessage);
                }
            }
        }

        setPeopleInspectionMode(false);
        setShowRemoveUser(false);
    }

    const createMessageAsync = async (groupChatId, message) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: 1,
            groupChatId: groupChatId,
            customerId: me?.id
        };

        await createGroupChatMessageAsync(newMessage);
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

    const handleRemoveUser = (e, user) => {
        const checked = e.target.checked;

        checked ? addPeopleForRemove(user) : removePeopleFromForRemove(user);
    }

    const hidePeopleInspectionMode = () => {
        setPeopleToRemove([]);

        setPeopleInspectionMode(false);
        setShowRemoveUser(false);
    }

    const handleRemoveUsers = () => {
        setShowRemoveUser((item) => !item);

        setPeopleToRemove([]);
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
                <div className="title">
                    <div>{t("Members")}</div>
                    <FontAwesomeIcon
                        icon={faUserXmark}
                        className={`remove${showRemoveUser ? "_active" : ""}`}
                        title={t("Remove")}
                        onClick={handleRemoveUsers}
                    />
                </div>
                <ul className="list">
                    {groupChatUsers.map((item) => (
                            <li className="group-chat-user" key={item.id}>
                                <User
                                    me={me}
                                    targetCustomerId={item.customerId}
                                    setUserInformation={setUserInformation}
                                    allowRemoveFriend={false}
                                />
                                {(me?.id === chat.customerId && item.customerId !== chat.customerId
                                    && showRemoveUser) &&
                                    <input className="form-check-input" type="checkbox" onChange={(e) => handleRemoveUser(e, item)} />
                                }
                            </li>
                        ))
                    }
                </ul>
                <div className="item-result">
                    <input type="button" value={t("Accept")} className="btn btn-success" onClick={async () => await removeGroupChatUserAsync()} />
                    <input type="button" value={t("Close")} className="btn btn-secondary" onClick={hidePeopleInspectionMode} />
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