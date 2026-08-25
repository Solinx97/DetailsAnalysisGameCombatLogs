import type { PostModel } from './PostModel';

export interface UserFeedModel extends PostModel {
    communityName?: string;
    postType?: number;
    restrictions?: number;
    communityId?: number;
}