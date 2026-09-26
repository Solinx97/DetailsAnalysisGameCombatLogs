import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type AchievementCategoryModel = WoWGameDataEntityModel & {
    quantity: number;
    points: number;
}