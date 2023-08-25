import { useState } from 'react';
import User from '../User';

const PeopleItem = ({ people }) => {
    const [userInformation, setUserInformation] = useState(null);

    return (
        <div>
            <div className="card">
                <User
                    userId={people.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                />
            </div>
            {userInformation}
        </div>
    );
}

export default PeopleItem;