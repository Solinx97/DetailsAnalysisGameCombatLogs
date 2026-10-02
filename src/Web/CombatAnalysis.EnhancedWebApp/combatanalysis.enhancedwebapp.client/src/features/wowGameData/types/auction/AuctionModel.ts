import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type AuctionModel = WoWGameDataEntityModel & {
    item: WoWGameDataEntityModel;
    unitPrice: number;
    quantity: number;
    timeLeft: string;
}