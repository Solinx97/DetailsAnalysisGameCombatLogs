import { memo, useEffect, useMemo, useState, type Dispatch, type SetStateAction } from 'react';
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

import './CombatReplyUnits.scss';

interface CombatReplyUnitsProps {
    t: (key: string) => string;
    unitPositions: Map<string, UnitPositionModel[]>;
    details: CombatDetailsModel;
    selectedGameId: string;
    setSelectedGameId: Dispatch<SetStateAction<string>>;
    currentTime: number;
    colors: Map<string, string>;
}

const CombatReplyUnits: React.FC<CombatReplyUnitsProps> = ({ t, unitPositions, details, selectedGameId, setSelectedGameId, currentTime, colors }) => {
    const [combatUnits, setCombatUnits] = useState<CombatUnitModel[]>([]);
    const [unitsCast, setUnitsCast] = useState<Map<string, UnitCastModel[]>>(new Map());
    const [unitsHealth, setUnitsHealth] = useState<Map<string, UnitHealthModel[]>>(new Map());
    const [selectedUnit, setSelectedUnit] = useState<CombatUnitModel | undefined>();
    const [selectedTargetGameId, setSelectedTargetGameId] = useState<string>("");
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

    const checkIfPositionsAlreadyExist = (positions: UnitPositionModel[] | undefined) => {
        return positions && positions[0].timeMs <= currentTime && positions.at(-1)!.timeMs >= currentTime;
    }

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
                                selectedGameId={selectedGameId}
                                setSelectedTargetGameId={setSelectedTargetGameId}
                                setSelectedGameId={setSelectedGameId}
                                currentTime={currentTime}
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
                                selectedGameId={selectedGameId}
                                setSelectedGameId={setSelectedGameId}
                                currentTime={currentTime}
                                color={colors.get(selectedTargetGameId) ?? "#FFFFFF"}
                            />
                        </div>
                    }
                </div>
            </li>
            <li className="units__category">
                <div className="title">{t("Players")}</div>
                <ul className="content">
                    {playerUnits.map((item) => (
                        checkIfPositionsAlreadyExist(unitPositions.get(item.gameId)) &&
                        <li className="player" key={item.gameId}>
                            <CombatReplyItem
                                unitsHealth={unitsHealth.get(item.gameId)}
                                unit={item}
                                selectedGameId={selectedGameId}
                                setSelectedGameId={setSelectedGameId}
                                currentTime={currentTime}
                                color={colors.get(item.gameId) ?? "#FFFFFF"}
                            />
                        </li>
                    ))
                    }
                </ul>
            </li>
            <li className="units__category">
                <div className="title">{t("PlayerCreatures")}</div>
                <ul className="content">
                    {playerCreatureUnits.map((item) => (
                        checkIfPositionsAlreadyExist(unitPositions.get(item.gameId)) &&
                        <li className="player" key={item.gameId}>
                            <CombatReplyItem
                                unitsHealth={unitsHealth.get(item.gameId)}
                                unit={item}
                                selectedGameId={selectedGameId}
                                setSelectedGameId={setSelectedGameId}
                                currentTime={currentTime}
                                color={colors.get(item.gameId) ?? "#FFFFFF"}
                            />
                        </li>
                    ))
                    }
                </ul>
            </li>
            <li className="units__category">
                <div className="title">{t("Enemy")}</div>
                <ul className="content">
                    {enemyUnits.map((item) => (
                        checkIfPositionsAlreadyExist(unitPositions.get(item.gameId)) &&
                        <li className="player" key={item.gameId}>
                            <CombatReplyItem
                                unitCasts={unitsCast.get(item.gameId)}
                                unitsHealth={unitsHealth.get(item.gameId)}
                                unit={item}
                                selectedGameId={selectedGameId}
                                setSelectedGameId={setSelectedGameId}
                                currentTime={currentTime}
                                color={colors.get(item.gameId) ?? "#FFFFFF"}
                            />
                        </li>
                    ))
                    }
                </ul>
            </li>
        </ul>
    );
}

export default memo(CombatReplyUnits);