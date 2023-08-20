import { faCircleCheck, faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCreateGroupChatAsyncMutation } from '../../../../store/api/GroupChat.api';
import { useCreateGroupChatUserAsyncMutation } from '../../../../store/api/GroupChatUser.api';
import Communication from '../../Communication';
import CommonItem from "./CommonItem";
import ItemConnector from './ItemConnector';
import RulesItem from "./RulesItem";

import "../../../../styles/communication/chats/createGroupChat.scss";
import AddPeople from '../../../AddPeople';

const CreateGroupChat = () => {
    const customer = useSelector((state) => state.customer.value);

    const { t } = useTranslation("communication/chats/createGroupChat");

    const navigate = useNavigate();

    const [itemIndex, seItemIndex] = useState(0);
    const [passedItemIndex, setPassedItemIndex] = useState(0);

    const [chatName, setChatName] = useState("");
    const [chatShortName, setChatShortName] = useState("");
    const [canFinishCreate, setCanFinishCreate] = useState(false);

    const [peopleIdToJoin, setPeopleIdToJoin] = useState([]);

    const [createGroupChatAsync] = useCreateGroupChatAsyncMutation();
    const [createGroupChatUserAsync] = useCreateGroupChatUserAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();

    const createNewGroupChatAsync = async () => {
        const newGroupChat = {
            name: chatName,
            shortName: chatShortName,
            lastMessage: " ",
            ownerId: customer?.id
        };

        const createdGroupChat = await createGroupChatAsync(newGroupChat);
        if (createdGroupChat.error !== undefined) {
            return null;
        }

        const newGroupChatUser = {
            userId: customer?.id,
            groupChatId: createdGroupChat.data.id,
        };

        const createdGroupChatUser = await createGroupChatUserAsync(newGroupChatUser);
        if (createdGroupChatUser.error !== undefined) {
            return null;
        }

        return createdGroupChat.data;
    }

    const joinPeopleAsync = async (groupChatId) => {
        for (var i = 0; i < peopleIdToJoin.length; i++) {
            const newGroupChatUser = {
                userId: peopleIdToJoin[i],
                groupChatId: groupChatId,
            };

            await createGroupChatUserMutAsync(newGroupChatUser);
        }
    }

    const handleCreateNewGroupChatAsync = async () => {
        const createdGroupChat = await createNewGroupChatAsync();
        await joinPeopleAsync(createdGroupChat.id);

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
            <Communication
                currentMenuItem={2}
            />
            <div className="communication__content create-group-chat">
                <div className="create-group-chat__content">
                    <ul className="create-group-chat__menu">
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
                    <div className="create-group-chat__items">
                        {itemIndex === 0 &&
                            <CommonItem
                                chatName={chatName}
                                setChatName={setChatName}
                                chatShortName={chatShortName}
                                setChatShortName={setChatShortName}
                                connector={
                                    <ItemConnector
                                        connectorType={1}
                                        nextStep={nextStep}
                                        nextStepIndex={1}
                                    />
                                }
                            />
                        }
                        {itemIndex === 1 &&
                            <RulesItem
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
                            <div className="create-group-chat__item">
                                <AddPeople
                                    customer={customer}
                                    communityUsersId={[customer.id]}
                                    peopleToJoin={peopleIdToJoin}
                                    setPeopleToJoin={setPeopleIdToJoin}
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