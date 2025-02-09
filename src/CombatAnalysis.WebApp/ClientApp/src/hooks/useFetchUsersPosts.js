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

const getUserPostsInterval = 5000;

const useFetchUsersPosts = (meId) => {
    const pageSizeRef = useRef(process.env.REACT_APP_COMMUNITY_POST_PAGE_SIZE);
    const currentDateRef = useRef((new Date()).toISOString());
    const appUserIdsRef = useRef(meId);
    const communityIdsRef = useRef("0");

    const [count, setCount] = useState(0);
    const [communityCount, setCommunityCount] = useState(0);

    const { data: myFriends, isLoading: friendsAreLoading } = useFriendSearchMyFriendsQuery(meId);
    const { data: myCommunitiesUsers, isLoading: communitiesAreLoading } = useCommunityUserSearchByUserIdQuery(meId);
    const { data: posts } = useGetUserPostByListOfUserIdsQuery({ appUserIds: appUserIdsRef.current, pageSize: pageSizeRef.current }, {
        selectFromResult: ({ data }) => ({ data }),
    });
    const { data: newPosts } = useGetNewUserPostByListOfUserIdsQuery({ appUserIds: appUserIdsRef.current, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        selectFromResult: ({ data }) => ({ data }),
    });
    const { data: communityPosts } = useGetCommunityPostByListOfCommunityIdsQuery({ communityIds: communityIdsRef.current, pageSize: pageSizeRef.current }, {
        selectFromResult: ({ data }) => ({ data }),
    });
    const { data: newCommunityPosts } = useGetNewCommunityPostByListOfCommunityIdsQuery({ communityIds: communityIdsRef.current, checkFrom: currentDateRef.current }, {
        pollingInterval: getUserPostsInterval,
        selectFromResult: ({ data }) => ({ data }),
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
            const appUserIds = myFriends
                ? myFriends.map(friend => friend.whoFriendId === meId
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
            const communityIds = myCommunitiesUsers
                ? myCommunitiesUsers.map(user => user.communityId)
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

    const getMoreUserPostsAsync = async (currentPostsSize) => {
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

    const getMoreCommunityPostsAsync = async (currentPostsSize) => {
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