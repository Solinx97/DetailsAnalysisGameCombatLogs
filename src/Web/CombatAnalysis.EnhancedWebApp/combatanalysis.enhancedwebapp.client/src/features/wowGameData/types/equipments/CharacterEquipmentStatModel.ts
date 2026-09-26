import type { WoWGameDataItemDisplayModel } from '../WoWGameDataItemDisplayModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type CharacterEquipmentStatModel = {
    type?: WoWGameDataTypeModel;
    value: number;
    isNegated?: boolean;
    display: WoWGameDataItemDisplayModel;
}