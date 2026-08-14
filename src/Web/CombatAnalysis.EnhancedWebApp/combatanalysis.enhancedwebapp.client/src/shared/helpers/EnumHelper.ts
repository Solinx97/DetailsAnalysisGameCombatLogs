export const ReactionType = {
    None: 1,
    AddLike: 2,
    RemoveLike: 3,
    AddDislike: 4,
    RemoveDislike: 5,
    Like: 6,
    Dislike: 7
} as const;

export const Reaction = {
    None: 0,
    Like: 1,
    Dislike: 2
} as const;