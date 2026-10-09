import type { RootState } from '@/app/Store';
import VerificationRestriction from '@/shared/components/VerificationRestriction';
import { GroupChatRulesType } from '@/shared/helpers/EnumHelper';
import { useChatHub } from '@/shared/hooks/useChatHub';
import logger from '@/utils/Logger';
import { useEffect, useState, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useRemoveGroupChatMutation } from '../../api/GroupChat.api';
import { useGetGroupChatRulesByChatIdQuery, useUpdateGroupChatRulesMutation } from '../../api/GroupChatRules.api';
import {
    useRemoveGroupChatUserAsyncMutation
} from '../../api/GroupChatUser.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatRulesItem from '../create/ChatRulesItem';
import GroupChatAddUser from './GroupChatAddUser';
import GroupChatMembers from './GroupChatMembers';

interface GroupChatRules {
    invitePeople: number,
    removePeople: number,
    pinMessage: number,
    announcements: number,
};

interface GroupChatMenuProps {
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    groupChatUsersId: string[];
    chat: GroupChatModel;
    t: (key: string) => string;
}

const GroupChatMenu: React.FC<GroupChatMenuProps> = ({ setSelectedChat, groupChatUsersId, chat, t }) => {
    const myself = useSelector((state: RootState) => state.user.value);
    const groupChatUser = useSelector((state: RootState) => state.groupChatUser.value);

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionModeOn] = useState(false);
    const [rulesInspectionModeOn, setRulesInspectionModeOn] = useState(false);
    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);
    const [invitePeople, setInvitePeople] = useState(0);
    const [removePeople, setRemovePeople] = useState(0);
    const [pinMessage, setPinMessage] = useState(0);
    const [announcements, setAnnouncements] = useState(0);
    const [payload, setPayload] = useState<GroupChatRules>({
        invitePeople: GroupChatRulesType["ANYONE"],
        removePeople: GroupChatRulesType["ANYONE"],
        pinMessage: GroupChatRulesType["ANYONE"],
        announcements: GroupChatRulesType["ANYONE"],
    });

    const chatHub = useChatHub();

    const [removeGroupChat] = useRemoveGroupChatMutation();
    const [removeGroupChatUser] = useRemoveGroupChatUserAsyncMutation();
    const [updateGroupChatRules] = useUpdateGroupChatRulesMutation();

    const { data: rules, isLoading } = useGetGroupChatRulesByChatIdQuery(chat.id);

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

    const removeGroupChatUsersAsync = async (peopleToRemove: GroupChatUserModel[]) => {
        try {
            if (!chatHub || !chatHub.groupChatHubConnectionRef.current) {
                return;
            }

            for (let i = 0; i < peopleToRemove.length; i++) {
                await chatHub.groupChatHubConnectionRef.current.invoke("RemoveUserFromChat", chat.ownerId, chat.id, peopleToRemove[i].id, peopleToRemove[i].username);
            }

            setPeopleInspectionModeOn(false);
        } catch (e) {
            logger.error("Failed to remove group chat users", e);
        }
    }

    const leaveFromChatAsync = async (groupChatUserId: string) => {
        try {
            setSelectedChat(null);
            await removeGroupChatUser(groupChatUserId).unwrap();
        } catch (e) {
            logger.error("Failed to leave from group chat", e);
        }
    }

    const removeChatAsync = async () => {
        try {
            await removeGroupChat(chat.id).unwrap();
            setSelectedChat(null);
        } catch (e) {
            logger.error("Failed to remove group chat", e);
        }
    }

    const updateGroupChatRulesAsync = async () => {
        if (!rules) {
            return;
        }

        try {
            const groupChatRules = {
                id: rules.id,
                invitePeople: invitePeople,
                removePeople: removePeople,
                pinMessage: pinMessage,
                announcements: announcements,
                groupChatId: chat.id
            };

            await updateGroupChatRules({ chatId: chat.id, groupChatRules }).unwrap();
            setRulesInspectionModeOn((item) => !item);
        } catch (e) {
            logger.error("Failed to update group chat rules", e);
        }
    }

    const canInvitePeople = (): boolean => {
        const canAnyone = rules?.invitePeople === GroupChatRulesType["ANYONE"];
        if (canAnyone) {
            return true;
        }

        return chat?.ownerId === myself?.id;
    }

    const canRemovePeople = (): boolean => {
        const canAnyone = rules?.removePeople === GroupChatRulesType["ANYONE"];
        if (canAnyone) {
            return true;
        }

        return chat?.ownerId === myself?.id;
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <>
            <div className="settings__content">
                <div className="main-settings">
                    <div className="btn-border-shadow" onClick={() => setPeopleInspectionModeOn((item) => !item)}>{t("Members")}</div>
                    {canInvitePeople() &&
                        <div className="btn-border-shadow" onClick={() => setShowAddPeople((item) => !item)}>{t("Invite")}</div>
                    }
                    {chat.ownerId === myself?.id &&
                        <div className="btn-border-shadow" onClick={() => setRulesInspectionModeOn((item) => !item)}>{t("Rules")}</div>
                    }
                    <div className="btn-border-shadow">{t("Documents")}</div>
                </div>
                <div className="danger-settings">
                    {myself?.id === chat.ownerId &&
                        <div className="btn-border-shadow" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("RemoveChat")}</div>
                    }
                    {myself?.id === chat.ownerId
                        ? <VerificationRestriction
                            contentText={t("Leave")}
                            infoText={t("YouShouldTransferRights")}
                        />
                        : <div className="btn-border-shadow" onClick={async () => await leaveFromChatAsync(groupChatUser?.id ?? "")}>{t("Leave")}</div>
                    }
                </div>
            </div>
            {peopleInspectionModeOn &&
                <GroupChatMembers
                    chatId={chat.id}
                    removeUsersAsync={removeGroupChatUsersAsync}
                    setShowMembers={setPeopleInspectionModeOn}
                    isPopup={true}
                    canRemovePeople={canRemovePeople}
                />
            }
            {showAddPeople &&
                <GroupChatAddUser
                    chat={chat}
                    groupChatUsersId={groupChatUsersId}
                    setShowAddPeople={setShowAddPeople}
                    t={t}
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