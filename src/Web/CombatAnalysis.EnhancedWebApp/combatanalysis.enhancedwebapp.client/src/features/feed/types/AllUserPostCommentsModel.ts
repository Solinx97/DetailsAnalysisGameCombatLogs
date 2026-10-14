import type { UserPostCommentModel } from './UserPostCommentModel';

export type AllUserPostCommentsModel = {
    comments: UserPostCommentModel[];
    count: number;
}