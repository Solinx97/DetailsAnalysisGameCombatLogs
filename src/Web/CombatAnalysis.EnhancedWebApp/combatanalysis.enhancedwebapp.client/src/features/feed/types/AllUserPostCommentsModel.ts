import type { UserPostCommentModel } from './UserPostCommentModel';

export interface AllUserPostCommentsModel {
    comments: UserPostCommentModel[];
    count: number;
}