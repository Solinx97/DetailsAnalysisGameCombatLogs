import type { DungeonCharacterModel } from "../mythicKeystone/DungeonCharacterModel";
import type { DungeonExpansionModel } from "./DungeonExpansionModel";

export type CharacterDungeonModel = {
    character: DungeonCharacterModel;
    expansions: DungeonExpansionModel[];
}