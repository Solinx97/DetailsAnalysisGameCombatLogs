export interface UserPost {
    id: number;
    owner: string;
    content: string;
    publicType: number;
    tags: string;
    createdAt: string;
    likeCount: number;
    dislikeCount: number;
    commentCount: number;
    appUserId: string;
}