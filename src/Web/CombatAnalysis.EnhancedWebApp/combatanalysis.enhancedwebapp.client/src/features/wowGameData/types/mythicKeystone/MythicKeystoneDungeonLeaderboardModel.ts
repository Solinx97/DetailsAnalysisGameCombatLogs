import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { MythicKeystoneDungeonLeaderboardAfixModel } from './MythicKeystoneDungeonLeaderboardAfixModel';
import type { MythicKeystoneDungeonLeaderboardGroupModel } from './MythicKeystoneDungeonLeaderboardGroupModel';

export type MythicKeystoneDungeonLeaderboardModel = {
    map: WoWGameDataEntityModel;
    period: number;
    periodStartTime: string;
    periodEndTime: string;
    leadingGroups: MythicKeystoneDungeonLeaderboardGroupModel[];
    afixes: MythicKeystoneDungeonLeaderboardAfixModel[];
    mapChallengeModeId: number;
    name: string;
}