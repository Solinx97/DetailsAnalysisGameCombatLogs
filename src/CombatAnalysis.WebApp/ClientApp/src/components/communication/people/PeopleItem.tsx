import { useState } from 'react';
import { PeopleItemProps } from '../../../types/components/communication/people/PeopleItemProps';
import User from '../User';

const PeopleItem: React.FC<PeopleItemProps> = ({ me, targetUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    return (
        <>
            <div className="card box-shadow">
                <User
                    me={me}
                    targetUserId={targetUser.id}
                    setUserInformation={setUserInformation}
                />
            </div>
            <div className="people-user-information">
                {userInformation}
            </div>
        </>
    );
}

export default PeopleItem;