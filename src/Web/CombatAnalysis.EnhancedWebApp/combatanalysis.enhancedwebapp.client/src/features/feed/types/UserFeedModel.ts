import type { PostModel } from "./PostModel";

export interface UserFeedModel extends PostModel {
    publicType: number;
    tags: string;
    communityName?: string;
    postType?: number;
    restrictions?: number;
    communityId?: number;
}