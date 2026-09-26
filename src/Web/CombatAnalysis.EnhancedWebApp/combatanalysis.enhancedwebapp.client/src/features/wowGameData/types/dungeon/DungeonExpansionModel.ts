import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { DungeonInstanceModel } from './DungeonInstanceModel';

export type DungeonExpansionModel = {
    expansion: WoWGameDataEntityModel;
    instances: DungeonInstanceModel[];
}