import { useEffect, useState } from 'react';
import { useLazyCommunityPostSearchByCommunityIdAsyncQuery, useLazyUserPostSearchByOwnerIdQuery } from '../store/api/CommunityApi';
import { useLazyGetUserPostByIdQuery } from '../store/api/communication/UserPost.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/communication/community/CommunityUser.api';

const checkNewPostsInterval = 1000;

const useFetchFriendsPosts = (appUserId, myFriends) => {
    const [allPosts, setAllPosts] = useState([]);
    const [allMyPersonalPosts, setAllMyPersonalPosts] = useState([]);
    const [postsAreLoading, setPostsAreLoading] = useState([]);

    const [getUserPostsAsync] = useLazyUserPostSearchByOwnerIdQuery();
    const [getCommunityUsers] = useLazySearchByUserIdAsyncQuery();
    const [getCommunityPosts] = useLazyCommunityPostSearchByCommunityIdAsyncQuery();
    const [getPostByIdAsync] = useLazyGetUserPostByIdQuery();

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
        const interval = setInterval(getAllUsersPostsAsync, checkNewPostsInterval);

        return () => clearInterval(interval);
    }, [myFriends]);


    const getAllMyPersonalPostsAsync = async () => {
        await getAllPostsAsync([appUserId]);
    }

    const getAllUsersPostsAsync = async () => {
        const peopleId = await getPeopleIdAsync();
        await getAllPostsAsync(peopleId);
    }

    const getAllPostsAsync = async (peopleId) => {
        const posts = await loadingPostsAsync(peopleId);

        setAllPosts(posts);
    }

    const loadingPostsAsync = async (peopleId) => {
        let posts = [];

        for (let i = 0; i < peopleId.length; i++) {
            const userPosts = await getUserPostsAsync(peopleId[i]);
            if (userPosts.data) {
                posts = [...posts, ...userPosts.data];
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

    const postsSortByTime = (postA, postB) => {
        if (postA.createdAt > postB.createdAt) {
            return -1;
        }

        if (postA.createdAt < postB.createdAt) {
            return 1;
        }

        return 0;
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