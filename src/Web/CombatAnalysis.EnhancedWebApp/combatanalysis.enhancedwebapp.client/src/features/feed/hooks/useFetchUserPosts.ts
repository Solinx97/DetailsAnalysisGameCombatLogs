import { useRef, type RefObject } from 'react';
import { useGetFeedQuery } from '../api/UserFeed.api';
import { useGetUserPostCountByUserIdQuery } from '../api/UserPost.api';
import type { UserFeedModel } from '../types/UserFeedModel';

interface useFetchUserPostsResult {
    userFeed: UserFeedModel[] | undefined;
    userFeedIsLoading: boolean;
    userPostCount: number;
    currentDateRef: RefObject<string>;
}

const useFetchUserPosts = (page: number, pageSize: number, appUserId: string): useFetchUserPostsResult => {
    const currentDateRef = useRef<string>((new Date()).toISOString());

    const { data: userFeed, isLoading: userFeedIsLoading } = useGetFeedQuery({ appUserId, page, pageSize });
    const { data: userPostCount } = useGetUserPostCountByUserIdQuery(appUserId);

    return { userFeed, userFeedIsLoading, userPostCount: userPostCount ?? 0, currentDateRef };
}

export default useFetchUserPosts;