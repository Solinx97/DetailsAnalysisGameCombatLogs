import { useEffect, useState } from 'react';
import { useLazyPostSearchByCommunityIdAsyncQuery, useLazyUserPostSearchByUserIdQuery } from '../store/api/ChatApi';
import { useLazyGetPostByIdQuery } from '../store/api/communication/Post.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/communication/community/CommunityUser.api';

const checkNewPostsInterval = 5000;

const useFetchFriendsPosts = (appUserId, myFriends) => {
    const [allPosts, setAllPosts] = useState([]);
    const [newPosts, setNewPosts] = useState([]);
    const [startDate, setStartDate] = useState(null);

    const [getUserPostsAsync] = useLazyUserPostSearchByUserIdQuery();
    const [getCommunityUsers] = useLazySearchByUserIdAsyncQuery();
    const [getCommunityPosts] = useLazyPostSearchByCommunityIdAsyncQuery();
    const [getPostByIdAsync] = useLazyGetPostByIdQuery();

    useEffect(() => {
        setStartDate(new Date());
    }, []);

    useEffect(() => {
        const checkNewPosts = async () => {
            const peopleId = await getPeopleIdAsync();
            await checkNewPostsAsync(peopleId);
        }

        const interval = setInterval(() => {
            checkNewPosts();
        }, [checkNewPostsInterval]);

        return () => clearInterval(interval);
    }, [startDate, myFriends]);

    useEffect(() => {
        const getAllPosts = async () => {
            const peopleId = await getPeopleIdAsync();
            await getAllPostsAsync(peopleId);
        }

        getAllPosts();
    }, [myFriends]);

    const checkNewPostsAsync = async (peopleId) => {
        const posts = await loadingPostsAsync(peopleId);

        const newPosts = posts.filter(post => new Date(post.when)?.getTime() > startDate?.getTime());

        setNewPosts(newPosts);
    }

    const insertNewPostsAsync = async () => {
        const insertedNewPosts = [...newPosts, ...allPosts];

        setAllPosts(insertedNewPosts);
        setStartDate(new Date());
        setNewPosts([]);
    }

    const loadingPostsAsync = async (peopleId = []) => {
        let posts = [];

        for (let i = 0; i < peopleId.length; i++) {
            const userPosts = await getUserPostsAsync(peopleId[i]);
            if (userPosts.data !== undefined) {
                const userPersonalPosts = await getPostsAsync(userPosts.data);
                posts = [...posts, ...userPersonalPosts];
            }
        }

        const communitiesUser = await getCommunityUsers(appUserId);
        if (communitiesUser.data !== undefined) {
            const userCommunityPosts = await getCommunityPostsAsync(communitiesUser.data);
            posts = [...posts, ...userCommunityPosts];
        }

        posts = posts.sort(postsSortByTime);

        return posts;
    }

    const getPeopleIdAsync = async () => {
        const friendsId = [];
        const allPeopleId = [appUserId];

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
            const post = await getPostByIdAsync(communityPosts[i].postId);

            if (post.data !== undefined) {
                posts.push(post.data);
            }
        }

        return posts;
    }

    return { getPeopleIdAsync, loadingPostsAsync, allPosts, setAllPosts, newPosts, insertNewPostsAsync };
}

export default useFetchFriendsPosts;