import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { DungeonModeModel } from './DungeonModeModel';

export type DungeonInstanceModel = {
    instance: WoWGameDataEntityModel;
    modes: DungeonModeModel[];
}