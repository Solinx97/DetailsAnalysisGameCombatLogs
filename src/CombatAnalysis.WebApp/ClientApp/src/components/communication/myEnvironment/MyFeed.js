import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazyUserPostSearchByOwnerIdQuery } from '../../../store/api/CommunityApi';
import {
    useLazyGetUserPostByIdQuery
} from '../../../store/api/communication/UserPost.api';
import Loading from '../../Loading';
import CreateUserPost from '../post/CreateUserPost';
import UserPost from '../post/UserPost';

const MyFeed = () => {
    const { t } = useTranslation("communication/feed");

    const user = useSelector((state) => state.user.value);

    const [getUserPosts] = useLazyUserPostSearchByOwnerIdQuery();
    const [getUserPostById] = useLazyGetUserPostByIdQuery();

    const [allPosts, setAllPosts] = useState([]);

    useEffect(() => {
        if (!user) {
            return;
        }

        const getAllPosts = async () => {
            await getAllPostsAsync();
        }

        getAllPosts();
    }, [user])

    const getAllPostsAsync = async () => {
        const userPosts = await getUserPosts(user.id);

        if (userPosts.data) {
            const userPersonalPosts = await getUserPostsAsync(userPosts.data);
            setAllPosts(userPersonalPosts);
        }
    }

    const getUserPostsAsync = async (userPosts) => {
        const userPersonalPosts = [];

        for (let i = 0; i < userPosts.length; i++) {
            const post = await getUserPostById(userPosts[i].postId);

            if (post.data) {
                userPersonalPosts.push(post.data);
            }
        }

        return userPersonalPosts;
    }

    if (!user) {
        return (<Loading />);
    }

    return (
        <div>
            <CreateUserPost
                user={user}
                owner={user?.username}
                t={t}
            />
            <ul className="posts">
                {allPosts?.map(post => (
                    <li className="posts__item" key={post.id}>
                        <UserPost
                            user={user}
                            post={post}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MyFeed;