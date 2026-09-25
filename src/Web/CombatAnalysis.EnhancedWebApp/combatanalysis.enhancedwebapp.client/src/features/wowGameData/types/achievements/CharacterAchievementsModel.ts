import type { DungeonCharacterModel } from '../mythicKeystone/DungeonCharacterModel';
import type { CharacterAchievementCategoryModel } from './CharacterAchievementCategoryModel';
import type { CharacterAchievementModel } from './CharacterAchievementModel';
import type { CharacterAchievementRecentEventsModel } from './CharacterAchievementRecentEventsModel';

export type CharacterAchievementsModel = {
    totalQuantity: number;
    totalPoints: number;
    achievements: CharacterAchievementModel[];
    categoryProgress: CharacterAchievementCategoryModel[];
    recentEvents: CharacterAchievementRecentEventsModel[];
    character: DungeonCharacterModel;
}