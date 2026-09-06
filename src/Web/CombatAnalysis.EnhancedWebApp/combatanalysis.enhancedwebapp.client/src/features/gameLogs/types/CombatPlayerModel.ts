import type { CombatPlayerStatsModel } from "./CombatPlayerStatsModel";
import type { PlayerModel } from "./PlayerModel";
import type { SpecializationScoreModel } from "./SpecializationScoreModel";

export type CombatPlayerModel = {
    id: number;
    averageItemLevel: number;
    resourcesRecovery: number;
    damageDone: number;
    healDone: number;
    damageTaken: number;
    stats?: CombatPlayerStatsModel;
    score?: SpecializationScoreModel;
    player: PlayerModel;
    combatId: number;
}