import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useChatHub } from '../../../context/ChatHubProvider';
import ChatRulesItem from "./ChatRulesItem";
import CreateGroupChatMenu from './CreateGroupChatMenu';
import ItemConnector from './ItemConnector';

import "../../../styles/communication/create.scss";

const payload = {
    invitePeople: 0,
    removePeople: 0,
    pinMessage: 1,
    announcements: 1,
};

const CreateGroupChat = ({ setShowCreateGroupChat }) => {
    const { t } = useTranslation("communication/create");

    const { groupChatHubConnection } = useChatHub();

    const me = useSelector((state) => state.user.value);

    const [itemIndex, seItemIndex] = useState(0);
    const [passedItemIndex, setPassedItemIndex] = useState(0);
    const [chatName, setChatName] = useState("");
    const [invitePeople, setInvitePeople] = useState(0);
    const [removePeople, setRemovePeople] = useState(0);
    const [pinMessage, setPinMessage] = useState(1);
    const [announcements, setAnnouncements] = useState(1);
    const [canFinishCreate, setCanFinishCreate] = useState(false);
    const [isCreating, setIsCreating] = useState(false);

    const menuItemsCount = 2;

    const createGroupChatAsync = async () => {
        const groupChat = {
            id: 0,
            name: chatName,
            appUserId: me?.id
        };

        const groupChatUser = {
            id: " ",
            username: me?.username,
            appUserId: me?.id
        };

        const groupChatRules = {
            id: 0,
            invitePeople: invitePeople,
            removePeople: removePeople,
            pinMessage: pinMessage,
            announcements: announcements,
        };

        const container = {
            groupChat,
            groupChatUser,
            groupChatRules
        };

        await groupChatHubConnection?.invoke("CreateGroupChat", container);
    }

    const handleCreateNewGroupChatAsync = async () => {
        setIsCreating(true);

        await createGroupChatAsync();

        setIsCreating(false);
        setShowCreateGroupChat(false);
    }

    const nextStep = (index) => {
        seItemIndex(index);
        passedItemIndex < index && setPassedItemIndex((index) => index + 1);

        if (index === menuItemsCount) {
            setCanFinishCreate(true);

            return;
        }
    }

    const previouslyStep = () => {
        seItemIndex((index) => index - 1);
    }

    return (
        <div className="communication-content create-community box-shadow">
            <div>{t("CreateGroupChat")}</div>
            <CreateGroupChatMenu
                passedItemIndex={passedItemIndex}
                seItemIndex={seItemIndex}
                itemIndex={itemIndex}
            />
            <div className="create-community__content">
                <div className="create-community__items">
                    {itemIndex === 0 &&
                        <div className="create-community__item">
                            <div className="title">{t("Description")}</div>
                            <div>
                                <div className="form-group">
                                    <label htmlFor="name">{t("Name")}</label>
                                    <input type="text" className="form-control" name="name" id="name"
                                        onChange={(e) => setChatName(e.target.value)} defaultValue={chatName} required />
                                </div>
                                <ItemConnector
                                    connectorType={1}
                                    nextStep={nextStep}
                                    nextStepIndex={1}
                                />
                            </div>
                        </div>
                    }
                    {itemIndex >= 1 &&
                        <ChatRulesItem
                            setInvitePeople={setInvitePeople}
                            setRemovePeople={setRemovePeople}
                            setPinMessage={setPinMessage}
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
                </div>
            </div>
            {(chatName.length === 0 && passedItemIndex > 0) &&
                <div className="chat-name-required">{t("NameRequired")}</div>
            }
            <div className="actions">
                {(canFinishCreate && chatName.length > 0) &&
                    <div className="btn-shadow create" onClick={handleCreateNewGroupChatAsync}>{t("Create")}</div>
                }
                <div className="btn-shadow" onClick={() => setShowCreateGroupChat(false)}>{t("Cancel")}</div>
            </div>
            {isCreating &&
                <>
                    <span className="creating"></span>
                    <div className="notify">{t("Creating")}</div>
                </>
            }
        </div>
    );
}

export default CreateGroupChat;