import { useEffect, useState } from 'react';
import { useRemoveGroupChatAsyncMutation } from '../../../store/api/chat/GroupChat.api';
import { useCreateGroupChatMessageAsyncMutation } from '../../../store/api/chat/GroupChatMessage.api';
import { useGetGroupChatRulesByIdQuery, useUpdateGroupChatRulesAsyncMutation } from '../../../store/api/chat/GroupChatRules.api';
import {
    useRemoveGroupChatUserAsyncMutation
} from '../../../store/api/chat/GroupChatUser.api';
import { useLazyGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { GroupChatUser } from '../../../types/GroupChatUser';
import { GroupChatMenuProps } from '../../../types/components/communication/chats/GroupChatMenuProps';
import VerificationRestriction from '../../common/VerificationRestriction';
import ChatRulesItem from '../create/ChatRulesItem';
import GroupChatMembers from './GroupChatMembers';

const rulesEnum = {
    "owner": 0,
    "anyone": 1
};

const defaultPayload = {
    invitePeople: 0,
    removePeople: 0,
    pinMessage: 1,
    announcements: 1,
};

const GroupChatMenu: React.FC<GroupChatMenuProps> = ({ me, setSelectedChat, setShowAddPeople, groupChatUsers, meInChat, chat, t }) => {
    const [peopleInspectionModeOn, setPeopleInspectionModeOn] = useState(false);
    const [rulesInspectionModeOn, setRulesInspectionModeOn] = useState(false);
    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);
    const [invitePeople, setInvitePeople] = useState(0);
    const [removePeople, setRemovePeople] = useState(0);
    const [pinMessage, setPinMessage] = useState(1);
    const [announcements, setAnnouncements] = useState(1);
    const [payload, setPayload] = useState(defaultPayload);

    const [removeGroupChatAsyncMut] = useRemoveGroupChatAsyncMutation();
    const [removeGroupChatUserAsyncMut] = useRemoveGroupChatUserAsyncMutation();
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [getUserByIdAsync] = useLazyGetUserByIdQuery();
    const [updateGroupChatRulesMutAsync] = useUpdateGroupChatRulesAsyncMutation();

    const { data: rules, isLoading } = useGetGroupChatRulesByIdQuery(chat?.id);

    useEffect(() => {
        if (!rules) {
            return;
        }

        setPayload({
            invitePeople: rules.invitePeople,
            removePeople: rules.removePeople,
            pinMessage: rules.pinMessage,
            announcements: rules.announcements,
        });
    }, [rules])

    const removeGroupChatUserAsync = async (peopleToRemove: GroupChatUser[]) => {
        for (let i = 0; i < peopleToRemove.length; i++) {
            const removed: any = await removeGroupChatUserAsyncMut(peopleToRemove[i].id);

            if (!removed.data) {
                continue;
            }

            const user = await getUserByIdAsync(peopleToRemove[i].appUserId);
            if (user.data !== undefined) {
                const systemMessage = `'${me?.username}' removed '${user.data.username}' from chat`;
                await createMessageAsync(chat?.id, systemMessage);
            }
        }

        setPeopleInspectionModeOn(false);
    }

    const createMessageAsync = async (chatId: number, message: string) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: 1,
            chatId: chatId,
            appUserId: me.id
        };

        await createGroupChatMessageAsync(newMessage);
    }

    const leaveFromChatAsync = async (groupChatUserId: string) => {
        const deletedItem: any = await removeGroupChatUserAsyncMut(groupChatUserId);
        if (deletedItem.data !== undefined) {
            setSelectedChat({ type: null, chat: null });
        }
    }

    const removeChatAsync = async () => {
        const deletedItem: any = await removeGroupChatAsyncMut(chat?.id);
        if (deletedItem.data !== undefined) {
            setSelectedChat({ type: null, chat: null });
        }
    }

    const updateGroupChatRulesAsync = async () => {
        const groupChatRules = {
            id: rules.id,
            invitePeople: invitePeople,
            removePeople: removePeople,
            pinMessage: pinMessage,
            announcements: announcements,
            chatId: rules.chatId
        };

        const updated: any = await updateGroupChatRulesMutAsync(groupChatRules);
        if (updated.data !== undefined) {
            setRulesInspectionModeOn((item) => !item);
        }
    }

    const canInvitePeople = (): boolean => {
        const canAnyone = rules?.invitePeople === rulesEnum["anyone"];
        if (canAnyone) {
            return true;
        }

        return chat?.appUserId === me?.id;
    }

    const canRemovePeople = (): boolean => {
        const canAnyone = rules?.removePeople === rulesEnum["anyone"];
        if (canAnyone) {
            return true;
        }

        return chat?.appUserId === me?.id;
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <>
            <div className="settings__content">
                <div className="main-settings">
                    <div className="btn-border-shadow" onClick={() => setPeopleInspectionModeOn((item) => !item)}>{t("Members")}</div>
                    {canInvitePeople()&&
                        <div className="btn-border-shadow" onClick={() => setShowAddPeople((item) => !item)}>{t("Invite")}</div>
                    }
                    {chat.appUserId === me.id &&
                        <div className="btn-border-shadow" onClick={() => setRulesInspectionModeOn((item) => !item)}>{t("Rules")}</div>
                    }
                    <div className="btn-border-shadow">{t("Documents")}</div>
                </div>
                <div className="danger-settings">
                    {me.id === chat.appUserId &&
                        <div className="btn-border-shadow" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("RemoveChat")}</div>
                    }
                    {me.id === chat.appUserId
                        ? <VerificationRestriction
                            contentText={t("Leave")}
                            infoText={t("YouShouldTransferRights")}
                        />
                        : <div className="btn-border-shadow" onClick={async () => await leaveFromChatAsync(meInChat?.id)}>{t("Leave")}</div>
                    }
                </div>
            </div>
            {peopleInspectionModeOn &&
                <GroupChatMembers
                    me={me}
                    groupChatUsers={groupChatUsers}
                    removeUsersAsync={removeGroupChatUserAsync}
                    setShowMembers={setPeopleInspectionModeOn}
                    isPopup={true}
                    canRemovePeople={canRemovePeople}
                />
            }
            {rulesInspectionModeOn &&
                <div className="rules-container box-shadow">
                    <ChatRulesItem
                        setInvitePeople={setInvitePeople}
                        setRemovePeople={setRemovePeople}
                        setPinMessage={setPinMessage}
                        setAnnouncements={setAnnouncements}
                        payload={payload}
                        t={t}
                    />
                    <div className="item-result">
                        <div className="btn-border-shadow save" onClick={updateGroupChatRulesAsync}>{t("SaveChanges")}</div>
                        <div className="btn-border-shadow" onClick={() => setRulesInspectionModeOn((item) => !item)}>{t("Cancel")}</div>
                    </div>
                </div>
            }
            {showRemoveChatAlert &&
                <div className="remove-chat-alert box-shadow">
                    <p>{t("AreYouSureRemoveChat")}</p>
                    <p>{t("ThatWillBeRemoveChat")}</p>
                    <div className="remove-chat-alert__actions">
                        <div className="btn-border-shadow remove" onClick={removeChatAsync}>{t("Remove")}</div>
                        <div className="btn-border-shadow cancel" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("Cancel")}</div>
                    </div>
                </div>
            }
        </>
    );
}

export default GroupChatMenu;