import { useState } from 'react';
import User from '../User';

const PeopleItem = ({ me, people, actionAfterRequests = null }) => {
    const [userInformation, setUserInformation] = useState(null);

    return (
        <>
            <div className="card box-shadow">
                <User
                    me={me}
                    targetUserId={people.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                    actionAfterRequests={actionAfterRequests}
                />
            </div>
            <div className="people-user-information">
                {userInformation}
            </div>
        </>
    );
}

export default PeopleItem;