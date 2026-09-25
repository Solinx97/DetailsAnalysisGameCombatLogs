import type { AchievementCategoryModel } from './AchievementCategoryModel';

export type AchievementCategoriesModel = {
    categories: AchievementCategoryModel[];
    rootCategories: AchievementCategoryModel[];
    guildCategories: AchievementCategoryModel[];
    totalQuantity: number;
    totalPoints: number
}