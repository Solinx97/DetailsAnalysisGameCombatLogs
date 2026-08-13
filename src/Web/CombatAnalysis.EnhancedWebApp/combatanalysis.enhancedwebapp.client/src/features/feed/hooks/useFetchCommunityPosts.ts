import { useRef, type RefObject } from 'react';
import { useGetCommunityPostsByCommunityIdQuery } from '../api/Post.api';
import type { CommunityPostModel } from '../types/CommunityPostModel';

interface useFetchCommunityPostsResult {
    posts: CommunityPostModel[] | undefined;
    isLoading: boolean;
    isFetching: boolean;
    count: number;
    currentDateRef: RefObject<string>;
}

const useFetchCommunityPosts = (page: number, pageSize: number, communityId: number): useFetchCommunityPostsResult => {
    const currentDateRef = useRef<string>((new Date()).toISOString());

    const { data: posts, isLoading, isFetching } = useGetCommunityPostsByCommunityIdQuery({ communityId: communityId, page, pageSize });

    return { posts: posts?.posts, isLoading, isFetching, count: posts?.count ?? 0, currentDateRef };
}

export default useFetchCommunityPosts;