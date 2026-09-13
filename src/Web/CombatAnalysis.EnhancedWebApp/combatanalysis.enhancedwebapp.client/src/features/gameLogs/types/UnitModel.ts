import type { UnitHealthModel } from './UnitHealthModel';
import type { UnitCastModel } from './UnitCastModel';
import type { UnitPositionModel } from './UnitPositionModel';

export type UnitModel = {
    id: string;
    gameId: string;
    name: string;
    unitHash: string;
    type: number;
    creatorGameId: string | null;
    unitHealthes: UnitHealthModel[];
    unitCasts: UnitCastModel[];
    unitPositions: UnitPositionModel[];
    combatId: number;
}