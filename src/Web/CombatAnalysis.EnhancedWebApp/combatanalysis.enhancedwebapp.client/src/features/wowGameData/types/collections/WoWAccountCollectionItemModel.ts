import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWAccountCollectionInfoModel } from './WoWAccountCollectionInfoModel';

export type WoWAccountCollectionItemModel = {
    item: WoWGameDataEntityModel;
    info: WoWAccountCollectionInfoModel;
}