import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type DungeonModeEncountModel = {
    encounter: WoWGameDataEntityModel;
    completedCount: number;
    lastKillTime: string;
}