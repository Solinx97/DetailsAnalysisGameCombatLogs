import type { UnitHealthModel } from './UnitHealthModel';
import type { UnitCastModel } from './UnitCastModel';
import type { UnitPositionModel } from './UnitPositionModel';
import type { UnitInfoModel } from './UnitInfoModel';

export type UnitModel = {
    id: string;
    gameId: string;
    name: string;
    unitHash: string;
    type: number;
    creatorGameId: string | null;
    unitInfo: UnitInfoModel;
    unitHealthes: UnitHealthModel[];
    unitCasts: UnitCastModel[];
    unitPositions: UnitPositionModel[];
    combatId: number;
}