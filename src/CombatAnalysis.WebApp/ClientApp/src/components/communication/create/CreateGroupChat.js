import { faCircleCheck, faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCreateGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import { useCreateGroupChatMessageCountAsyncMutation } from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import { useCreateGroupChatMessageAsyncMutation } from '../../../store/api/communication/chats/GroupChatMessage.api';
import { useCreateGroupChatRulesAsyncMutation } from '../../../store/api/communication/chats/GroupChatRules.api';
import { useCreateGroupChatUserAsyncMutation } from '../../../store/api/communication/chats/GroupChatUser.api';
import AddPeople from '../../AddPeople';
import CommunicationMenu from '../CommunicationMenu';
import ChatRulesItem from "./ChatRulesItem";
import ItemConnector from './ItemConnector';

import "../../../styles/communication/create.scss";

const payload = {
    invitePeople: 0,
    removePeople: 0,
    pinMessage: 1,
    announcements: 1,
};

const CreateGroupChat = () => {
    const customer = useSelector((state) => state.customer.value);

    const { t } = useTranslation("communication/create");

    const navigate = useNavigate();

    const [itemIndex, seItemIndex] = useState(0);
    const [passedItemIndex, setPassedItemIndex] = useState(0);
    const [chatName, setChatName] = useState("");
    const [invitePeople, setInvitePeople] = useState(0);
    const [removePeople, setRemovePeople] = useState(0);
    const [pinMessage, setPinMessage] = useState(1);
    const [announcements, setAnnouncements] = useState(1);
    const [canFinishCreate, setCanFinishCreate] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState([]);

    const [createGroupChatMutAsync] = useCreateGroupChatAsyncMutation();
    const [createGroupChatRulesMutAsync] = useCreateGroupChatRulesAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();
    const [createGroupChatCountAsyncMut] = useCreateGroupChatMessageCountAsyncMutation();
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();

    const createGroupChatAsync = async () => {
        const groupChat = {
            name: chatName,
            lastMessage: " ",
            customerId: customer?.id
        };

        const createdGroupChat = await createGroupChatMutAsync(groupChat);
        if (createdGroupChat.error !== undefined) {
            return null;
        }

        return createdGroupChat.data;
    }

    const createGroupChatRulesAsync = async (groupChatId) => {
        const groupChatRules = {
            invitePeople: invitePeople,
            removePeople: removePeople,
            pinMessage: pinMessage,
            announcements: announcements,
            groupChatId: groupChatId
        };

        await createGroupChatRulesMutAsync(groupChatRules);
    }

    const createGroupChatCountAsync = async (chatId, groupChatUserId) => {
        const newMessagesCount = {
            count: 0,
            groupChatUserId: groupChatUserId,
            groupChatId: +chatId,
        };

        const createdMessagesCount = await createGroupChatCountAsyncMut(newMessagesCount);
        return createdMessagesCount.data !== undefined;
    }

    const joinPeopleAsync = async (groupChatId) => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const newGroupChatUser = {
                id: " ",
                username: peopleToJoin[i].username,
                customerId: peopleToJoin[i].id,
                groupChatId: groupChatId,
            };

            const createdGroupChatUser = await createGroupChatUserMutAsync(newGroupChatUser);
            if (createdGroupChatUser.data !== undefined) {
                await createGroupChatCountAsync(groupChatId, createdGroupChatUser.data.id);

                if (peopleToJoin[i].id !== customer?.id) {
                    const systemMessage = `'${customer?.username}' added '${peopleToJoin[i].username}' to chat`;
                    await createMessageAsync(groupChatId, systemMessage);
                }
            }
        }
    }

    const createMessageAsync = async (groupChatId, message) => {
        const today = new Date();
        const newMessage = {
            message: message,
            when: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: 1,
            groupChatId: groupChatId,
            customerId: customer?.id
        };

        await createGroupChatMessageAsync(newMessage);
    }

    const handleCreateNewGroupChatAsync = async () => {
        const createdGroupChat = await createGroupChatAsync();

        await createGroupChatRulesAsync(createdGroupChat.id);

        const people = peopleToJoin;
        people.push(customer);

        setPeopleToJoin(people);

        await joinPeopleAsync(createdGroupChat?.id);

        navigate("/chats");
    }

    const nextStep = (index) => {
        seItemIndex(index);
        passedItemIndex < index && setPassedItemIndex((index) => index + 1);

        if (index === 3) {
            setCanFinishCreate(true);
            return;
        }
    }

    const previouslyStep = () => {
        seItemIndex((index) => index - 1);
    }

    const changeMenuItem = (index) => {
        if (passedItemIndex < index) {
            return;
        }

        seItemIndex(index);
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={2}
            />
            <div className="communication__content create-community box-shadow">
                <div className="create-community__content">
                    <ul className="create-community__menu">
                        <li className={`menu-item ${passedItemIndex >= 0 && "passed"}`} onClick={() => changeMenuItem(0)}>
                            {(passedItemIndex > 0 && itemIndex !== 0)
                                ? <FontAwesomeIcon
                                    className="menu-item__passed"
                                    icon={faCircleCheck}
                                />
                                : itemIndex === 0 &&
                                <FontAwesomeIcon
                                    icon={faCircleQuestion}
                                />
                            }
                            <div>{t("Description")}</div>
                        </li>
                        <li className={`menu-item ${passedItemIndex >= 1 && "passed"}`} onClick={() => changeMenuItem(1)}>
                            {(passedItemIndex > 1 && itemIndex !== 1)
                                ? <FontAwesomeIcon
                                    className="menu-item__passed"
                                    icon={faCircleCheck}
                                />
                                : itemIndex === 1 &&
                                <FontAwesomeIcon
                                    icon={faCircleQuestion}
                                />
                            }
                            <div>{t("Rules")}</div>
                        </li>
                        <li className={`menu-item ${passedItemIndex >= 2 && "passed"}`} onClick={() => changeMenuItem(2)}>
                            {(passedItemIndex > 2 && itemIndex !== 2)
                                ? <FontAwesomeIcon
                                    className="menu-item__passed"
                                    icon={faCircleCheck}
                                />
                                : itemIndex === 2 &&
                                <FontAwesomeIcon
                                    icon={faCircleQuestion}
                                />
                            }
                            <div>{t("InvitePeople")}</div>
                        </li>
                    </ul>
                    <div className="create-community__items">
                        {itemIndex === 0 &&
                            <div className="create-community__item">
                                <div className="title">{t("Description")}</div>
                                <div>
                                    <div>
                                        <div className="form-group">
                                            <label htmlFor="name">{t("Name")}</label>
                                            <input type="text" className="form-control" name="name" id="name"
                                                onChange={(e) => setChatName(e.target.value)} defaultValue={chatName} required />
                                        </div>
                                    </div>
                                    <ItemConnector
                                        connectorType={1}
                                        nextStep={nextStep}
                                        nextStepIndex={1}
                                    />
                                </div>
                            </div>
                        }
                        {itemIndex === 1 &&
                            <ChatRulesItem
                                setInvitePeople={setInvitePeople}
                                setRemovePeople={setRemovePeople}
                                setPinMessage={setPinMessage }
                                setAnnouncements={setAnnouncements}
                                payload={payload}
                                connector={
                                    <ItemConnector
                                        connectorType={3}
                                        nextStep={nextStep}
                                        previouslyStep={previouslyStep}
                                        nextStepIndex={2}
                                    />
                                }
                            />
                        }
                        {itemIndex >= 2 &&
                            <div className="create-community__item">
                                <AddPeople
                                    customer={customer}
                                    communityUsersId={[customer.id]}
                                    peopleToJoin={peopleToJoin}
                                    setPeopleToJoin={setPeopleToJoin}
                                />
                                <ItemConnector
                                    connectorType={3}
                                    nextStep={nextStep}
                                    previouslyStep={previouslyStep}
                                    nextStepIndex={3}
                                />
                            </div>
                        }
                    </div>
                </div>
                <div className="finish-create">
                    <input type="button" value={t("Create")} className="btn btn-success"
                        onClick={async () => await handleCreateNewGroupChatAsync()} disabled={!canFinishCreate} />
                    <input type="submit" value={t("Cancel")} className="btn btn-warning" onClick={() => navigate("/chats")} />
                </div>
            </div>
        </>
    );
}

export default CreateGroupChat;