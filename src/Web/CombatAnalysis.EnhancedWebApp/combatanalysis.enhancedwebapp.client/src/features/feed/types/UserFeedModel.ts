import type { PostModel } from './PostModel';

export type UserFeedModel = PostModel & {
    communityName?: string;
    postType?: number;
    restrictions?: number;
    communityId?: number;
}