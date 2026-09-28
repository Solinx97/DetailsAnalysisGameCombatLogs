import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';
import type { DungeonCharacterModel } from './DungeonCharacterModel';

export type MythicKeystoneDungeonLeaderboardGroupMemberModel = {
    character: DungeonCharacterModel;
    faction: WoWGameDataTypeModel;
    specialization: WoWGameDataEntityModel;
}