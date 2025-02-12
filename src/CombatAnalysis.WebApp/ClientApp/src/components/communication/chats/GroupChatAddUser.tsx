import { useState } from 'react';
import { useChatHub } from '../../../context/ChatHubProvider';
import { AppUser } from '../../../types/AppUser';
import { GroupChatAddUserProps } from '../../../types/components/communication/chats/GroupChatAddUserProps';
import AddPeople from '../../AddPeople';

const GroupChatAddUser: React.FC<GroupChatAddUserProps> = ({ me, chatId, groupChatUsersId, setShowAddPeople, t }) => {
    const { groupChatHubConnection } = useChatHub();

    const [peopleToJoin, setPeopleToJoin] = useState<AppUser[]>([]);

    const createGroupChatUserAsync = async () => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const newGroupChatUser = {
                id: " ",
                username: peopleToJoin[i].username,
                appUserId: peopleToJoin[i].id,
                chatId: chatId,
            };

            await groupChatHubConnection.invoke("AddUserToChat", newGroupChatUser);
        }

        setPeopleToJoin([]);
        setShowAddPeople(false);
    }

    return (
        <div className="add-people-to-chat box-shadow">
            <AddPeople
                user={me}
                communityUsersId={groupChatUsersId}
                peopleToJoin={peopleToJoin}
                setPeopleToJoin={setPeopleToJoin}
            />
            <div className="item-result">
                <div className="btn-border-shadow invite" onClick={createGroupChatUserAsync}>{t("Invite")}</div>
                <div className="btn-border-shadow" onClick={() => setShowAddPeople(false)}>{t("Close")}</div>
            </div>
        </div>
    );
}

export default GroupChatAddUser;