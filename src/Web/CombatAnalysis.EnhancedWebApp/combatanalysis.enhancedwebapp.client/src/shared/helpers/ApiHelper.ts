import type { CommunityPostReactionModel } from '@/features/feed/types/CommunityPostReactionModel';
import type { UserFeedModel } from '@/features/feed/types/UserFeedModel';
import type { UserPostModel } from '@/features/feed/types/UserPostModel';
import type { UserPostReactionModel } from '@/features/feed/types/UserPostReactionModel';
import { Reaction, ReactionType } from './EnumHelper';

export const checkStatus = (createdReaction: UserPostReactionModel | CommunityPostReactionModel, post: UserPostModel | UserFeedModel) => {
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