export type PostModel = {
    id: number;
    owner: string;
    content: string;
    createdAt: Date;
    appUserId: string;
    likeCount: number;
    dislikeCount: number;
    commentCount: number;
    reaction: number;
}