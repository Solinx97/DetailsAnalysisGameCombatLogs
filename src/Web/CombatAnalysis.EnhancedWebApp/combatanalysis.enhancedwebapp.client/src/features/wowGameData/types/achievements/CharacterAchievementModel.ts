import type { CharacterAchievementCriteriaModel } from './CharacterAchievementCriteriaModel';
import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type CharacterAchievementModel = {
    achievement: WoWGameDataEntityModel;
    criteria: CharacterAchievementCriteriaModel;
    completedTime: string;
}