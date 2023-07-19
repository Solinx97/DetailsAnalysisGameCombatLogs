import { useUserPostSearchByUserIdQuery as useSearchByUserIdQuery } from '../../store/api/ChatApi';
import Post from './Post';

const UserPosts = ({ customer, userId }) => {
    const { data: userPosts, isLoading } = useSearchByUserIdQuery(userId);

    if (isLoading) {
        return <>Loading...</>;
    }

    return userPosts.map((item) => (
        <li key={item?.id}>
            <Post
                key={item?.id}
                customer={customer}
                targetPostType={item}
            />
        </li>
    ));
}

export default UserPosts;