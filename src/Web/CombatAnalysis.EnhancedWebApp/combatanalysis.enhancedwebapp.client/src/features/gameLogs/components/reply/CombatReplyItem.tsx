import useTime from '@/shared/hooks/useTime';
import { useEffect, useMemo, useState, type Dispatch, type SetStateAction } from 'react';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { CombatPlayerCastModel } from '../../types/CombatPlayerCastModel';
import { useLazyGetCombatPlayerCastsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import CastBar from './CastBar';
import InstantCast from './InstantCast';

interface CombatReplyItemProps {
    combatPlayer: CombatPlayerModel;
    selectedPlayerId: number;
    setSelectedPlayerId: Dispatch<SetStateAction<number>>;
    currentTime: number;
    color: string;
}

const CombatReplyItem: React.FC<CombatReplyItemProps> = ({ combatPlayer, selectedPlayerId, setSelectedPlayerId, currentTime, color }) => {
    const INSTANT_CAST_DURATION = 500;

    const [combatPlayerCasts, setCombatPlayerCasts] = useState<CombatPlayerCastModel[]>([]);

    const { timeToMs } = useTime();

    const [getCombatPlayerCasts] = useLazyGetCombatPlayerCastsByCombatPlayerIdQuery();

    useEffect(() => {
        if (selectedPlayerId < 1) {
            return;
        }

        const loadData = async (combatPlayerId: number) => {
            try {
                const [combatPlayerCasts] = await Promise.all([
                    getCombatPlayerCasts(combatPlayerId).unwrap(),
                ]);

                setCombatPlayerCasts(combatPlayerCasts);
            } catch (e) {
                console.error(e);
            }
        }

        loadData(selectedPlayerId);
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
        () => combatPlayerCasts.filter(x => x.isImmediatly),
        [combatPlayerCasts]
    );

    const currentImmediatlyCast = useMemo(() => {
        return immediatlyCasts
            .find(cast =>
                currentTime >= timeToMs(cast.startTime) &&
                currentTime <= timeToMs(cast.startTime) + INSTANT_CAST_DURATION
            );
    }, [currentTime, combatPlayerCasts]);

    const handleSelectPlayer = () => {
        if (selectedPlayerId !== 0 && selectedPlayerId !== combatPlayer.id) {
            setSelectedPlayerId(0);
            setSelectedPlayerId(combatPlayer.id);
        }
        else if (selectedPlayerId !== 0 && selectedPlayerId === combatPlayer.id) {
            setSelectedPlayerId(0);
        }
        else {
            setSelectedPlayerId(combatPlayer.id);
        }
    }

    return (
        <>
            {(currentNotImmediatlyCast && selectedPlayerId === combatPlayer.id) &&
                <CastBar
                    spell={currentNotImmediatlyCast.spell}
                    progress={progressNotImmediatly}
                />
            }
            {(currentImmediatlyCast && selectedPlayerId === combatPlayer.id) &&
                <InstantCast
                    spell={currentImmediatlyCast.spell}
                />
            }
            <div className={`username ${selectedPlayerId === combatPlayer.id ? "selected" : ""}`} style={{ color: color }} onClick={handleSelectPlayer}>{combatPlayer.player.username}</div>
        </>
    );
}

export default CombatReplyItem;