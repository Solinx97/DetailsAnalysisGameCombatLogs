import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import { useEffect, useRef, useState, type RefObject } from 'react';
import { useCommunityUserFindByUserIdQuery } from '../../community/api/CommunityUser.api';
import type { CommunityUserModel } from '../../community/types/CommunityUserModel';
import { useFindFriendByUserIdQuery } from '../../user/api/Friend.api';
import type { FriendModel } from '../../user/types/FriendModel';
import { useLazyGetCommunityPostCountByListOfCommunityIdQuery } from '../api/CommunityPost.api';
import {
    useGetUserPostsByUserIdQuery,
    useGetCommunityPostsByCommunityIdQuery
} from '../api/Post.api';
import { useLazyGetUserPostCountByUserIdQuery } from '../api/UserPost.api';
import type { CommunityPostModel } from '../types/CommunityPostModel';
import type { UserPostModel } from '../types/UserPostModel';

// const getUserPostsInterval = 5000;

interface UseFetchUsersPostsResult {
    userPosts: UserPostModel[] | undefined;
    communityPosts: CommunityPostModel[] | undefined;
    // newPosts: UserPostModel[] | undefined;
    // newCommunityPosts: CommunityPostModel[] | undefined;
    count: number;
    communityCount: number;
    // getMoreUserPostsAsync: (currentPostsSize: number) => Promise<UserPostModel[]>;
    // getMoreCommunityPostsAsync: (currentPostsSize: number) => Promise<CommunityPostModel[]>;
    currentDateRef: RefObject<string>;
}

const useFetchPosts = (myselfId: string): UseFetchUsersPostsResult => {
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communityPostPageSize ?? 5);
    const currentDateRef = useRef<string>((new Date()).toISOString());
    const appUserIdsRef = useRef<string>(myselfId);
    const communityIdsRef = useRef<string>("0");

    const [count, setCount] = useState(0);
    const [communityCount, setCommunityCount] = useState(0);

    // const { data: myFriends } = useFindFriendByUserIdQuery(myselfId);
    // const { data: myCommunitiesUsers } = useCommunityUserFindByUserIdQuery(myselfId);
    const { data: userPosts } = useGetUserPostsByUserIdQuery({ appUserId: myselfId, page: 1, pageSize: pageSizeRef.current });
    // const { data: newPosts } = useGetNewUserPostByListOfUserIdQuery({ collectionUserId: appUserIdsRef.current, checkFrom: currentDateRef.current }, {
    //     pollingInterval: getUserPostsInterval,
    // });
    const { data: communityPosts } = useGetCommunityPostsByCommunityIdQuery({ communityId: 0, page: 1, pageSize: pageSizeRef.current });
    // const { data: newCommunityPosts } = useGetNewCommunityPostByListOfCommunityIdQuery({ collectionCommunityId: communityIdsRef.current, checkFrom: currentDateRef.current }, {
    //     pollingInterval: getUserPostsInterval,
    // });

    const [getUserPostCountByUserId] = useLazyGetUserPostCountByUserIdQuery();
    const [getCommunityPostsCount] = useLazyGetCommunityPostCountByListOfCommunityIdQuery();
    // const [getMoreUsersPosts] = useLazyGetMoreUserPostByListOfUserIdQuery();
    // const [getMoreCommunityPosts] = useLazyGetMoreCommunityPostByListOfCommunityIdQuery();

    useEffect(() => {
        const getUserPostCount = async () => {
            try {
                appUserIdsRef.current = myselfId;

                const count = await getUserPostCountByUserId(myselfId).unwrap();
                setCount(count);
            } catch (e) {
                logger.error("Failed to receive user post count", e);
            }
        }

        getUserPostCount();
    }, [myselfId]);

    // useEffect(() => {
    //     if (!myFriends) {
    //         return;
    //     }

    //     const getUserPostCount = async () => {
    //         try {
    //             const appUserIds: string[] = myFriends
    //                 ? myFriends.map((friend: FriendModel) => friend.whoFriendId === myselfId
    //                     ? friend.forWhomId
    //                     : friend.whoFriendId)
    //                 : [];
    //             appUserIds.push(myselfId);

    //             appUserIdsRef.current = appUserIds.join(',');

    //             const count = await getUserPostCountByUserId(appUserIdsRef.current).unwrap();
    //             setCount(count);
    //         } catch (e) {
    //             logger.error("Failed to receive user post count", e);
    //         }
    //     }

    //     getUserPostCount();
    // }, [myFriends]);

    // useEffect(() => {
    //     if (!myCommunitiesUsers) {
    //         return;
    //     }

    //     const getCommunityPostByListOfCommunityIds = async () => {
    //         try {
    //             const collectionOfCommunityId: number[] = myCommunitiesUsers
    //                 ? myCommunitiesUsers.map((user: CommunityUserModel) => user.communityId)
    //                 : [];

    //             if (collectionOfCommunityId.length === 0) {
    //                 return;
    //             }

    //             communityIdsRef.current = collectionOfCommunityId.join(',');

    //             const count = await getCommunityPostsCount(communityIdsRef.current).unwrap();
    //             setCommunityCount(count);
    //         } catch (e) {
    //             logger.error("Failed to receive community post count", e);
    //         }
    //     }

    //     getCommunityPostByListOfCommunityIds();
    // }, [myCommunitiesUsers]);

    // const getMoreUserPostsAsync = async (currentPostsSize: number): Promise<UserPostModel[]> => {
    //     try {
    //         const arg = {
    //             collectionUserId: appUserIdsRef.current,
    //             offset: currentPostsSize,
    //             pageSize: pageSizeRef.current
    //         };

    //         const userPosts = await getMoreUsersPosts(arg).unwrap();
    //         return userPosts;
    //     } catch (e) {
    //         logger.error("Failed to receive more user posts", e);

    //         return [];
    //     }
    // }

    // const getMoreCommunityPostsAsync = async (currentPostsSize: number): Promise<CommunityPostModel[]> => {
    //     try {
    //         const arg = {
    //             collectionCommunityId: communityIdsRef.current,
    //             offset: currentPostsSize,
    //             pageSize: pageSizeRef.current
    //         };

    //         const communityPosts = await getMoreCommunityPosts(arg).unwrap();
    //         return communityPosts;
    //     } catch (e) {
    //         logger.error("Failed to receive more user posts", e);

    //         return [];
    //     }
    // }

    return { userPosts, communityPosts, count, communityCount, currentDateRef };
}

export default useFetchPosts;