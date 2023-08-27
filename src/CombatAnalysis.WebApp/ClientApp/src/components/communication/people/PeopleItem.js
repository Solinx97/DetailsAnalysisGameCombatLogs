import { useState } from 'react';
import User from '../User';

const PeopleItem = ({ me, people }) => {
    const [userInformation, setUserInformation] = useState(null);

    return (
        <div>
            <div className="card">
                <User
                    me={me}
                    targetCustomerId={people.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                />
            </div>
            {userInformation}
        </div>
    );
}

export default PeopleItem;