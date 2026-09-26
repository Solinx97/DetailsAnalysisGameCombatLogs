import type { WoWGameDataEntityModel } from './WoWGameDataEntityModel';
import type { WoWRealmModel } from './WoWRealmModel';

export type WoWGuildModel = {
    name: string;
    realm: WoWRealmModel;
    faction: WoWGameDataEntityModel;
}