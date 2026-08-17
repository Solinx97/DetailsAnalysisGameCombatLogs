import { type ChangeEvent, type SetStateAction } from 'react';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';
import CommunityMemberItem from './CommunityMemberItem';

interface CommunityUsersItemProps {
    myself: AppUserModel;
    communityUser: CommunityUserModel;
    usersToRemove: CommunityUserModel[];
    setUsersToRemove(value: SetStateAction<CommunityUserModel[]>): void;
    showRemoveUser: boolean;
}

const CommunityUsersItem: React.FC<CommunityUsersItemProps> = ({ myself, communityUser, usersToRemove, setUsersToRemove, showRemoveUser }) => {
    const addUserToUsersToRemove = (communityUser: CommunityUserModel) => {
        const users = usersToRemove;
        users.push(communityUser);

        setUsersToRemove(users);
    }

    const excludeUserFromUsersToRemove = (communityUser: CommunityUserModel) => {
        const people = usersToRemove.filter(user => user.id !== communityUser.id);

        setUsersToRemove(people);
    }

    const removeUserHandle = (e: ChangeEvent<HTMLInputElement>, communityUser: CommunityUserModel) => {
        const checked = e.target.checked;

        if (checked) {
            addUserToUsersToRemove(communityUser);
        }
        else {
            excludeUserFromUsersToRemove(communityUser);
        }
    }

    return (
        <>
            <div className="user-target-community__information">
                <CommunityMemberItem
                    comunityUser={communityUser}
                />
                {(myself.id !== communityUser.appUserId && showRemoveUser) &&
                    <input className="form-check-input" type="checkbox"
                        onChange={(e) => removeUserHandle(e, communityUser)} />
                }
            </div>
        </>
    );
}

export default CommunityUsersItem;