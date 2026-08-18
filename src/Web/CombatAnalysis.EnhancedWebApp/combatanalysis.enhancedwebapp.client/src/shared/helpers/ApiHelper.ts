import type { CommunityPostReactionModel } from '@/features/feed/types/CommunityPostReactionModel';
import type { UserPostReactionModel } from '@/features/feed/types/UserPostReactionModel';
import type { PostModel } from '@/features/feed/types/PostModel';
import { Reaction, ReactionType } from './EnumHelper';

export const updateReactionsStatus = (createdReaction: UserPostReactionModel | CommunityPostReactionModel, post: PostModel) => {
    switch (createdReaction.status) {
        case ReactionType.Like:
            post.likeCount++;
            post.dislikeCount = Math.max(
                0,
                post.dislikeCount - 1
            );
            post.reaction = Reaction.Like;
            break;
        case ReactionType.Dislike:
            post.dislikeCount++;
            post.likeCount = Math.max(
                0,
                post.likeCount - 1
            );
            post.reaction = Reaction.Dislike;
            break;
        case ReactionType.AddLike:
            post.likeCount++;
            post.reaction = Reaction.Like;
            break;
        case ReactionType.RemoveLike:
            post.likeCount--;
            post.reaction = Reaction.None;
            break;
        case ReactionType.AddDislike:
            post.dislikeCount++;
            post.reaction = Reaction.Dislike;
            break;
        case ReactionType.RemoveDislike:
            post.dislikeCount--;
            post.reaction = Reaction.None;
            break;
        default:
            break;
    }
}