import type { DungeonModeModel } from './DungeonModeModel';
import type { DungeonNameModel } from './DungeonNameModel';

export type DungeonInstanceModel = {
    instance: DungeonNameModel;
    modes: DungeonModeModel[];
}