import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';
import type { WoWAccountCollectionItemInfoModel } from './WoWAccountCollectionItemInfoModel';
import type { WoWAccountPetStatModel } from './WoWAccountPetStatModel';

export type WoWAccountPetInfoModel = WoWAccountCollectionItemInfoModel & {
    id: number;
    level: number;
    quality: WoWGameDataTypeModel;
    stats: WoWAccountPetStatModel;
}