import { useEffect, useRef } from 'react';
import {
    useGetUserPostsByUserIdQuery,
    useGetNewUserPostsByUserIdQuery,
    useLazyGetMoreUserPostsByUserIdQuery
} from '../store/api/CommunityApi';
import { useGetUserPostCountByUserIdQuery } from '../store/api/communication/UserPost.api';

const getUserPostsInterval = 2000;

const useFetchUsersPosts = (appUserId) => {
    const pageSizeRef = useRef(1);
    const currentDateRef = useRef((new Date()).toISOString());
    const skipCheckNewPostsRef = useRef(true);

    const { data: posts, isLoading } = useGetUserPostsByUserIdQuery({ appUserId, pageSize: pageSizeRef.current });
    const { data: newPosts } = useGetNewUserPostsByUserIdQuery({ appUserId, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        skipPollingIfUnfocused: skipCheckNewPostsRef.current,
    });
    const { data: count, isLoading: countIsLoading } = useGetUserPostCountByUserIdQuery(appUserId);

    const [getMoreUsersPosts] = useLazyGetMoreUserPostsByUserIdQuery();

    useEffect(() => {
        pageSizeRef.current = process.env.REACT_APP_COMMUNITY_POST_PAGE_SIZE;
    }, []);

    const getMoreUserPostsAsync = async (currentCommunityPostsSize) => {
        const arg = {
            appUserId,
            offset: currentCommunityPostsSize,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreUsersPosts(arg);
        if (!response.error) {
            return response.data;
        }

        return [];
    }

    return { posts, newPosts, count, isLoading: isLoading || countIsLoading, getMoreUserPostsAsync, currentDateRef, skipCheckNewPostsRef };
}

export default useFetchUsersPosts;