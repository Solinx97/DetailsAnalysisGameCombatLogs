import type { CommunityDiscussionCommentModel } from './CommunityDiscussionCommentModel';

export interface AllDiscussionCommentModel {
    comments: CommunityDiscussionCommentModel[];
    count: number;
}