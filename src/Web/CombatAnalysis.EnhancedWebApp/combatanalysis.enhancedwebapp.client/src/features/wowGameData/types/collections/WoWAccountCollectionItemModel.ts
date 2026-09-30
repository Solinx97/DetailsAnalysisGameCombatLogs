import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWAccountCollectionItemInfoModel } from './WoWAccountCollectionItemInfoModel';
import type { WoWAccountPetInfoModel } from './WoWAccountPetInfoModel';

export type WoWAccountCollectionItemModel = {
    item: WoWGameDataEntityModel;
    info?: WoWAccountCollectionItemInfoModel | WoWAccountPetInfoModel;
}