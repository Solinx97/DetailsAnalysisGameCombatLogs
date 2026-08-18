export type PostModel = {
    id: number;
    content: string;
    tags: string;
    createdAt: Date;
    publicType: number;
    appUserId: string;
    likeCount: number;
    dislikeCount: number;
    commentCount: number;
    reaction: number;
}