import type { AchievementModel } from './AchievementModel';
import type { SelectedAchievementCriteriaModel } from './SelectedAchievementCriteriaModel';

export type SelectedAchievementModel = {
    category: AchievementModel;
    name: string;
    description: string;
    points: number;
    isAccountWide: boolean;
    criteria: SelectedAchievementCriteriaModel;
    nextAchievement: AchievementModel;
    displayOrder: number;
}