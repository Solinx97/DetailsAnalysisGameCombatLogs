import type { WoWGameDataEntityModel } from './WoWGameDataEntityModel';

export type SearchItemDataModel = WoWGameDataEntityModel & {
    level: number;
    requiredLevel: number;
    sellPrice: number;
    isEquippable: boolean;
    purchaseQuantity: number;
    maxCount: number;
    isStackable: boolean;
    name: string;
    purchasePrice: number;
}