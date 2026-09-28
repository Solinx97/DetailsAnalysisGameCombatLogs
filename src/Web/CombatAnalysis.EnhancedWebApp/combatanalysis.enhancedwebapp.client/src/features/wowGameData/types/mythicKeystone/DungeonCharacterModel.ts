import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWRealmModel } from '../WoWRealmModel';

export type DungeonCharacterModel = WoWGameDataEntityModel & {
    realm: WoWRealmModel;
}