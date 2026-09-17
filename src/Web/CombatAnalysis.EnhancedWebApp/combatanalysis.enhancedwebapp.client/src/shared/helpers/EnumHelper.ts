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

export const DamageModificationType = {
    Normal: 0,
    Crit: 1,
    Crushing: 2,
    Dodge: 3,
    Parry: 4,
    Miss: 5,
    Resist: 6,
    Immune: 7,
    Block: 8,
    Absorb: 9,
} as const;

export const CombatUnitType = {
    PlayerCreature: 0,
    EnemyCreature: 1,
    Vehicle: 2,
    Player: 3,
    Pet: 4,
} as const;

export const UnitHealthStatus = {
    Increase: 0,
    Decrease: 1,
    Dead: 2,
} as const;