import type { PostModel } from './PostModel';

export type CommunityPostModel = PostModel & {
    postType: number;
    restrictions: number;
    communityId: number;
    communityName?: string;
}