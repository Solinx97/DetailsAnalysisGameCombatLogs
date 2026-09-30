import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type SelectedWoWAccountToyItemModel = {
    id: number;
    item: WoWGameDataEntityModel;
    description: string;
    source: WoWGameDataTypeModel;
}