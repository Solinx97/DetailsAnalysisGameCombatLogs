import type { DungeonInstanceModel } from './DungeonInstanceModel';
import type { DungeonNameModel } from './DungeonNameModel'

export type DungeonExpansionModel = {
    expansion: DungeonNameModel;
    instances: DungeonInstanceModel[];
}