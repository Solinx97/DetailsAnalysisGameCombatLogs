import type { PostModel } from './PostModel';

export interface CommunityPostModel extends PostModel {
    communityName?: string;
    postType: number;
    restrictions: number;
    communityId: number;
}