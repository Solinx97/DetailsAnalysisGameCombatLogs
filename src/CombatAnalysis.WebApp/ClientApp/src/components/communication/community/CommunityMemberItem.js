import { useState } from 'react';
import { useGetUserByIdQuery } from '../../../store/api/Account.api';
import User from '../User';

const CommunityMemberItem = ({ comunityUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    const { data: member, isLoading } = useGetUserByIdQuery(comunityUser.appUserId);

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            <User
                targetUserId={member?.id}
                setUserInformation={setUserInformation}
                allowRemoveFriend={false}
            />
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default CommunityMemberItem;