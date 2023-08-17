import { faCircleCheck, faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCreateGroupChatAsyncMutation } from '../../../../store/api/GroupChat.api';
import { useCreateGroupChatUserAsyncMutation } from '../../../../store/api/GroupChatUser.api';
import Communication from '../../Communication';
import DescriptionItem from "./DescriptionItem";
import InvitePeopleItem from "./InvitePeopleItem";
import RulesItem from "./RulesItem";

import "../../../../styles/communication/chats/createGroupChat.scss";

const CreateGroupChat = () => {
    const customer = useSelector((state) => state.customer.value);

    const { t } = useTranslation("communication/chats/createGroupChat");

    const navigate = useNavigate();

    const [itemIndex, seItemIndex] = useState(0);
    const [passedItemIndex, setPassedItemIndex] = useState(0);

    const [chatName, setChatName] = useState("");
    const [chatShortName, setChatShortName] = useState("");

    const [peopleIdToJoin, setPeopleIdToJoin] = useState([]);

    const [createGroupChatAsync] = useCreateGroupChatAsyncMutation();
    const [createGroupChatUserAsync] = useCreateGroupChatUserAsyncMutation();

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

    const nextStep = (index) => {
        seItemIndex(index);
        setPassedItemIndex((index) => index + 1);
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
                        {itemIndex === 2 &&
                            <FontAwesomeIcon
                                icon={faCircleQuestion}
                            />
                        }
                        <div>{t("InvitePeople")}</div>
                    </li>
                    <li>
                        <input type="submit" value={t("Cancel")} className="btn btn-warning" onClick={() => navigate("/chats")} />
                    </li>
                </ul>
                <div className="create-group-chat__content">
                    {itemIndex === 0 &&
                        <DescriptionItem
                            chatName={chatName}
                            setChatName={setChatName}
                            chatShortName={chatShortName}
                            setChatShortName={setChatShortName}
                            nextStep={nextStep}
                        />
                    }
                    {itemIndex === 1 &&
                        <RulesItem
                            nextStep={nextStep}
                            previouslyStep={previouslyStep}
                        />
                    }
                    {itemIndex === 2 &&
                        <InvitePeopleItem
                            customer={customer}
                            createNewGroupChatAsync={createNewGroupChatAsync}
                            previouslyStep={previouslyStep}
                            peopleIdToJoin={peopleIdToJoin}
                            setPeopleToJoin={setPeopleIdToJoin}
                        />
                    }
                </div>
            </div>
        </>
    );
}

export default CreateGroupChat;