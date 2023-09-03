import { useState } from 'react';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import User from '../User';

const CommunityMemberItem = ({ comunityUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    const { data: member, isLoading } = useGetCustomerByIdQuery(comunityUser.customerId);

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            <User
                targetCustomerId={member?.id}
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