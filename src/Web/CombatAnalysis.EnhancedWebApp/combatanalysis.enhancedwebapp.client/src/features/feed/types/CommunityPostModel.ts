import type { PostModel } from './PostModel';

export type CommunityPostModel = PostModel & {
    communityName?: string;
    postType: number;
    restrictions: number;
    communityId: number;
}