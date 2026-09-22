import type { CombatPlayerStatsModel } from "./CombatPlayerStatsModel";
import type { PlayerModel } from "./PlayerModel";
import type { SpecializationScoreModel } from "./SpecializationScoreModel";
import type { UnitModel } from './UnitModel';

export type CombatPlayerModel = {
    id: number;
    averageItemLevel: number;
    deathCount: number;
    stats?: CombatPlayerStatsModel;
    score?: SpecializationScoreModel;
    player: PlayerModel;
    unit: UnitModel;
    unitId: string;
}