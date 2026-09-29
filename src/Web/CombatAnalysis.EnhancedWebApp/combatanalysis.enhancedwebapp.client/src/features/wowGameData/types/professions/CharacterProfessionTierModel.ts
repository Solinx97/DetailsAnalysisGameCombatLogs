import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type CharacterProfessionTierModel = {
    skillPoints: number;
    maxSkillPoints: number;
    tier: WoWGameDataEntityModel;
    knownRecipes: WoWGameDataEntityModel[];
}