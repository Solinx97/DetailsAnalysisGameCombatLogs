import type { CommunityPostCommentModel } from './CommunityPostCommentModel';

export type AllCommunityPostCommentsModel = {
    comments: CommunityPostCommentModel[];
    count: number;
}