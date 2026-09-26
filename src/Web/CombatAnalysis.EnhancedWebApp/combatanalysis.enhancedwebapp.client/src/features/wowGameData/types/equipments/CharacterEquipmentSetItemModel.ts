import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type CharacterEquipmentSetItemModel = {
    item: WoWGameDataEntityModel;
    isEquipped: boolean;
}