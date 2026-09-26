import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type CharacterEquipmentTransmogModel = {
    item: WoWGameDataEntityModel;
    displayString: string;
    itemModifiedAppearanceId: number;
}