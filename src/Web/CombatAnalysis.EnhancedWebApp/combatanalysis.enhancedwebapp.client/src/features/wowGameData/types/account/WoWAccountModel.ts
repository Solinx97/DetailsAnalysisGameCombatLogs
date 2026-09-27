import type { CharacterModel } from './CharacterModel';

export type WoWAccountModel = {
    id: number;
    characters: Map<string, CharacterModel[]>;
}