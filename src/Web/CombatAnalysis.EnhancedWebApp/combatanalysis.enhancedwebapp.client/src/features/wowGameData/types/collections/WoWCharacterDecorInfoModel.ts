import type { WoWAccountCollectionItemInfoModel } from './WoWAccountCollectionItemInfoModel';

export type WoWCharacterDecorInfoModel = WoWAccountCollectionItemInfoModel & {
    quantity: number;
}