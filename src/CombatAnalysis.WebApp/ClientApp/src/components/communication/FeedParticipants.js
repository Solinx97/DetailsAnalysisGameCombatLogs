import { useEffect, useState } from 'react';
import { useLazyPostSearchByCommunityIdAsyncQuery, useLazyUserPostSearchByUserIdQuery } from '../../store/api/ChatApi';
import { useFriendSearchByUserIdQuery } from '../../store/api/UserApi';
import { useLazyGetPostByIdQuery } from '../../store/api/communication/Post.api';
import { useLazySearchByUserIdAsyncQuery } from '../../store/api/communication/community/CommunityUser.api';
import Post from './Post';

const FeedParticipants = ({ customer }) => {
    const { data: friends, isLoading } = useFriendSearchByUserIdQuery(customer?.id);

    const [getUserPosts] = useLazyUserPostSearchByUserIdQuery();
    const [getCommunityUsers] = useLazySearchByUserIdAsyncQuery();

    const [getCommunityPosts] = useLazyPostSearchByCommunityIdAsyncQuery();
    const [getPostById] = useLazyGetPostByIdQuery();

    const [peopleId, setPeopleId] = useState([customer?.id]);
    const [allPosts, setAllPosts] = useState([]);

    useEffect(() => {
        if (friends === undefined) {
            return;
        }

        getPeopleId();
    }, [friends])

    useEffect(() => {
        const getAllPosts = async () => {
            await getAllPostsAsync();
        }

        getAllPosts();
    }, [peopleId])

    const getPeopleId = () => {
        const friendsId = [];

        for (let i = 0; i < friends?.length; i++) {
            const personId = friends[i].whoFriendId === customer?.id ? friends[i].forWhomId : friends[i].whoFriendId;
            friendsId.push(personId);
        }

        setPeopleId([...peopleId, ...friendsId]);
    }

    const getAllPostsAsync = async () => {
        let posts = [];

        for (let i = 0; i < peopleId.length; i++) {
            let userPersonalPosts = [];
            let userCommunityPosts = [];

            const userPosts = await getUserPosts(peopleId[i]);
            if (userPosts.data !== undefined) {
                userPersonalPosts = await getUserPostsAsync(userPosts.data);
            }

            const communitiesUser = await getCommunityUsers(peopleId[i]);
            if (communitiesUser.data !== undefined) {
                userCommunityPosts = await getCommunityPostsAsync(communitiesUser.data);
            }

            posts = [...posts, ...userPersonalPosts];
            posts = [...posts, ...userCommunityPosts];
        }

        posts = posts.sort(postsSortByTime);

        setAllPosts(posts);
    }

    const postsSortByTime = (a, b) => {
        if (a.when > b.when) {
            return -1;
        }

        if (a.when < b.when) {
            return 1;
        }

        return 0;
    }

    const getUserPostsAsync = async (userPosts) => {
        const userPersonalPosts = [];

        for (let i = 0; i < userPosts.length; i++) {
            const post = await getPostById(userPosts[i].postId);

            if (post.data !== undefined) {
                userPersonalPosts.push(post.data);
            }
        }

        return userPersonalPosts;
    }

    const getCommunityPostsAsync = async (communitiesUser) => {
        let userCommunityPosts = [];

        for (let i = 0; i < communitiesUser.length; i++) {
            const communityPosts = await getCommunityPosts(communitiesUser[i].communityId);

            if (communityPosts.data !== undefined) {
                const posts = await getPostsByComunityPostsAsync(communityPosts.data);
                userCommunityPosts = [...userCommunityPosts, ...posts];
            }
        }

        return userCommunityPosts;
    }

    const getPostsByComunityPostsAsync = async (communityPosts) => {
        const posts = [];

        for (let i = 0; i < communityPosts.length; i++) {
            const post = await getPostById(communityPosts[i].postId);

            if (post.data !== undefined) {
                posts.push(post.data);
            }
        }

        return posts;
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <ul className="posts">
            {allPosts?.map(post => (
                <li key={post.id}>
                    <Post
                        customer={customer}
                        post={post}
                        deletePostAsync={null}
                    />
                </li>
            ))}
        </ul>
    );
}

export default FeedParticipants;