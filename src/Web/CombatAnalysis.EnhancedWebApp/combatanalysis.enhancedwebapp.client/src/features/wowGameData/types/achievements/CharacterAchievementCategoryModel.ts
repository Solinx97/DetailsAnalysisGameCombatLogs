import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type CharacterAchievementCategoryModel = {
    category: WoWGameDataEntityModel;
    quantity: number;
    points: number;
}