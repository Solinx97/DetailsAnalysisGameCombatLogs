import type { CharacterEquipmentSlotModel } from "./CharacterEquipmentSlotModel";

export type CharacterEquipmentEnchantmentModel = {
    id: number;
    displayString: string;
    slot: CharacterEquipmentSlotModel;
}