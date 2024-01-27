import { useEffect, useState } from 'react';
import { useLazyPostSearchByCommunityIdAsyncQuery, useLazyUserPostSearchByUserIdQuery } from '../../store/api/ChatApi';
import { useLazyGetPostByIdQuery, useRemovePostMutation } from '../../store/api/communication/Post.api';
import { useRemoveUserPostAsyncMutation, useLazyUserPostSearchByPostIdQuery } from '../../store/api/communication/UserPost.api';
import { useLazySearchByUserIdAsyncQuery } from '../../store/api/communication/community/CommunityUser.api';
import { useFriendSearchMyFriendsQuery } from '../../store/api/communication/myEnvironment/Friend.api';
import Post from './Post';

const postType = {
    user: 0,
    community: 1
}

const FeedParticipants = ({ customer }) => {
    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(customer?.id);

    const [getUserPosts] = useLazyUserPostSearchByUserIdQuery();
    const [getCommunityUsers] = useLazySearchByUserIdAsyncQuery();

    const [getCommunityPosts] = useLazyPostSearchByCommunityIdAsyncQuery();
    const [getPostById] = useLazyGetPostByIdQuery();
    const [getUserPostByPostId] = useLazyUserPostSearchByPostIdQuery();
    const [removePost] = useRemovePostMutation();
    const [removeUserPost] = useRemoveUserPostAsyncMutation();

    const [peopleId, setPeopleId] = useState([customer?.id]);
    const [allPosts, setAllPosts] = useState([]);

    useEffect(() => {
        if (myFriends === undefined) {
            return;
        }

        getPeopleId();
    }, [myFriends])

    useEffect(() => {
        const getAllPosts = async () => {
            await getAllPostsAsync();
        }

        getAllPosts();
    }, [peopleId])

    const getPeopleId = () => {
        const friendsId = [];

        for (let i = 0; i < myFriends?.length; i++) {
            const personId = myFriends[i].whoFriendId === customer?.id ? myFriends[i].forWhomId : myFriends[i].whoFriendId;
            friendsId.push(personId);
        }

        setPeopleId([...peopleId, ...friendsId]);
    }

    const getAllPostsAsync = async () => {
        let posts = [];

        for (let i = 0; i < peopleId.length; i++) {
            const userPosts = await getUserPosts(peopleId[i]);
            if (userPosts.data !== undefined) {
                const userPersonalPosts = await getUserPostsAsync(userPosts.data);
                posts = [...posts, ...userPersonalPosts];
            }
        }

        const communitiesUser = await getCommunityUsers(customer.id);
        if (communitiesUser.data !== undefined) {
            const userCommunityPosts = await getCommunityPostsAsync(communitiesUser.data);
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

    const removeUserPostAsync = async (postId) => {
        const userPost = await getUserPostByPostId(postId);
        if (userPost.data === undefined || userPost.data.length === 0) {
            return;
        }

        const result = await removeUserPost(userPost.data[0].id);
        if (result.error === undefined) {
            await removePost(postId);
        }
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
                        deletePostAsync={async () => await removeUserPostAsync(post.id)}
                        canBeRemoveFromUserFeed={post.postType === postType["user"]}
                    />
                </li>
            ))}
        </ul>
    );
}

export default FeedParticipants;