import type { PostModel } from './PostModel';

export interface CommunityPostModel extends PostModel {
    communityName: string;
    postType: number;
    publicType: number;
    restrictions: number;
    tags: string;
    communityId: number;
}