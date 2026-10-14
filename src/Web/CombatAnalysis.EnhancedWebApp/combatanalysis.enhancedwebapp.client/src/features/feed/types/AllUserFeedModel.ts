import type { UserFeedModel } from './UserFeedModel';

export type AllUserFeedModel = {
    posts: UserFeedModel[];
    count: number;
}