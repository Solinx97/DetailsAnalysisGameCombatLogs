import { useState } from "react";
import { CommunityUser } from "../../../types/CommunityUser";
import { CommunityUsersItemProps } from "../../../types/components/communication/community/CommunityUsersItemProps";
import User from "../User";

const CommunityUsersItem: React.FC<CommunityUsersItemProps> = ({ me, communityUser, usersToRemove, setUsersToRemove, showRemoveUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    const addUserToUsersToRemove = (communityUser: CommunityUser) => {
        const users = usersToRemove;
        users.push(communityUser);

        setUsersToRemove(users);
    }

    const excludeUserFromUsersToRemove = (communityUser: CommunityUser) => {
        const people = usersToRemove.filter(user => user.id !== communityUser.id);

        setUsersToRemove(people);
    }

    const handleRemoveUser = (e: any, communityUser: CommunityUser) => {
        const checked = e.target.checked;

        checked ? addUserToUsersToRemove(communityUser) : excludeUserFromUsersToRemove(communityUser);
    }

    return (
        <>
            <div className="user-target-community__information">
                <User
                    me={me}
                    targetUserId={communityUser.appUserId}
                    setUserInformation={setUserInformation}
                />
                {(me.id !== communityUser.appUserId && showRemoveUser) &&
                    <input className="form-check-input" type="checkbox" onChange={(e) => handleRemoveUser(e, communityUser)} />
                }
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default CommunityUsersItem;