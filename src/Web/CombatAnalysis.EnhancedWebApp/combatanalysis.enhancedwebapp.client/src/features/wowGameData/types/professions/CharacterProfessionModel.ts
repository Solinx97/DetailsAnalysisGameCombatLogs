import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { CharacterProfessionTierModel } from './CharacterProfessionTierModel';

export type CharacterProfessionModel = {
    profession: WoWGameDataEntityModel;
    tiers: CharacterProfessionTierModel[];
}