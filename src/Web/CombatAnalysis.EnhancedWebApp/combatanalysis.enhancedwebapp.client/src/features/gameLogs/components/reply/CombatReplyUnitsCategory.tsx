import { faMinus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import CombatReplyContext from '@/context/CombatReplyContext';
import useCombatLogs from '@/shared/hooks/useCombatLogs';
import { useContext, useEffect, useState } from 'react';
import type { UnitModel } from '../../types/UnitModel';
import type { UnitPositionModel } from '../../types/UnitPositionModel';
import CombatReplyUnit from './CombatReplyUnit';
import useTime from '@/shared/hooks/useTime';

interface CombatReplyUnitsCategoryProps {
    name: string;
    units: UnitModel[];
}

const CombatReplyUnitsCategory: React.FC<CombatReplyUnitsCategoryProps> = ({ name, units }) => {
    const context = useContext(CombatReplyContext);

    if (!context) {
        throw new Error("Child must be inside CombatReplyContext.Provider");
    }

    const { t, selectedGameId, setSelectedGameId, setSelectedTargetGameId, currentTime, colors } = context;

    const [hide, setHide] = useState(false);
    const [runExclude, setRunExclude] = useState(false);
    const [excluded, setExcluded] = useState<Map<string, string>>(new Map());
    const [excludedPositions, setExcludedPositions] = useState<Map<string, UnitPositionModel[]>>(new Map());

    const { removeServerName } = useCombatLogs();
    const { timeToMs } = useTime();

    useEffect(() => {
        excluded.forEach((_, value) => {
            const ePos = units.find(x => x.gameId === value)?.unitPositions;
            if (ePos) {
                excludedPositions.set(value, ePos);
                setExcludedPositions(excludedPositions);

                // unitPositions.delete(value);
            }
        });
    }, [excluded]);

    const excludeHandle = (gameId: string, username: string) => {
        const alreadyExcluded = new Map(excluded);
        alreadyExcluded.set(gameId, username);

        setExcluded(alreadyExcluded);

        if (selectedGameId === gameId) {
            setSelectedGameId("");
        }
    }

    const includeHandle = (gameId: string) => {
        const ePos = excludedPositions.get(gameId);
        const unitPos = units.find(x => x.gameId === gameId)?.unitPositions
        if (ePos && !unitPos) {
            // unitPositions.set(gameId, ePos);
            excludedPositions.delete(gameId);

            const exl = new Map(excluded);
            exl.delete(gameId);
            setExcluded(exl);
        }
    }

    const hideHandle = () => {
        setHide(true);
        setSelectedGameId("");
        setSelectedTargetGameId("");
    }

    const isHaveHealth = (unit: UnitModel) => {
        if (unit.unitHealthes.length === 0) {
            return false;
        }

        var healthes = unit.unitHealthes.filter(x => timeToMs(x.time) <= currentTime);
        return healthes && healthes.length > 0 && healthes[healthes.length - 1].currentHealth > 0;
    }

    if (hide) {
        return (
            <li className="units__category">
                <div className="title">
                    <div className="name hide" onClick={() => setHide(false)}>{name}</div>
                </div>
            </li>
        );
    }

    return (
        <li className="units__category">
            <div className="title">
                <div className="name" onClick={hideHandle}>{name}</div>
                <div className={`btn-shadow ${runExclude ? 'run' : ''}`} onClick={() => setRunExclude(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faMinus}
                    />
                    <div>{t("Exclude")}</div>
                </div>
            </div>
            <ul className="excluded">
                {Array.from(excluded.entries()).map(([key, value]) => (
                    <li key={key} onClick={() => includeHandle(key)}>{removeServerName(value)}</li>
                ))
                }
            </ul>
            <ul className="content">
                {units.map((item) => (
                    isHaveHealth(item) &&
                    <li className="player" key={item.gameId}>
                        {runExclude &&
                            <div className="btn-shadow" onClick={() => excludeHandle(item.gameId, item.name)}>
                                <FontAwesomeIcon
                                    icon={faMinus}
                                />
                            </div>
                        }
                        <CombatReplyUnit
                            unit={item}
                            color={colors.get(item.gameId) ?? "#FFFFFF"}
                        />
                    </li>
                ))
                }
            </ul>
        </li>
    );
}

export default CombatReplyUnitsCategory;