import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { SelectedAchievementCriteriaModel } from './SelectedAchievementCriteriaModel';

export type SelectedAchievementModel = {
    category: WoWGameDataEntityModel;
    name: string;
    description: string;
    points: number;
    isAccountWide: boolean;
    criteria: SelectedAchievementCriteriaModel;
    nextAchievement: WoWGameDataEntityModel;
    displayOrder: number;
}