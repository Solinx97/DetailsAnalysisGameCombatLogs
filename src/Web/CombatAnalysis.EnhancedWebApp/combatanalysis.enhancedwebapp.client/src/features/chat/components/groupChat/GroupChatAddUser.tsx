import type { RootState } from '@/app/Store';
import AddPeople from '@/shared/components/AddPeople';
import logger from '@/utils/Logger';
import { useState, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useCreateGroupChatUserMutation } from '../../api/GroupChatUser.api';
import type { CreateGroupChatUserModel } from '../../types/CreateGroupChatUserModel';
import type { GroupChatModel } from '../../types/GroupChatModel';

interface GroupChatAddUserProps {
    chat: GroupChatModel;
    groupChatUsersId: string[];
    setShowAddPeople: (value: SetStateAction<boolean>) => void;
    t: (key: string) => string;
}

const GroupChatAddUser: React.FC<GroupChatAddUserProps> = ({ chat, groupChatUsersId, setShowAddPeople, t }) => {
    const myself = useSelector((state: RootState) => state.user.value);
    
    const [peopleToJoin, setPeopleToJoin] = useState<AppUserModel[]>([]);

    const [addUser] = useCreateGroupChatUserMutation();

    const createGroupChatUserAsync = async () => {
        if (!myself) {
            return;
        }

        try {
            for (let i = 0; i < peopleToJoin.length; i++) {
                const newGroupChatUser: CreateGroupChatUserModel = {
                    id: "",
                    username: peopleToJoin[i].username,
                    unreadMessages: 0,
                    groupChatId: chat.id,
                    appUserId: peopleToJoin[i].id,
                    whoAddId: myself.id
                };

                await addUser(newGroupChatUser).unwrap();
            }

            setPeopleToJoin([]);
            setShowAddPeople(false);
        } catch (e) {
            logger.error("Failed to add group chat users", e);
        }
    }

    return (
        <div className="add-people-to-chat box-shadow">
            <AddPeople
                usersId={groupChatUsersId}
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