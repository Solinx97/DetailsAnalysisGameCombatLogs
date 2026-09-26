import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type SelectedMountModel = WoWGameDataEntityModel & {
    description: string;
    source: WoWGameDataTypeModel;
    faction?: WoWGameDataTypeModel;
}