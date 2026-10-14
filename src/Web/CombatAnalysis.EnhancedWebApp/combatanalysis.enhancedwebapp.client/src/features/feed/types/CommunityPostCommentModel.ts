import type { CommentModel } from './CommentModel';

export type CommunityPostCommentModel = CommentModel & {
    commentType: number;
    communityPostId: number;
    communityId: number;
}