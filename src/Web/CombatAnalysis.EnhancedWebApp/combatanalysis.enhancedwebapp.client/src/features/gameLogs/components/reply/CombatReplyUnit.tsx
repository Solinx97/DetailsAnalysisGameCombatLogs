import useNumber from '@/shared/hooks/useNumber';
import useTime from '@/shared/hooks/useTime';
import useCombatLogs from '@/shared/hooks/useCombatLogs';
import { faSkull } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import CombatReplyContext from '@/context/CombatReplyContext';
import { useContext, useEffect, useMemo, useState } from 'react';
import type { UnitCastModel } from '../../types/UnitCastModel';
import CastBar from './CastBar';
import InstantCast from './InstantCast';
import type { UnitModel } from '../../types/UnitModel';

interface CombatReplyItemProps {
    unit: UnitModel;
    color: string;
    unitCasts?: UnitCastModel[] | undefined;
}

const CombatReplyUnit: React.FC<CombatReplyItemProps> = ({ unit, color, unitCasts }) => {
    const INSTANT_CAST_DURATION = 500;

    const context = useContext(CombatReplyContext);

    if (!context) {
        throw new Error("Child must be inside CombatReplyContext.Provider");
    }

    const { selectedGameId, setSelectedGameId, setSelectedTargetGameId, currentTime } = context;
    
    const [currentHealth, setCurrentHealth] = useState(100);
    const [maxHealth, setMaxHealth] = useState(100);

    const { timeToMs } = useTime();
    const { formatNumber } = useNumber();
    const { removeServerName } = useCombatLogs();

    const health = useMemo(() => {
        if (!unit.unitHealthes) {
            return;
        }

        return unit.unitHealthes
            .filter(health => timeToMs(health.time) <= currentTime).at(-1);
    }, [currentTime, unit]);

    const currentHealthProcentage = useMemo(() => {
        if (currentHealth === 0 || maxHealth === 0) {
            return 0;
        }

        return (currentHealth / maxHealth) * 100;
    }, [currentHealth, maxHealth]);

    useEffect(() => {
        if (!health || health.currentHealth < 0 || health.maxHealth < 0) {
            return;
        }

        setCurrentHealth(health.currentHealth);
        setMaxHealth(health.maxHealth);
    }, [health]);

    const handleSelectUnit = () => {
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
                onClick={handleSelectUnit}>
                {health?.currentHealth === 0 &&
                    <FontAwesomeIcon
                        icon={faSkull}
                    />
                }
                <div>{removeServerName(unit.name)}</div>
                <div>{removeServerName(unit.unitHash)}</div>
            </div>
            <div className={`health ${health?.currentHealth === 0 ? 'dead' : ''}`}>
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

export default CombatReplyUnit;