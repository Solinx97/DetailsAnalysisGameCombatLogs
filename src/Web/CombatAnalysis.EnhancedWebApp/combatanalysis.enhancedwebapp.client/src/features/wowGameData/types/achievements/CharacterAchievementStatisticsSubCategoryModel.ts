import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { CharacterAchievementStatisticModel } from './CharacterAchievementStatisticModel';

export type CharacterAchievementStatisticsSubCategoryModel = WoWGameDataEntityModel & {
    statistics: CharacterAchievementStatisticModel[];
}