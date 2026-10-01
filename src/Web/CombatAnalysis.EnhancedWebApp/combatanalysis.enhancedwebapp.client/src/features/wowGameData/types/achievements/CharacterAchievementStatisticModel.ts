import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type CharacterAchievementStatisticModel = WoWGameDataEntityModel & {
    lastUpdatedTime: string;
    description?: string;
    quantity: number;
}