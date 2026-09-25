import type { CharacterAchievementCriteriaModel } from './CharacterAchievementCriteriaModel';
import type { AchievementModel } from './AchievementModel';

export type CharacterAchievementModel = {
    achievement: AchievementModel;
    criteria: CharacterAchievementCriteriaModel;
    completedTime: string;
}