import { useEffect, useRef, useState } from 'react';
import { useCommunityUserSearchByUserIdQuery } from '../store/api/community/CommunityUser.api';
import {
    useGetCommunityPostByListOfCommunityIdsQuery,
    useGetNewCommunityPostByListOfCommunityIdsQuery,
    useGetNewUserPostByListOfUserIdsQuery,
    useGetUserPostByListOfUserIdsQuery,
    useLazyGetMoreCommunityPostByListOfCommunityIdsQuery,
    useLazyGetMoreUserPostByListOfUserIdsQuery
} from '../store/api/core/Post.api';
import { useLazyGetCommunityPostCountByListOfCommunityIdQuery } from '../store/api/post/CommunityPost.api';
import { useLazyGetUserPostCountByUserIdQuery } from '../store/api/post/UserPost.api';
import { useFriendSearchMyFriendsQuery } from '../store/api/user/Friend.api';
import { CommunityPost } from '../types/CommunityPost';
import { CommunityUser } from '../types/CommunityUser';
import { Friend } from '../types/Friend';
import { UserPost } from '../types/UserPost';
import { UseFetchUsersPostsResult } from '../types/hooks/UseFetchUsersPostsResult';

const getUserPostsInterval = 5000;

const useFetchUsersPosts = (meId: string): UseFetchUsersPostsResult => {
    const pageSizeRef = useRef<number>(parseInt(process.env.REACT_APP_COMMUNITY_POST_PAGE_SIZE || '5'));
    const currentDateRef = useRef<string>((new Date()).toISOString());
    const appUserIdsRef = useRef<string>(meId);
    const communityIdsRef = useRef<string>("0");

    const [count, setCount] = useState(0);
    const [communityCount, setCommunityCount] = useState(0);

    const { data: myFriends, isLoading: friendsAreLoading } = useFriendSearchMyFriendsQuery(meId, {
        selectFromResult: ({ data }: { data: Friend[] }) => ({ data }),
    });
    const { data: myCommunitiesUsers, isLoading: communitiesAreLoading } = useCommunityUserSearchByUserIdQuery(meId, {
        selectFromResult: ({ data }: { data: CommunityUser[] }) => ({ data }),
    });
    const { data: posts } = useGetUserPostByListOfUserIdsQuery({ appUserIds: appUserIdsRef.current, pageSize: pageSizeRef.current }, {
        selectFromResult: ({ data }: { data: UserPost[] }) => ({ data }),
    });
    const { data: newPosts } = useGetNewUserPostByListOfUserIdsQuery({ appUserIds: appUserIdsRef.current, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        selectFromResult: ({ data }: { data: UserPost[] }) => ({ data }),
    });
    const { data: communityPosts } = useGetCommunityPostByListOfCommunityIdsQuery({ communityIds: communityIdsRef.current, pageSize: pageSizeRef.current }, {
        selectFromResult: ({ data }: { data: CommunityPost[] }) => ({ data }),
    });
    const { data: newCommunityPosts } = useGetNewCommunityPostByListOfCommunityIdsQuery({ communityIds: communityIdsRef.current, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        selectFromResult: ({ data }: { data: CommunityPost[] }) => ({ data }),
    });

    const [getUserPostCountByUserId] = useLazyGetUserPostCountByUserIdQuery();
    const [getCommunityPostsCountAsync] = useLazyGetCommunityPostCountByListOfCommunityIdQuery();
    const [getMoreUsersPosts] = useLazyGetMoreUserPostByListOfUserIdsQuery();
    const [getMoreCommunityPosts] = useLazyGetMoreCommunityPostByListOfCommunityIdsQuery();

    useEffect(() => {
        const getUserPostCount = async () => {
            appUserIdsRef.current = meId;

            let response = await getUserPostCountByUserId(meId);
            if (!response.error) {
                setCount(response.data);
            }
        }

        getUserPostCount();
    }, [meId]);

    useEffect(() => {
        if (!myFriends) {
            return;
        }

        const getUserPostCount = async () => {
            const appUserIds: string[] = myFriends
                ? myFriends.map((friend: Friend) => friend.whoFriendId === meId
                    ? friend.forWhomId
                    : friend.whoFriendId)
                : [];
            appUserIds.push(meId);

            appUserIdsRef.current = appUserIds.join(',');

            let response = await getUserPostCountByUserId(appUserIdsRef.current);
            if (!response.error) {
                setCount(response.data);
            }
        }

        getUserPostCount();
    }, [myFriends]);

    useEffect(() => {
        if (!myCommunitiesUsers) {
            return;
        }

        const getCommunityPostByListOfCommunityIds = async () => {
            const communityIds: string[] = myCommunitiesUsers
                ? myCommunitiesUsers.map((user: CommunityUser) => user.communityId)
                : [];

            if (communityIds.length === 0) {
                return;
            }

            communityIdsRef.current = communityIds.join(',');

            let response = await getCommunityPostsCountAsync(communityIdsRef.current);
            if (!response.error) {
                setCommunityCount(response.data);
            }
        }

        getCommunityPostByListOfCommunityIds();
    }, [myCommunitiesUsers]);

    const getMoreUserPostsAsync = async (currentPostsSize: number): Promise<UserPost[]> => {
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

    const getMoreCommunityPostsAsync = async (currentPostsSize: number): Promise<CommunityPost[]> => {
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