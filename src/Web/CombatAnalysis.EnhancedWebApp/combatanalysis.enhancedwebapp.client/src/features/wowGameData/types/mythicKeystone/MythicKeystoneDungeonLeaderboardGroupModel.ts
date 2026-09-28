import type { MythicKeystoneDungeonLeaderboardGroupMemberModel } from './MythicKeystoneDungeonLeaderboardGroupMemberModel';
import type { MythicKeystoneRaitingModel } from './MythicKeystoneRaitingModel';

export type MythicKeystoneDungeonLeaderboardGroupModel = {
    ranking: number;
    duration: string;
    completedTime: string;
    keystoneLevel: number;
    members: MythicKeystoneDungeonLeaderboardGroupMemberModel[];
    mythicRating: MythicKeystoneRaitingModel;
}