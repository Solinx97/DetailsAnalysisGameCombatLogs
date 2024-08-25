import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazyUserPostSearchByOwnerIdQuery } from '../../../store/api/CommunityApi';
import {
    useLazyGetUserPostByIdQuery,
    useRemoveUserPostMutation
} from '../../../store/api/communication/UserPost.api';
import Loading from '../../Loading';
import CreateUserPost from '../CreateUserPost';
import Post from '../Post';

const MyFeed = () => {
    const { t } = useTranslation("communication/feed");

    const user = useSelector((state) => state.user.value);

    const [getUserPosts] = useLazyUserPostSearchByOwnerIdQuery();
    const [getUserPostById] = useLazyGetUserPostByIdQuery();
    const [removeUserPost] = useRemoveUserPostMutation();

    const [allPosts, setAllPosts] = useState([]);

    useEffect(() => {
        if (user === null) {
            return;
        }

        const getAllPosts = async () => {
            await getAllPostsAsync();
        }

        getAllPosts();
    }, [user])

    const getAllPostsAsync = async () => {
        const userPosts = await getUserPosts(user.id);

        if (userPosts.data !== undefined) {
            const userPersonalPosts = await getUserPostsAsync(userPosts.data);
            setAllPosts(userPersonalPosts);
        }
    }

    const getUserPostsAsync = async (userPosts) => {
        const userPersonalPosts = [];

        for (let i = 0; i < userPosts.length; i++) {
            const post = await getUserPostById(userPosts[i].postId);

            if (post.data !== undefined) {
                userPersonalPosts.push(post.data);
            }
        }

        return userPersonalPosts;
    }

    const removeUserPostAsync = async (userPostId) => {
        await removeUserPost(userPostId);
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
                        <Post
                            customer={user}
                            data={post}
                            deletePostAsync={async () => await removeUserPostAsync(post.id)}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MyFeed;