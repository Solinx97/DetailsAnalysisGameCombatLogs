import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { CharacterEquipmentSetEffectModel } from './CharacterEquipmentSetEffectModel';
import type { CharacterEquipmentSetItemModel } from './CharacterEquipmentSetItemModel';

export type CharacterEquipmentSetModel = {
    itemSet: WoWGameDataEntityModel;
    items: CharacterEquipmentSetItemModel[];
    effects: CharacterEquipmentSetEffectModel[];
    displayString: string;
}