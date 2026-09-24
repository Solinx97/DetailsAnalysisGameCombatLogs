import type { CharacterReputationFactionModel } from './CharacterReputationFactionModel';
import type { CharacterReputationStandingModel } from './CharacterReputationStandingModel';

export type CharacterReputationModel = {
    faction: CharacterReputationFactionModel;
    standing: CharacterReputationStandingModel;
}