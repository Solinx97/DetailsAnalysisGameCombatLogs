import type { WoWGameDataEntityModel } from './WoWGameDataEntityModel';

export type RealmModel = WoWGameDataEntityModel & {
    slug: string;
}