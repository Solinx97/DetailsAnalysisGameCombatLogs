import type { CommentModel } from './CommentModel';

export interface CommunityPostCommentModel extends CommentModel {
    commentType: number;
    communityPostId: number;
    communityId: number;
}