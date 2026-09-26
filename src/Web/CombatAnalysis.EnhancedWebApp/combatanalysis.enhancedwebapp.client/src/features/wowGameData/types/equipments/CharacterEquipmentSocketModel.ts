import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type CharacterEquipmentSocketModel = {
    type: WoWGameDataTypeModel;
    item: WoWGameDataEntityModel;
    displayString: string;
}