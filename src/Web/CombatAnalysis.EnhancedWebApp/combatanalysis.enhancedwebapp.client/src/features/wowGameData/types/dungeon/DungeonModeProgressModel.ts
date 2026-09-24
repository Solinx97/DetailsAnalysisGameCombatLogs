import type { DungeonModeEncountModel } from './DungeonModeEncountModel';

export type DungeonModeProgressModel = {
    completedCount: number;
    totalCount: number;
    encounters: DungeonModeEncountModel[];
}