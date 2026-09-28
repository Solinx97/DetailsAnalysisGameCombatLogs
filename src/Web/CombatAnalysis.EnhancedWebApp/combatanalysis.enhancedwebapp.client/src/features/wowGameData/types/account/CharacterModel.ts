import type { WoWRealmModel } from '../WoWRealmModel';
import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type CharacterModel = WoWGameDataEntityModel & {
    realm: WoWRealmModel;
    playableClass: WoWGameDataEntityModel;
    playableRace: WoWGameDataEntityModel;
    gender: WoWGameDataTypeModel;
    faction: WoWGameDataTypeModel;
    level: number;
}