import type { PostReactionModel } from './PostReactionModel';

export interface CommunityPostReactionModel extends PostReactionModel {
    communityId: number;
    communityPostId: number;
}