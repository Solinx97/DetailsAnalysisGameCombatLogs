import type { MythicKeystoneAfixModel } from './MythicKeystoneAfixModel';
import type { MythicKeystoneDungeonModel } from './MythicKeystoneDungeonModel';
import type { MythicKeystoneMemberModel } from './MythicKeystoneMemberModel';
import type { MythicKeystoneRaitingModel } from './MythicKeystoneRaitingModel';

export type MythicKeystoneBestRunModel = {
    completedTimestamp: number;
    duration: number;
    level: number;
    afixes: MythicKeystoneAfixModel[];
    members: MythicKeystoneMemberModel[];
    dungeon: MythicKeystoneDungeonModel;
    isCompletedWithinTime: boolean;
    mythicRating: MythicKeystoneRaitingModel;
    mapRating: MythicKeystoneRaitingModel;
}