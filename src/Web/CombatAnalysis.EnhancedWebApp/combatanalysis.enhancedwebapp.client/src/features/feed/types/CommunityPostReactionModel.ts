import type { PostReactionModel } from './PostReactionModel';

export type CommunityPostReactionModel = PostReactionModel & {
    communityId: number;
    communityPostId: number;
}