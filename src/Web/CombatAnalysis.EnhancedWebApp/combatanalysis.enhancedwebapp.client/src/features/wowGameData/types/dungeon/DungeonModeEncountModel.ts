import type { DungeonNameModel } from './DungeonNameModel';

export type DungeonModeEncountModel = {
    encounter: DungeonNameModel;
    completedCount: number;
    lastKillTime: string;
}