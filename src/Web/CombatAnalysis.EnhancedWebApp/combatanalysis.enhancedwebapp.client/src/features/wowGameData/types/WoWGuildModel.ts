import type { WoWFactionModel } from './WoWFactionModel';
import type { WoWRealmModel } from './WoWRealmModel';

export type WoWGuildModel = {
    name: string;
    realm: WoWRealmModel;
    faction: WoWFactionModel;
}