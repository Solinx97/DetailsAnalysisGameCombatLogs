import type { RealmModel } from '../RealmModel';
import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type CharacterModel = WoWGameDataEntityModel & {
    realm: RealmModel;
    playableClass: WoWGameDataEntityModel;
    playableRace: WoWGameDataEntityModel;
    gender: WoWGameDataTypeModel;
    faction: WoWGameDataTypeModel;
    level: number;
}