import type { AchievementSelectedCategoryFactionModel } from './AchievementSelectedCategoryFactionModel';
import type { AchievementCategoryModel } from './AchievementCategoryModel';
import type { AchievementExtendModel } from './AchievementExtendModel';

export type AchievementSelectedCategoryModel = {
    achievements: AchievementExtendModel[];
    subcategories: AchievementCategoryModel[];
    isGuildCategory: boolean;
    aggregatesByFaction: AchievementSelectedCategoryFactionModel;
    displayOrder: number;
}