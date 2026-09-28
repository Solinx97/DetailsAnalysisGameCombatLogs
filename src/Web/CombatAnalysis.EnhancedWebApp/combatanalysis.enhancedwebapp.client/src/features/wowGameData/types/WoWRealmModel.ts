import type { WoWGameDataEntityModel } from './WoWGameDataEntityModel';

export type WoWRealmModel = WoWGameDataEntityModel & {
    slug: string;
}