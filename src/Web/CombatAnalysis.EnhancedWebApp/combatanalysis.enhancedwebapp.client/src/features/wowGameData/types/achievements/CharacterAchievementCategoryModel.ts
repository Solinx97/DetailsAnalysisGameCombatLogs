import type { AchievementModel } from './AchievementModel';

export type CharacterAchievementCategoryModel = {
    category: AchievementModel;
    quantity: number;
    points: number;
}