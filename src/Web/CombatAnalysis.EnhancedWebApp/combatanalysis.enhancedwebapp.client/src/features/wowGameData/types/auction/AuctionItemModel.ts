import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { AuctionItemTypeModel } from './AuctionItemTypeModel';

export type AuctionItemModel = WoWGameDataEntityModel & {
    context: number;
    modifiers: AuctionItemTypeModel[];
}