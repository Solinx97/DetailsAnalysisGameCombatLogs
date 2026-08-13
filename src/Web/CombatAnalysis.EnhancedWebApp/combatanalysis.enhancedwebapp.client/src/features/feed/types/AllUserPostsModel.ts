import type { UserPostModel } from './UserPostModel';

export interface AllUserPostsModel {
    posts: UserPostModel[];
    count: number;
}