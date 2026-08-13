import type { CommunityPostModel } from './CommunityPostModel';

export interface AllCimmunityPostsModel {
    posts: CommunityPostModel[];
    count: number;
}