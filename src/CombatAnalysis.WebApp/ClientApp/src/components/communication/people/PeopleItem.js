import { useState } from 'react';
import User from '../User';

const PeopleItem = ({ me, people, actionAfterRequests = null }) => {
    const [userInformation, setUserInformation] = useState(null);

    return (
        <>
            <div className="card box-shadow">
                <User
                    me={me}
                    targetCustomerId={people.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                    actionAfterRequests={actionAfterRequests}
                />
            </div>
            {userInformation}
        </>
    );
}

export default PeopleItem;