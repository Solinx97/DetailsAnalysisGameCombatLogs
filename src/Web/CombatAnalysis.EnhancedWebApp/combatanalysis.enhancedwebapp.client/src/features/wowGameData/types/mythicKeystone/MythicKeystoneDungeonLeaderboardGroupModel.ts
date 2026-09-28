import type { MythicKeystoneDungeonLeaderboardGroupMemberModel } from './MythicKeystoneDungeonLeaderboardGroupMemberModel';
import type { MythicKeystoneRaitingModel } from './MythicKeystoneRaitingModel';

export type MythicKeystoneDungeonLeaderboardGroupModel = {
    ranking: number;
    duration: number;
    completedTimestamp: number;
    keystoneLevel: number;
    members: MythicKeystoneDungeonLeaderboardGroupMemberModel[];
    mythicRating: MythicKeystoneRaitingModel;
}