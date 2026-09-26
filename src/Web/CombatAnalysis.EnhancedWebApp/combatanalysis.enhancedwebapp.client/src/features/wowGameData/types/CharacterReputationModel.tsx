import type { CharacterReputationStandingModel } from './CharacterReputationStandingModel';
import type { WoWGameDataEntityModel } from './WoWGameDataEntityModel';

export type CharacterReputationModel = {
    faction: WoWGameDataEntityModel;
    standing: CharacterReputationStandingModel;
}