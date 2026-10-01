import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { CharacterAchievementStatisticModel } from './CharacterAchievementStatisticModel';
import type { CharacterAchievementStatisticsSubCategoryModel } from './CharacterAchievementStatisticsSubCategoryModel';

export type CharacterAchievementStatisticsCategoryModel = WoWGameDataEntityModel & {
    subCategories: CharacterAchievementStatisticsSubCategoryModel[];
    statistics: CharacterAchievementStatisticModel[];
}