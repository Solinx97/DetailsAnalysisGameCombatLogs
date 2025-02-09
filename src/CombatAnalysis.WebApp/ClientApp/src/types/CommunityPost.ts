export interface CommunityPost {
    id: number;
    communityName: string;
    owner: string;
    content: string;
    postType: number;
    publicType: number;
    restrictions: number;
    tags: string;
    createdAt: string;
    likeCount: number;
    dislikeCount: number;
    commentCount: number;
    communityId: number;
    appUserId: string;
}