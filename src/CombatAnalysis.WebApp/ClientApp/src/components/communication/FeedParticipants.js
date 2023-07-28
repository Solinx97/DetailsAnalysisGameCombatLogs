import { useEffect, useState } from 'react';
import { useFriendSearchByUserIdQuery } from '../../store/api/UserApi';
import UserPosts from './UserPosts';

const FeedParticipants = ({ customer }) => {
    const { data: friends, isLoading } = useFriendSearchByUserIdQuery(customer?.id);

    const [peopleId, setPeopleId] = useState([customer?.id]);

    useEffect(() => {
        !isLoading && getPeopleId();
    }, [isLoading])

    const getPeopleId = () => {
        const friendsId = [];
        for (let i = 0; i < friends?.length; i++) {
            const personId = friends[i].whoFriendId === customer?.id ? friends[i].forWhomId : friends[i].whoFriendId;
            friendsId.push(personId);
        }

        setPeopleId([...peopleId, ...friendsId]);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <ul>
            {peopleId.map(item => (
                <span key={item}>
                    <UserPosts
                        customer={customer}
                        userId={item}
                    />
                </span>
            ))}
        </ul>
    );
}

export default FeedParticipants;