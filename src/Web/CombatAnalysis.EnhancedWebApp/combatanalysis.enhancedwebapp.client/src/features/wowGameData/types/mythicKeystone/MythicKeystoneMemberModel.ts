import type { CharacterSpecializationModel } from '../character/CharacterSpecializationModel';
import type { WoWCharacterRaceModel } from '../character/WoWCharacterRaceModel';
import type { DungeonCharacterModel } from './DungeonCharacterModel';

export type MythicKeystoneMemberModel = {
    character: DungeonCharacterModel;
    specialization: CharacterSpecializationModel;
    race: WoWCharacterRaceModel;
    equippedItemLevel: number;
}