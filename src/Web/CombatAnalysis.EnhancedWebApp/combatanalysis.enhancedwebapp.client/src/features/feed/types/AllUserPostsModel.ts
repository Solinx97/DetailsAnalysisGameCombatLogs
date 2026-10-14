import type { UserPostModel } from './UserPostModel';

export type AllUserPostsModel = {
    posts: UserPostModel[];
    count: number;
}