import type { DungeonModeModel } from './DungeonModeModel';
import type { DungeonModel } from './DungeonModel';

export type DungeonInstanceModel = {
    instance: DungeonModel;
    modes: DungeonModeModel[];
}