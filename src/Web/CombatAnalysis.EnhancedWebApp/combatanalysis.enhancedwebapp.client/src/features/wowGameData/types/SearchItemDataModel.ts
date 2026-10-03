import type { WoWGameDataEntityModel } from './WoWGameDataEntityModel';
import type { WoWGameDataTypeModel } from './WoWGameDataTypeModel';

export type SearchItemDataModel = WoWGameDataEntityModel & {
    level: number;
    requiredLevel: number;
    sellPrice: number;
    itemClass: WoWGameDataEntityModel;
    itemSubclass: WoWGameDataEntityModel;
    quality: WoWGameDataTypeModel;
    isEquippable: boolean;
    purchaseQuantity: number;
    maxCount: number;
    isStackable: boolean;
    name: string;
    purchasePrice: number;
}