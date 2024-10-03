import { useEffect, useRef, useState } from 'react';
import {
    useGetNewCommunityPostByListOfCommunityIdsQuery,
    useGetNewUserPostByListOfUserIdsQuery,
    useLazyGetCommunityPostByListOfCommunityIdsQuery,
    useLazyGetMoreCommunityPostByListOfCommunityIdsQuery,
    useLazyGetMoreUserPostByListOfUserIdsQuery,
    useLazyGetUserPostByListOfUserIdsQuery
} from '../store/api/CommunityApi';
import { useLazyGetCommunityPostCountByListOfCommunityIdQuery } from '../store/api/communication/CommunityPost.api';
import { useLazyGetUserPostCountByListOfUserIdQuery } from '../store/api/communication/UserPost.api';
import { useSearchByUserIdAsyncQuery } from '../store/api/communication/community/CommunityUser.api';
import { useFriendSearchMyFriendsQuery } from '../store/api/communication/myEnvironment/Friend.api';

const getUserPostsInterval = 2000;

const useFetchUsersPosts = (appUserId, skipFriendPosts) => {
    const pageSizeRef = useRef(1);
    const currentDateRef = useRef((new Date()).toISOString());
    const skipCheckNewPostsRef = useRef(true);
    const appUserIdsRef = useRef("");
    const communityIdsRef = useRef("0");

    const [posts, setPosts] = useState([]);
    const [communityPosts, setCommunityPosts] = useState([]);
    const [count, setCount] = useState(0);
    const [communityCount, setCommunityCount] = useState(0);

    const { data: myFriends, isLoading: friendsAreLoading } = useFriendSearchMyFriendsQuery(appUserId, {
        skip: skipFriendPosts,
    });
    const { data: myCommunitiesUsers, isLoading: communitiesAreLoading } = useSearchByUserIdAsyncQuery(appUserId);
    const { data: newPosts } = useGetNewUserPostByListOfUserIdsQuery({ appUserIds: appUserIdsRef.current, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        skip: skipCheckNewPostsRef.current || skipFriendPosts,
        selectFromResult: ({ data }) => ({ data }),
    });
    const { data: newCommunityPosts } = useGetNewCommunityPostByListOfCommunityIdsQuery({ communityIds: communityIdsRef.current, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        skip: skipCheckNewPostsRef.current || skipFriendPosts,
        selectFromResult: ({ data }) => ({ data }),
    });

    const [getPostsCountAsync] = useLazyGetUserPostCountByListOfUserIdQuery();
    const [getCommunityPostsCountAsync] = useLazyGetCommunityPostCountByListOfCommunityIdQuery();
    const [getUserPostByListOfUserIdsAsync] = useLazyGetUserPostByListOfUserIdsQuery();
    const [getCommunityPostByListOfCommunityIdsAsync] = useLazyGetCommunityPostByListOfCommunityIdsQuery();
    const [getMoreUsersPosts] = useLazyGetMoreUserPostByListOfUserIdsQuery();
    const [getMoreCommunityPosts] = useLazyGetMoreCommunityPostByListOfCommunityIdsQuery();

    useEffect(() => {
        pageSizeRef.current = process.env.REACT_APP_COMMUNITY_POST_PAGE_SIZE;
    }, []);

    useEffect(() => {
        const getUserPostByListOfUserIds = async () => {
            appUserIdsRef.current = appUserId;

            let response = await getPostsCountAsync(appUserIdsRef.current);
            if (!response.error) {
                setCount(response.data);
            }

            const arg = {
                appUserIds: appUserIdsRef.current,
                pageSize: pageSizeRef.current
            };

            response = await getUserPostByListOfUserIdsAsync(arg);
            if (!response.error) {
                setPosts(response.data);
            }
        }

        getUserPostByListOfUserIds();
    }, [appUserId]);

    useEffect(() => {
        if (!myFriends) {
            return;
        }

        const getUserPostByListOfUserIds = async () => {
            const appUserIds = myFriends
                ? myFriends.map(friend => friend.whoFriendId === appUserId
                    ? friend.forWhomId
                    : friend.whoFriendId)
                : [];
            appUserIds.push(appUserId);

            appUserIdsRef.current = appUserIds.join(',');

            let response = await getPostsCountAsync(appUserIdsRef.current);
            if (!response.error) {
                setCount(response.data);
            }

            const arg = {
                appUserIds: appUserIdsRef.current,
                pageSize: pageSizeRef.current
            };

            response = await getUserPostByListOfUserIdsAsync(arg);
            if (!response.error) {
                setPosts(response.data);

                skipCheckNewPostsRef.current = false;
            }
        }

        getUserPostByListOfUserIds();
    }, [myFriends]);

    useEffect(() => {
        if (!myCommunitiesUsers) {
            return;
        }

        const getCommunityPostByListOfCommunityIds = async () => {
            const communityIds = myCommunitiesUsers
                ? myCommunitiesUsers.map(user => user.communityId)
                : [];

            if (communityIds.length === 0) {
                return;
            }

            communityIdsRef.current = communityIds.join(',');

            let response = await getCommunityPostsCountAsync(communityIdsRef.current);
            if (!response.error) {
                setCommunityCount(response.data);
            }

            const arg = {
                communityIds: communityIdsRef.current,
                pageSize: pageSizeRef.current
            };

            response = await getCommunityPostByListOfCommunityIdsAsync(arg);
            if (!response.error) {
                setCommunityPosts(response.data);
            }
        }

        getCommunityPostByListOfCommunityIds();
    }, [myCommunitiesUsers]);

    const getMoreUserPostsAsync = async (currentPostsSize) => {
        const arg = {
            appUserIds: appUserIdsRef.current,
            offset: currentPostsSize,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreUsersPosts(arg);
        if (!response.error) {
            return response.data;
        }

        return [];
    }

    const getMoreCommunityPostsAsync = async (currentPostsSize) => {
        const arg = {
            communityIds: communityIdsRef.current,
            offset: currentPostsSize,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreCommunityPosts(arg);
        if (!response.error) {
            return response.data;
        }

        return [];
    }

    return { posts, communityPosts, newPosts, newCommunityPosts, count, communityCount, isLoading: friendsAreLoading || communitiesAreLoading, getMoreUserPostsAsync, getMoreCommunityPostsAsync, currentDateRef };
}

export default useFetchUsersPosts;