import type { WoWFactionModel } from '../WoWFactionModel';
import type { WoWCharacterGenderModel } from './WoWCharacterGenderModel';
import type { WoWRealmModel } from '../WoWRealmModel';
import type { WoWGuildModel } from '../WoWGuildModel';
import type { WoWCharacterTitleModel } from './WoWCharacterTitleModel';
import type { WoWCharacterRaceModel } from './WoWCharacterRaceModel';
import type { CharacterSpecializationModel } from './CharacterSpecializationModel';
import type { WoWCharacterClassModel } from './WoWCharacterClassModel';

export type WoWCharacterModel = {
    name: string;
    gender: WoWCharacterGenderModel;
    faction: WoWFactionModel;
    race: WoWCharacterRaceModel;
    class: WoWCharacterClassModel;
    activeSpec: CharacterSpecializationModel;
    realm: WoWRealmModel;
    guild: WoWGuildModel;
    level: number;
    experience: number;
    achievementPoints: number;
    lastLogin: number;
    averageItemLevel: number;
    equippedItemLevel: number;
    activeTitle: WoWCharacterTitleModel;
    isRemix: boolean;
    nameSearch: string;
}