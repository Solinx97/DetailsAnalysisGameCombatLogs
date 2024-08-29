import { useEffect, useRef, useState } from 'react';
import {
    useGetNewUserPostByListOfUserIdsQuery,
    useLazyGetMoreUserPostByListOfUserIdsQuery,
    useLazyGetUserPostByListOfUserIdsQuery
} from '../store/api/CommunityApi';
import { useLazyGetUserPostCountByListOfUserIdQuery } from '../store/api/communication/UserPost.api';
import { useFriendSearchMyFriendsQuery } from '../store/api/communication/myEnvironment/Friend.api';

const getUserPostsInterval = 2000;

const useFetchUsersPosts = (appUserId, skipFriendPosts) => {
    const pageSizeRef = useRef(1);
    const currentDateRef = useRef((new Date()).toISOString());
    const skipCheckNewPostsRef = useRef(true);
    const appUserIdsRef = useRef("");

    const [posts, setPosts] = useState([]);
    const [count, setCount] = useState(0);

    const { data: myFriends, isLoading: friendsIsLoading } = useFriendSearchMyFriendsQuery(appUserId, {
        skip: skipFriendPosts,
    });
    const { data: newPosts } = useGetNewUserPostByListOfUserIdsQuery({ appUserIds: appUserIdsRef.current, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        skip: skipCheckNewPostsRef.current || skipFriendPosts,
    });

    const [getPostsCountAsync] = useLazyGetUserPostCountByListOfUserIdQuery();
    const [getUserPostByListOfUserIdsAsync] = useLazyGetUserPostByListOfUserIdsQuery();
    const [getMoreUsersPosts] = useLazyGetMoreUserPostByListOfUserIdsQuery();

    useEffect(() => {
        pageSizeRef.current = process.env.REACT_APP_COMMUNITY_POST_PAGE_SIZE;
    }, []);

    useEffect(() => {
        if (!skipFriendPosts) {
            return;
        }

        const getUserPostByListOfUserIds = async () => {
            appUserIdsRef.current = appUserId;

            let response = await getPostsCountAsync(appUserIdsRef.current);
            if (!response.error) {
                setCount(response.data);
            }

            const arg = {
                appUserIds: appUserId,
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
        if (!myFriends && skipFriendPosts) {
            return;
        }

        const getUserPostByListOfUserIds = async () => {
            const appUserIds = myFriends ?
                myFriends.map(friend => friend.whoFriendId === appUserId
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

    const getMoreUserPostsAsync = async (currentCommunityPostsSize) => {
        const arg = {
            appUserIds: appUserIdsRef.current,
            offset: currentCommunityPostsSize,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreUsersPosts(arg);
        if (!response.error) {
            return response.data;
        }

        return [];
    }

    return { posts, newPosts, count, isLoading: friendsIsLoading, getMoreUserPostsAsync, currentDateRef };
}

export default useFetchUsersPosts;