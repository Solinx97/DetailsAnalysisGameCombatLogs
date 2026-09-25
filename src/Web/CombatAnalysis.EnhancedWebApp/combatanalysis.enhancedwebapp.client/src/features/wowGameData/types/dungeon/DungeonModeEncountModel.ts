import type { DungeonModel } from './DungeonModel';

export type DungeonModeEncountModel = {
    encounter: DungeonModel;
    completedCount: number;
    lastKillTime: string;
}