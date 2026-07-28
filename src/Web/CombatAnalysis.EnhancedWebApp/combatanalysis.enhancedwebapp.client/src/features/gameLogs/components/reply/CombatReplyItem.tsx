import useNumber from '@/shared/hooks/useNumber';
import useTime from '@/shared/hooks/useTime';
import { faSkull } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useMemo, useState, type Dispatch, type SetStateAction } from 'react';
import type { UnitCastModel } from '../../types/UnitCastModel';
import CastBar from './CastBar';
import InstantCast from './InstantCast';
import type { UnitHealthModel } from '../../types/UnitHealthModel';
import type { CombatUnitModel } from '../../types/CombatUnitModel';

interface CombatReplyItemProps {
    unitsHealth: UnitHealthModel[] | undefined;
    unit: CombatUnitModel;
    selectedGameId: string;
    setSelectedGameId: Dispatch<SetStateAction<string>>;
    currentTime: number;
    color: string;
    setSelectedTargetGameId?: Dispatch<SetStateAction<string>>;
    unitCasts?: UnitCastModel[] | undefined;
}

const CombatReplyItem: React.FC<CombatReplyItemProps> = ({ unitsHealth, unit, selectedGameId, setSelectedGameId, setSelectedTargetGameId, currentTime, color, unitCasts }) => {
    const INSTANT_CAST_DURATION = 500;

    const [currentHealth, setCurrentHealth] = useState(100);
    const [maxHealth, setMaxHealth] = useState(100);

    const { timeToMs } = useTime();
    const { formatNumber } = useNumber();

    const health = useMemo(() => {
        if (!unitsHealth) {
            return;
        }

        return unitsHealth
            .filter(health => timeToMs(health.time) <= currentTime).at(-1);
    }, [currentTime, unitsHealth]);

    const currentHealthProcentage = useMemo(() => {
        if (currentHealth === 0 || maxHealth === 0) {
            return 0;
        }

        return (currentHealth / maxHealth) * 100;
    }, [currentHealth, maxHealth]);

    useEffect(() => {
        if (!health) {
            return;
        }

        setCurrentHealth(health.currentHealth);
        setMaxHealth(health.maxHealth);
    }, [health]);

    const handleSelectPlayer = () => {
        if (selectedGameId !== unit.gameId) {
            setSelectedGameId("");
            setSelectedGameId(unit.gameId);
        }
        else if (selectedGameId !== "" && selectedGameId === unit.gameId) {
            setSelectedGameId("");
            if (setSelectedTargetGameId) {
                setSelectedTargetGameId("");
            }
        }
        else {
            setSelectedGameId(unit.gameId);
        }
    }

    const removeServerName = (username: string): string => {
        const fullname = username.replaceAll('"', '').split('-')[0];

        return fullname;
    }

    const notImmediatlyCasts = useMemo(() => {
        if (!unitCasts) {
            return [];
        }

        return unitCasts.filter(x => !x.isImmediatly);
    }, [unitCasts]);

    const currentNotImmediatlyCast = useMemo(() => {
        const current = notImmediatlyCasts
            .find(cast =>
                currentTime >= timeToMs(cast.time) &&
                currentTime <= timeToMs(cast.finishTime)
            );

        return current;
    }, [currentTime]);

    const progressNotImmediatly = useMemo(() => {
        if (!currentNotImmediatlyCast) {
            return 0;
        }

        return (
            ((currentTime - timeToMs(currentNotImmediatlyCast.time)) /
                (timeToMs(currentNotImmediatlyCast.finishTime) - timeToMs(currentNotImmediatlyCast.time))) * 100
        );
    }, [currentTime, currentNotImmediatlyCast]);

    const immediatlyCasts = useMemo(() => {
        if (!unitCasts) {
            return [];
        }

        return unitCasts.filter(x => x.isImmediatly && x.isSuccess);
    }, [unitCasts]);

    const currentImmediatlyCast = useMemo(() => {
        const current = immediatlyCasts
            .find(cast =>
                currentTime >= timeToMs(cast.time) &&
                currentTime <= timeToMs(cast.time) + INSTANT_CAST_DURATION
            );

        return current;
    }, [currentTime]);

    useEffect(() => {
        if (currentImmediatlyCast && setSelectedTargetGameId) {
            setSelectedTargetGameId(currentImmediatlyCast.targetGameId ?? "");
        }
        else if (currentNotImmediatlyCast && setSelectedTargetGameId) {
            setSelectedTargetGameId(currentNotImmediatlyCast.targetGameId ?? "");
        }
    }, [currentImmediatlyCast, currentNotImmediatlyCast]);

    return (
        <>
            <div className={`username ${selectedGameId === unit.gameId ? "selected" : ""}`} style={{ color: color }}
                onClick={handleSelectPlayer}>
                {health?.isDead &&
                    <FontAwesomeIcon
                        icon={faSkull}
                    />
                }
                <div>{removeServerName(unit.username)}</div>
            </div>
            <div className={`health ${health?.isDead ? 'dead' : ''}`}>
                <div className="health__current" style={{ width: `${currentHealthProcentage}%` }}>{formatNumber(currentHealth)}/{formatNumber(maxHealth)}</div>
            </div>
            {unitCasts &&
                <>
                    <CastBar
                        spell={currentNotImmediatlyCast?.spell}
                        progress={progressNotImmediatly}
                        isSuccess={currentNotImmediatlyCast?.isSuccess}
                        isRunCast={selectedGameId === unit.gameId}
                    />
                    <InstantCast
                        spell={currentImmediatlyCast?.spell}
                        isRunCast={selectedGameId === unit.gameId}
                    />
                </>
            }
        </>
    );
}

export default CombatReplyItem;