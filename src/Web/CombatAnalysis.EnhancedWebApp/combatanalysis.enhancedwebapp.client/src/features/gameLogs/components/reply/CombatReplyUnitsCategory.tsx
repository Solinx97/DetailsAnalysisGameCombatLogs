import { faMinus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import CombatReplyContext from '@/context/CombatReplyContext';
import useCombatLogs from '@/shared/hooks/useCombatLogs';
import { useContext, useEffect, useState } from 'react';
import type { CombatUnitModel } from '../../types/CombatUnitModel';
import type { UnitPositionModel } from '../../types/UnitPositionModel';
import CombatReplyItem from './CombatReplyItem';
import type { UnitHealthModel } from '../../types/UnitHealthModel';

interface CombatReplyUnitsCategoryProps {
    name: string;
    units: CombatUnitModel[];
    unitPositions: Map<string, UnitPositionModel[]>;
    unitsHealth: Map<string, UnitHealthModel[]>;
}

const CombatReplyUnitsCategory: React.FC<CombatReplyUnitsCategoryProps> = ({ name, units, unitPositions, unitsHealth }) => {
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

    useEffect(() => {
        excluded.forEach((_, value) => {
            const ePos = unitPositions.get(value);
            if (ePos) {
                excludedPositions.set(value, ePos);
                setExcludedPositions(excludedPositions);

                unitPositions.delete(value);
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
        const unitPos = unitPositions.get(gameId);
        if (ePos && !unitPos) {
            unitPositions.set(gameId, ePos);
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

    const checkIfPositionsAlreadyExist = (positions: UnitPositionModel[] | undefined) => {
        return positions && positions[0].timeMs <= currentTime && positions.at(-1)!.timeMs >= currentTime;
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
                    checkIfPositionsAlreadyExist(unitPositions.get(item.gameId)) &&
                    <li className="player" key={item.gameId}>
                        {runExclude &&
                            <div className="btn-shadow" onClick={() => excludeHandle(item.gameId, item.username)}>
                                <FontAwesomeIcon
                                    icon={faMinus}
                                />
                            </div>
                        }
                        <CombatReplyItem
                            unitsHealth={unitsHealth.get(item.gameId)}
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