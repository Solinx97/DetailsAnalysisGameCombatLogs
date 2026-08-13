import { useRef, type RefObject } from 'react';
import { useGetFeedQuery } from '../api/UserFeed.api';
import type { UserFeedModel } from '../types/UserFeedModel';

interface useFetchUserPostsResult {
    posts: UserFeedModel[] | undefined;
    isLoading: boolean;
    isFetching: boolean;
    count: number;
    currentDateRef: RefObject<string>;
}

const useFetchUserPosts = (page: number, pageSize: number, appUserId: string): useFetchUserPostsResult => {
    const currentDateRef = useRef<string>((new Date()).toISOString());

    const { data: userFeed, isLoading, isFetching } = useGetFeedQuery({ appUserId, page, pageSize });

    return { posts: userFeed?.posts, isLoading, isFetching, count: userFeed?.count ?? 0, currentDateRef };
}

export default useFetchUserPosts;