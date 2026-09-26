import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type AchievementExtendModel = WoWGameDataEntityModel & {
    completedTime?: string;
}