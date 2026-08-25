import type { CommunityPostCommentModel } from './CommunityPostCommentModel';

export interface AllCommunityPostCommentsModel {
    comments: CommunityPostCommentModel[];
    count: number;
}