import type { CommunityPostModel } from './CommunityPostModel';

export type AllCimmunityPostsModel = {
    posts: CommunityPostModel[];
    count: number;
}