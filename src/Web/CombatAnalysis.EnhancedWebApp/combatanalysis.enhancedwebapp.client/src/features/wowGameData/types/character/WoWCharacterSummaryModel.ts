import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { WoWRealmModel } from '../WoWRealmModel';
import type { WoWGuildModel } from '../WoWGuildModel';
import type { WoWCharacterTitleModel } from './WoWCharacterTitleModel';
import type { WoWCharacterRaceModel } from './WoWCharacterRaceModel';
import type { CharacterSpecializationModel } from './CharacterSpecializationModel';
import type { WoWCharacterClassModel } from './WoWCharacterClassModel';
import type { WoWGameDataTypeModel } from '../WoWGameDataTypeModel';

export type WoWCharacterSummaryModel = {
    name: string;
    gender: WoWGameDataTypeModel;
    faction: WoWGameDataEntityModel;
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