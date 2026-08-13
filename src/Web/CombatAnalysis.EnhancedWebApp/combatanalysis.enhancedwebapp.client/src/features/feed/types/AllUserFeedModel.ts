import type { UserFeedModel } from './UserFeedModel';

export interface AllUserFeedModel {
    posts: UserFeedModel[];
    count: number;
}