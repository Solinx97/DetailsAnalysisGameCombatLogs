import type { PostReactionModel } from './PostReactionModel';

export type UserPostReactionModel = PostReactionModel & {
    userPostId: number;
}