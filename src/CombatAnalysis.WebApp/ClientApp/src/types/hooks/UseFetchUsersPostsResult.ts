import { CommunityPost } from '../CommunityPost';
import { UserPost } from '../UserPost';

export interface UseFetchUsersPostsResult {
    posts: UserPost[] | undefined;
    communityPosts: CommunityPost[] | undefined;
    newPosts: UserPost[] | undefined;
    newCommunityPosts: CommunityPost[] | undefined;
    count: number;
    communityCount: number;
    isLoading: boolean;
    getMoreUserPostsAsync: (currentPostsSize: number) => Promise<UserPost[]>;
    getMoreCommunityPostsAsync: (currentPostsSize: number) => Promise<CommunityPost[]>;
    currentDateRef: React.MutableRefObject<string>;
}