import { faSkull } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import useTime from '@/shared/hooks/useTime';
import { useEffect, useMemo, useState, type Dispatch, type SetStateAction } from 'react';
import type { CombatPlayerCastModel } from '../../types/CombatPlayerCastModel';
import { useLazyGetCombatPlayerCastsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import CastBar from './CastBar';
import InstantCast from './InstantCast';
import type { UnitHealthModel } from '../../types/UnitHealthModel';
import type { CombatUnitModel } from '../../types/CombatUnitModel';

interface CombatReplyItemProps {
    combatPlayerId: number;
    unit: CombatUnitModel;
    unitsHealth: UnitHealthModel[];
    selectedPlayerId: string;
    setSelectedPlayerId: Dispatch<SetStateAction<string>>;
    currentTime: number;
    color: string;
}

const CombatReplyItem: React.FC<CombatReplyItemProps> = ({ combatPlayerId, unit, unitsHealth, selectedPlayerId, setSelectedPlayerId, currentTime, color }) => {
    const INSTANT_CAST_DURATION = 500;

    const [combatPlayerCasts, setCombatPlayerCasts] = useState<CombatPlayerCastModel[]>([]);
    const [currentHealth, setCurrentHealth] = useState(100);
    const [maxHealth, setMaxHealth] = useState(100);

    const { timeToMs } = useTime();

    const [getCombatPlayerCasts] = useLazyGetCombatPlayerCastsByCombatPlayerIdQuery();

    useEffect(() => {
        if (unit.gameId !== selectedPlayerId) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatPlayerCasts] = await Promise.all([
                    getCombatPlayerCasts(combatPlayerId).unwrap(),
                ]);

                setCombatPlayerCasts(combatPlayerCasts);
            } catch (e) {
                console.error(e);
            }
        }

        loadData();
    }, [selectedPlayerId]);

    const notImmediatlyCasts = useMemo(
        () => combatPlayerCasts.filter(x => !x.isImmediatly),
        [combatPlayerCasts]
    );

    const currentNotImmediatlyCast = useMemo(() => {
        return notImmediatlyCasts
            .find(cast =>
                currentTime >= timeToMs(cast.startTime) &&
                currentTime <= timeToMs(cast.finishTime)
            );
    }, [currentTime, combatPlayerCasts]);

    const progressNotImmediatly = useMemo(() => {
        if (!currentNotImmediatlyCast) {
            return 0;
        }

        return (
            ((currentTime - timeToMs(currentNotImmediatlyCast.startTime)) /
                (timeToMs(currentNotImmediatlyCast.finishTime) - timeToMs(currentNotImmediatlyCast.startTime))) * 100
        );
    }, [currentTime, currentNotImmediatlyCast]);

    const immediatlyCasts = useMemo(
        () => combatPlayerCasts.filter(x => x.isImmediatly && x.isSuccess),
        [combatPlayerCasts]
    );

    const currentImmediatlyCast = useMemo(() => {
        return immediatlyCasts
            .find(cast =>
                currentTime >= timeToMs(cast.startTime) &&
                currentTime <= timeToMs(cast.startTime) + INSTANT_CAST_DURATION
            );
    }, [currentTime, combatPlayerCasts]);

    const health = useMemo(() => {
        return unitsHealth
            .filter(health => timeToMs(health.time) <= currentTime).at(-1);
    }, [currentTime, unitsHealth]);

    useEffect(() => {
        if (health === undefined) {
            return;
        }

        setCurrentHealth(health.currentHealth);
        setMaxHealth(health.maxHealth);
    }, [health]);

    const handleSelectPlayer = () => {
        if (selectedPlayerId !== unit.gameId) {
            setSelectedPlayerId("");
            setSelectedPlayerId(unit.gameId);
        }
        else if (selectedPlayerId !== "" && selectedPlayerId === unit.gameId) {
            setSelectedPlayerId("");
        }
        else {
            setSelectedPlayerId(unit.gameId);
        }
    }

    const removeServerName = (username: string): string => {
        const fullname = username.replaceAll('"', '').split('-')[0];

        return fullname;
    }

    return (
        <>
            <div className={`username ${selectedPlayerId === unit.gameId ? "selected" : ""}`} style={{ color: color }}
                onClick={handleSelectPlayer}>
                {health?.isDead &&
                    <FontAwesomeIcon
                        icon={faSkull}
                    />
                }
                <div>{removeServerName(unit.username)}</div>
            </div>
            <div className={`health ${health?.isDead ? 'dead' : ''}`}>
                <div className="health__current" style={{ width: `${currentHealth}%` }}>{currentHealth}/{maxHealth}</div>
            </div>
            <CastBar
                spell={currentNotImmediatlyCast?.spell}
                progress={progressNotImmediatly}
                isSuccess={currentNotImmediatlyCast?.isSuccess}
                isRunCast={selectedPlayerId === unit.gameId}
            />
            <InstantCast
                spell={currentImmediatlyCast?.spell}
                isRunCast={selectedPlayerId === unit.gameId}
            />
        </>
    );
}

export default CombatReplyItem;