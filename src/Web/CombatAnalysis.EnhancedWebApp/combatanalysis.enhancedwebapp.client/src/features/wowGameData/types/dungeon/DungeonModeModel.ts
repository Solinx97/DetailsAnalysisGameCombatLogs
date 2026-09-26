import type { DungeonModeProgressModel } from './DungeonModeProgressModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type DungeonModeModel = {
    difficulty: WoWGameDataTypeModel;
    status: WoWGameDataTypeModel;
    progress: DungeonModeProgressModel;
}