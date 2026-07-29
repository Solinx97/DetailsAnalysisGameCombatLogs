import CombatReplyContext from '@/context/CombatReplyContext';
import { memo, useContext, useEffect, useMemo, useState } from 'react';
import type { CombatUnitModel } from '../../types/CombatUnitModel';
import type { UnitHealthModel } from '../../types/UnitHealthModel';
import CombatReplyItem from './CombatReplyItem';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import {
    useLazyGetCombatUnitsByCombatIdQuery,
    useLazyGetUnitCastsByCombatPlayerIdQuery, useLazyGetUnitsHealthByCombatIdQuery
} from '../../api/GameLogs.api';
import type { UnitPositionModel } from '../../types/UnitPositionModel';
import type { UnitCastModel } from '../../types/UnitCastModel';
import CombatReplyUnitsCategory from './CombatReplyUnitsCategory';

import './CombatReplyUnits.scss';

interface CombatReplyUnitsProps {
    unitPositions: Map<string, UnitPositionModel[]>;
    details: CombatDetailsModel;
}

const CombatReplyUnits: React.FC<CombatReplyUnitsProps> = ({ unitPositions, details }) => {
    const context = useContext(CombatReplyContext);

    if (!context) {
        throw new Error("Child must be inside CombatReplyContext.Provider");
    }

    const { t, selectedGameId, selectedTargetGameId, colors } = context;

    const [combatUnits, setCombatUnits] = useState<CombatUnitModel[]>([]);
    const [unitsCast, setUnitsCast] = useState<Map<string, UnitCastModel[]>>(new Map());
    const [unitsHealth, setUnitsHealth] = useState<Map<string, UnitHealthModel[]>>(new Map());
    const [selectedUnit, setSelectedUnit] = useState<CombatUnitModel | undefined>();
    const [selectedTargetUnit, setSelectedTargetUnit] = useState<CombatUnitModel | undefined>();

    const [getCombatUnits] = useLazyGetCombatUnitsByCombatIdQuery();
    const [getUnitCasts] = useLazyGetUnitCastsByCombatPlayerIdQuery();
    const [getUnitsHealth] = useLazyGetUnitsHealthByCombatIdQuery();

    useEffect(() => {
        if (details.id === 0) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatUnits, unitsCast, unitsHealth] = await Promise.all([
                    getCombatUnits(details.id).unwrap(),
                    getUnitCasts(details.id).unwrap(),
                    getUnitsHealth(details.id).unwrap()
                ]);

                setCombatUnits(combatUnits);

                const unitsCastMap = new Map(Object.entries(unitsCast));
                setUnitsCast(unitsCastMap);

                const unitsHealthMap = new Map(Object.entries(unitsHealth));
                setUnitsHealth(unitsHealthMap);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [details]);

    useEffect(() => {
        if (selectedGameId === "") {
            setSelectedUnit(undefined);
        }
        else {
            const selected = combatUnits.find(x => x.gameId === selectedGameId);
            setSelectedUnit(selected);
        }
    }, [selectedGameId]);

    useEffect(() => {
        if (selectedTargetGameId === "" || selectedGameId === "") {
            setSelectedTargetUnit(undefined);
        }
        else {
            const selected = combatUnits.find(x => x.gameId === selectedTargetGameId);
            setSelectedTargetUnit(selected);
        }
    }, [selectedGameId, selectedTargetGameId]);

    const playerUnits = useMemo(() => {
        return combatUnits.filter(x => x.gameId.startsWith("Player"));
    }, [combatUnits]);

    const playerCreatureUnits = useMemo(() => {
        return combatUnits.filter(x => (x.gameId.startsWith("Creature") || x.gameId.startsWith("Pet"))
            && ((!x.creatorGameId && x.gameId.startsWith("Pet")) || x.creatorGameId && x.creatorGameId.startsWith("Player")));
    }, [combatUnits]);

    const enemyUnits = useMemo(() => {
        return combatUnits.filter(x => !x.gameId.startsWith("Player") && !x.gameId.startsWith("Pet")
            && (!x.creatorGameId || (x.creatorGameId && !x.creatorGameId.startsWith("Player") && x.unitType && x.unitType.startsWith("0x20"))));
    }, [combatUnits]);

    return (
        <ul className="units">
            <li className="units__category">
                <div className="selected-units">
                    {selectedUnit &&
                        <div className="player">
                            <CombatReplyItem
                                unitCasts={unitsCast.get(selectedGameId)}
                                unitsHealth={unitsHealth.get(selectedGameId)}
                                unit={selectedUnit}
                                color={colors.get(selectedGameId) ?? "#FFFFFF"}
                            />
                        </div>
                    }
                    {selectedTargetUnit &&
                        <div className="player">
                            <CombatReplyItem
                                unitCasts={unitsCast.get(selectedTargetGameId)}
                                unitsHealth={unitsHealth.get(selectedTargetGameId)}
                                unit={selectedTargetUnit}
                                color={colors.get(selectedTargetGameId) ?? "#FFFFFF"}
                            />
                        </div>
                    }
                </div>
            </li>
            <CombatReplyUnitsCategory
                name={t("Players")}
                units={playerUnits}
                unitPositions={unitPositions}
                unitsHealth={unitsHealth}
            />
            <CombatReplyUnitsCategory
                name={t("PlayerCreatures")}
                units={playerCreatureUnits}
                unitPositions={unitPositions}
                unitsHealth={unitsHealth}
            />
            <CombatReplyUnitsCategory
                name={t("Enemy")}
                units={enemyUnits}
                unitPositions={unitPositions}
                unitsHealth={unitsHealth}
            />
        </ul>
    );
}

export default memo(CombatReplyUnits);