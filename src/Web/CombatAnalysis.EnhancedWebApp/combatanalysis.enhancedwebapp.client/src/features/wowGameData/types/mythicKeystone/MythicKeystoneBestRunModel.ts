import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { MythicKeystoneMemberModel } from './MythicKeystoneMemberModel';
import type { MythicKeystoneRaitingModel } from './MythicKeystoneRaitingModel';

export type MythicKeystoneBestRunModel = {
    completedTimestamp: number;
    duration: number;
    level: number;
    afixes: WoWGameDataEntityModel[];
    members: MythicKeystoneMemberModel[];
    dungeon: WoWGameDataEntityModel;
    isCompletedWithinTime: boolean;
    mythicRating: MythicKeystoneRaitingModel;
    mapRating: MythicKeystoneRaitingModel;
}