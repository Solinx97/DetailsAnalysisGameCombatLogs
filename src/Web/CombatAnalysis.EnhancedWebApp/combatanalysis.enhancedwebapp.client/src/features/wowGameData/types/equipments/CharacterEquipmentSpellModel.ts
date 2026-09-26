import type { WoWGameDataColorModel } from '../WoWGameDataColorModel';
import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type CharacterEquipmentSpellModel = {
    spell: WoWGameDataEntityModel;
    description: string;
    color: WoWGameDataColorModel;
}