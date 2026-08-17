export type CommunityDiscussionCommentModel = {
    id: number;
    content: string;
    createdAt: Date;
    appUserId: string;
    communityDiscussionId: number;
}