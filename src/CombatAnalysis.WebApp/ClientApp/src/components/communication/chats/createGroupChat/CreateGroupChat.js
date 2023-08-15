import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCreateGroupChatAsyncMutation } from '../../../../store/api/GroupChat.api';
import { useCreateGroupChatUserAsyncMutation } from '../../../../store/api/GroupChatUser.api';
import Communication from '../../Communication';
import DescriptionItem from "./DescriptionItem";
import InvitePeopleItem from "./InvitePeopleItem";
import PolicyItem from "./PolicyItem";
import RulesItem from "./RulesItem";

import "../../../../styles/communication/chats/createGroupChat.scss";

const CreateGroupChat = () => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    const customer = useSelector((state) => state.customer.value);

    const navigate = useNavigate();

    const [chatPolicyType, setChatPolicyType] = useState(0);

    const [itemIndex, seItemIndex] = useState(0);

    const [chatName, setChatName] = useState("");
    const [chatShortName, setChatShortName] = useState("");

    const [createGroupChatAsync] = useCreateGroupChatAsyncMutation();
    const [createGroupChatUserAsync] = useCreateGroupChatUserAsyncMutation();

    const createNewGroupChatAsync = async () => {
        const newGroupChat = {
            name: chatName,
            shortName: chatShortName,
            lastMessage: " ",
            memberNumber: 100,
            chatPolicyType: chatPolicyType,
            ownerId: customer?.id
        };

        const createdGroupChat = await createGroupChatAsync(newGroupChat);
        if (createdGroupChat.error !== undefined) {
            return;
        }

        const newGroupChatUser = {
            userId: customer?.id,
            groupChatId: createdGroupChat.data.id,
        };

        const createdGroupChatUser = await createGroupChatUserAsync(newGroupChatUser);
        if (createdGroupChatUser.data !== undefined) {
            navigate("/chats");
        }
    }

    return (
        <>
            <Communication
                currentMenuItem={0}
            />
            <div className="communication__content create-group-chat">
                <ul className="create-group-chat__menu">
                    <li className="menu-item" onClick={() => seItemIndex(0)}>
                        Description
                    </li>
                    <li className="menu-item" onClick={() => seItemIndex(1)}>
                        Policy
                    </li>
                    <li className="menu-item" onClick={() => seItemIndex(2)}>
                        Rules
                    </li>
                    <li className="menu-item" onClick={() => seItemIndex(3)}>
                        Invite people
                    </li>
                </ul>
                <div className="create-group-chat__content">
                    {itemIndex === 0 &&
                        <DescriptionItem
                            setChatName={setChatName}
                            setChatShortName={setChatShortName}
                        />
                    }
                    {itemIndex === 1 &&
                        <PolicyItem
                            setChatPolicyType={setChatPolicyType}
                        />
                    }
                    {itemIndex === 2 &&
                        <RulesItem />
                    }
                    {itemIndex === 3 &&
                        <InvitePeopleItem
                            createNewGroupChatAsync={createNewGroupChatAsync}
                        />
                    }
                </div>
            </div>
        </>
    );
}

export default CreateGroupChat;