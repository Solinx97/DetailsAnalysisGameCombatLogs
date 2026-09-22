import CombatReplyContext from '@/context/CombatReplyContext';
import { CombatUnitType } from '@/shared/helpers/EnumHelper';
import { memo, useContext, useEffect, useMemo, useState } from 'react';
import type { UnitModel } from '../../types/UnitModel';
import type { UnitCastModel } from '../../types/UnitCastModel';
import CombatReplyUnit from './CombatReplyUnit';
import CombatReplyUnitsCategory from './CombatReplyUnitsCategory';

import './CombatReplyUnits.scss';

interface CombatReplyUnitsProps {
    combatUnits: UnitModel[];
}

const CombatReplyUnits: React.FC<CombatReplyUnitsProps> = ({ combatUnits }) => {
    const context = useContext(CombatReplyContext);

    if (!context) {
        throw new Error("Child must be inside CombatReplyContext.Provider");
    }

    const { t, selectedGameId, selectedTargetGameId, colors } = context;

    const [selectedUnitCasts, setSelectedUnitCasts] = useState<UnitCastModel[]>([]);
    const [selectedTargetUnitCasts, setSelectedTargetUnitCasts] = useState<UnitCastModel[]>([]);
    const [selectedUnit, setSelectedUnit] = useState<UnitModel | undefined>();
    const [selectedTargetUnit, setSelectedTargetUnit] = useState<UnitModel | undefined>();

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
        if (!selectedUnit) {
            return;
        }

        setSelectedUnitCasts(selectedUnit.unitCasts);
    }, [selectedUnit]);

    useEffect(() => {
        if (!selectedTargetUnit) {
            return;
        }

        setSelectedTargetUnitCasts(selectedTargetUnit.unitCasts);
    }, [selectedTargetUnit]);

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
        return combatUnits.filter(x => x.type === CombatUnitType["Player"]);
    }, [combatUnits]);

    const playerCreatureUnits = useMemo(() => {
        return combatUnits.filter(x => x.type === CombatUnitType["PlayerCreature"] || x.type === CombatUnitType["Pet"]);
    }, [combatUnits]);

    const enemyUnits = useMemo(() => {
        return combatUnits.filter(x => x.type === CombatUnitType["EnemyCreature"] || x.type === CombatUnitType["Vehicle"]);
    }, [combatUnits]);

    return (
        <ul className="units">
            <li className="units__category">
                <div className="selected-units">
                    {selectedUnit &&
                        <div className="player">
                            <CombatReplyUnit
                                unitCasts={selectedUnitCasts}
                                unit={selectedUnit}
                                color={colors.get(selectedGameId) ?? "#FFFFFF"}
                            />
                        </div>
                    }
                    {selectedTargetUnit &&
                        <div className="player">
                            <CombatReplyUnit
                                unitCasts={selectedTargetUnitCasts}
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
            />
            <CombatReplyUnitsCategory
                name={t("PlayerCreatures")}
                units={playerCreatureUnits}
            />
            <CombatReplyUnitsCategory
                name={t("Enemy")}
                units={enemyUnits}
            />
        </ul>
    );
}

export default memo(CombatReplyUnits);