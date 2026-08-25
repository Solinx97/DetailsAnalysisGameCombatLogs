export type CommunityDiscussionModel = {
    id: number;
    title: string;
    content: string;
    createdAt: Date;
    appUserId: string;
    communityId: number;
}