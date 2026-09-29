import type { CharacterProfessionModel } from './CharacterProfessionModel';

export type CharacterProfessionsResponse = {
    primaries: CharacterProfessionModel[];
    secondaries: CharacterProfessionModel[];
}