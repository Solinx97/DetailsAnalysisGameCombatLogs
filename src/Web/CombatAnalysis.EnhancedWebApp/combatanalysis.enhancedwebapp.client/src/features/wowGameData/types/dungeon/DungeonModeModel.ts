import type { DungeonModeProgressModel } from './DungeonModeProgressModel';
import type { DungeonModeTypeModel } from './DungeonModeTypeModel';

export type DungeonModeModel = {
    difficulty: DungeonModeTypeModel;
    status: DungeonModeTypeModel;
    progress: DungeonModeProgressModel;
}