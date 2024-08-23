import { useEffect, useState } from 'react';
import { useLazyPostSearchByCommunityIdAsyncQuery, useLazyUserPostSearchByUserIdQuery } from '../store/api/ChatApi';
import { useLazyGetPostByIdQuery } from '../store/api/communication/Post.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/communication/community/CommunityUser.api';

const checkNewPostsInterval = 1000;

const useFetchFriendsPosts = (appUserId, myFriends) => {
    const [allPosts, setAllPosts] = useState([]);
    const [allMyPersonalPosts, setAllMyPersonalPosts] = useState([]);
    const [postsAreLoading, setPostsAreLoading] = useState([]);

    const [getUserPostsAsync] = useLazyUserPostSearchByUserIdQuery();
    const [getCommunityUsers] = useLazySearchByUserIdAsyncQuery();
    const [getCommunityPosts] = useLazyPostSearchByCommunityIdAsyncQuery();
    const [getPostByIdAsync] = useLazyGetPostByIdQuery();

    useEffect(() => {
        setPostsAreLoading(true);

        getAllMyPersonalPostsAsync().then(() => {
            setPostsAreLoading(false);
        });
    }, []);

    useEffect(() => {
        const interval = setInterval(getAllMyPersonalPostsAsync, checkNewPostsInterval);

        return () => clearInterval(interval);
    }, [appUserId]);

    useEffect(() => {
        const interval = setInterval(getAllPosts, checkNewPostsInterval);

        return () => clearInterval(interval);
    }, [myFriends]);

    const getAllMyPersonalPostsAsync = async () => {
        const userPosts = await getUserPostsAsync(appUserId);
        if (userPosts.data) {
            const userPersonalPosts = await getPostsAsync(userPosts.data);
            setAllMyPersonalPosts(userPersonalPosts);
        }
    }

    const getAllPosts = async () => {
        const peopleId = await getPeopleIdAsync();
        await getAllPostsAsync(peopleId);
    }

    const loadingPostsAsync = async (peopleId = []) => {
        let posts = [];

        for (let i = 0; i < peopleId.length; i++) {
            const userPosts = await getUserPostsAsync(peopleId[i]);
            if (userPosts.data) {
                const userPersonalPosts = await getPostsAsync(userPosts.data);
                posts = [...posts, ...userPersonalPosts];
            }
        }

        const communitiesUser = await getCommunityUsers(appUserId);
        if (!communitiesUser.data) {
            const userCommunityPosts = await getCommunityPostsAsync(communitiesUser.data);
            posts = [...posts, ...userCommunityPosts];
        }

        posts = posts.sort(postsSortByTime);

        return posts;
    }

    const getPeopleIdAsync = async () => {
        const friendsId = [];
        const allPeopleId = [];

        for (let i = 0; i < myFriends?.length; i++) {
            const personId = myFriends[i].whoFriendId === appUserId ? myFriends[i].forWhomId : myFriends[i].whoFriendId;
            friendsId.push(personId);
        }

        const temporaryPeopleId = [...allPeopleId, ...friendsId];

        return temporaryPeopleId
    }

    const getAllPostsAsync = async (peopleId) => {
        const posts = await loadingPostsAsync(peopleId);

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

    const getPostsAsync = async (userPosts) => {
        const userPersonalPosts = [];

        for (let i = 0; i < userPosts?.length; i++) {
            const post = await getPostByIdAsync(userPosts[i].postId);
            if (post.data) {
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
            const post = await getPostByIdAsync(communityPosts[i].postId);

            if (post.data !== undefined) {
                posts.push(post.data);
            }
        }

        return posts;
    }

    return { allMyPersonalPosts, allPosts, postsAreLoading };
}

export default useFetchFriendsPosts;