import type { CharacterEquipmentModel } from './CharacterEquipmentModel';
import type { CharacterEquipmentSetModel } from './CharacterEquipmentSetModel';

export type CharacterEquipmentsResponse = {
    equippedItems: CharacterEquipmentModel[];
    equippedItemSets: CharacterEquipmentSetModel[];
}