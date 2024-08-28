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
    const offsetRef = useRef(0);
    const currentDateRef = useRef((new Date()).toISOString());
    const skipCheckNewPostsRef = useRef(true);

    const { data: communityPosts, isLoading } = useGetCommunityPostsByCommunityIdQuery({ communityId, pageSize: pageSizeRef.current });
    const { data: newCommunityPosts } = useGetNewCommunityPostsByCommunityIdQuery({ communityId, checkFrom: currentDateRef.current }, {
        pollingInterval: getCommunityPostsInterval,
        skipPollingIfUnfocused: skipCheckNewPostsRef.current,
    });
    const { data: count, isLoading: countIsLoading } = useGetCommunityPostCountByCommunityIdQuery(communityId);

    const [getMoreCommunityPosts] = useLazyGetMoreCommunityPostsByCommunityIdQuery();

    useEffect(() => {
        pageSizeRef.current = process.env.REACT_APP_COMMUNITY_POST_PAGE_SIZE;
    }, []);

    const getMoreCommunityPostsAsync = async () => {
        offsetRef.current += +pageSizeRef.current;

        const arg = {
            communityId,
            offset: offsetRef.current,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreCommunityPosts(arg);
        if (!response.error) {
            return response.data;
        }

        return [];
    }

    return { communityPosts, newCommunityPosts, count, isLoading: isLoading || countIsLoading, getMoreCommunityPostsAsync, currentDateRef, skipCheckNewPostsRef };
}

export default useFetchCommunityPosts;