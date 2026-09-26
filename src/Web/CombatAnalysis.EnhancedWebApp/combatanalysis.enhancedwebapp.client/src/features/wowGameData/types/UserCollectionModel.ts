import type { WoWGameDataEntityModel } from './WoWGameDataEntityModel';

export type UserCollectionModel = WoWGameDataEntityModel & {
    isReceived: boolean;
}