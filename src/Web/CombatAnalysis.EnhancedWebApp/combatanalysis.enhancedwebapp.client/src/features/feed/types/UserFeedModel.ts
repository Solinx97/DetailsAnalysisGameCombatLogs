import type { PostModel } from './PostModel';

export interface UserFeedModel extends PostModel {
    publicType: number;
    tags: string;
    communityName?: string;
    likeCount: number;
    dislikeCount: number;
    commentCount: number;
    postType?: number;
    restrictions?: number;
    communityId?: number;
}