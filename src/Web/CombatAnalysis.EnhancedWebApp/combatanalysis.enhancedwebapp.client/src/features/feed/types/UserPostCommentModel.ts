import type { CommentModel } from './CommentModel';

export type UserPostCommentModel = CommentModel & {
    userPostId: number;
}