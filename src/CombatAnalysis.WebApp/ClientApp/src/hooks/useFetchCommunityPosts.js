import { useEffect, useRef } from 'react';
import {
    useGetCommunityPostsByCommunityIdQuery,
    useGetNewCommunityPostsByCommunityIdQuery,
    useLazyGetMoreCommunityPostsByCommunityIdQuery
} from '../store/api/CommunityApi';
import { useGetCommunityPostCountByCommunityIdQuery } from '../store/api/communication/CommunityPost.api';

const getCommunityPostsInterval = 2000;

const useFetchCommunityPosts = (communityId) => {
    const pageSizeRef = useRef(1);
    const currentDateRef = useRef((new Date()).toISOString());
    const skipCheckNewPostsRef = useRef(true);

    const { data: posts, isLoading } = useGetCommunityPostsByCommunityIdQuery({ communityId, pageSize: pageSizeRef.current });
    const { data: newPosts } = useGetNewCommunityPostsByCommunityIdQuery({ communityId, checkFrom: currentDateRef.current }, {
        pollingInterval: getCommunityPostsInterval,
        skipPollingIfUnfocused: skipCheckNewPostsRef.current,
    });
    const { data: count, isLoading: countIsLoading } = useGetCommunityPostCountByCommunityIdQuery(communityId);

    const [getMoreCommunityPosts] = useLazyGetMoreCommunityPostsByCommunityIdQuery();

    useEffect(() => {
        pageSizeRef.current = process.env.REACT_APP_COMMUNITY_POST_PAGE_SIZE;
    }, []);

    const getMoreCommunityPostsAsync = async (currentCommunityPostsSize) => {
        const arg = {
            communityId,
            offset: currentCommunityPostsSize,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreCommunityPosts(arg);
        if (!response.error) {
            return response.data;
        }

        return [];
    }

    return { posts, newPosts, count, isLoading: isLoading || countIsLoading, getMoreCommunityPostsAsync, currentDateRef, skipCheckNewPostsRef };
}

export default useFetchCommunityPosts;