import type { DungeonInstanceModel } from './DungeonInstanceModel';
import type { DungeonModel } from './DungeonModel'

export type DungeonExpansionModel = {
    expansion: DungeonModel;
    instances: DungeonInstanceModel[];
}