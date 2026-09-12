import type { UnitCastModel } from './UnitCastModel';
import type { UnitPositionModel } from './UnitPositionModel';

export type CombatUnitModel = {
    id: string;
    gameId: string;
    name: string;
    health: number;
    unitHash: string;
    type: number;
    creatorGameId: string | null;
    unitCasts: UnitCastModel[];
    unitPositions: UnitPositionModel[];
    combatId: number;
}