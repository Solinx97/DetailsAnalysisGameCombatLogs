import { useRef, type RefObject } from 'react';
import { useGetCommunityPostsByCommunityIdQuery } from '../api/Post.api';
import { useGetCommunityPostCountByCommunityIdQuery } from '../api/CommunityPost.api';
import type { CommunityPostModel } from '../types/CommunityPostModel';

interface useFetchCommunityPostsResult {
    communityPosts: CommunityPostModel[] | undefined;
    communityPostIsLoading: boolean;
    communityPostCount: number;
    currentDateRef: RefObject<string>;
}

const useFetchCommunityPosts = (page: number, pageSize: number, communityId: number): useFetchCommunityPostsResult => {
    const currentDateRef = useRef<string>((new Date()).toISOString());

    const { data: communityPosts, isLoading: communityPostIsLoading } = useGetCommunityPostsByCommunityIdQuery({ communityId: communityId, page, pageSize });
    const { data: communityPostCount } = useGetCommunityPostCountByCommunityIdQuery(communityId );

    return { communityPosts, communityPostIsLoading, communityPostCount: communityPostCount ?? 0, currentDateRef };
}

export default useFetchCommunityPosts;