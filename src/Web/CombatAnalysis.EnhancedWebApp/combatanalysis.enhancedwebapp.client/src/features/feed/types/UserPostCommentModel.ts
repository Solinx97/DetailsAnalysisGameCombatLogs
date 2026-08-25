import type { CommentModel } from './CommentModel';

export interface UserPostCommentModel extends CommentModel {
    userPostId: number;
}