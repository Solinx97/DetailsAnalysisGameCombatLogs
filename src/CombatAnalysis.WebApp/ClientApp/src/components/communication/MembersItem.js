import { useState } from "react";
import User from "./User";

const MembersItem = ({ me, user, communityItem, peopleToRemove, setPeopleToRemove, showRemoveUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    const addPeopleForRemove = (user) => {
        const people = peopleToRemove;
        people.push(user);

        setPeopleToRemove(people);
    }

    const removePeopleFromForRemove = (user) => {
        const people = peopleToRemove.filter((item) => item.id !== user.id);

        setPeopleToRemove(people);
    }

    const handleRemoveUser = (e, user) => {
        const checked = e.target.checked;

        checked ? addPeopleForRemove(user) : removePeopleFromForRemove(user);
    }

    return (
        <>
            <div className="user-target-community__information">
                <User
                    me={me}
                    targetCustomerId={user.customerId}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                />
                {(me?.id === communityItem.customerId && user.customerId !== communityItem.customerId
                    && showRemoveUser) &&
                    <input className="form-check-input" type="checkbox" onChange={(e) => handleRemoveUser(e, user)} />
                }
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default MembersItem;