import type { MythicKeystoneDungeonLeaderboardModel } from './MythicKeystoneDungeonLeaderboardModel';

export type MythicKeystoneLeaderboardModel = {
    currentLeaderboards: Map<string, MythicKeystoneDungeonLeaderboardModel>;
}