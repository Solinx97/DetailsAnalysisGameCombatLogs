import type { WoWGameDataPlayableClassModel } from '../WoWGameDataPlayableClassModel';
import type { WoWGameDataValueModel } from '../WoWGameDataValueModel';

export type CharacterEquipmentRequirementsModel = {
    level: WoWGameDataValueModel;
    playableClasses: WoWGameDataPlayableClassModel;
}