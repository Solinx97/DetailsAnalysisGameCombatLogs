import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery, useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerPositionModel } from '../../types/CombatPlayerPositionModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import CombatReplyItem from './CombatReplyItem';
import useCombatReply from '../../hooks/useCombatReply';

import './CombatReply.scss';

const CombatReply: React.FC = () => {
    const { t } = useTranslation('combatDetails/reply');

    const navigate = useNavigate();
    const location = useLocation();

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combatPlayers, setCombatPlayers] = useState<CombatPlayerModel[]>([]);
    const [combatPlayerPositions, setCombatPlayerPositions] = useState<CombatPlayerPositionModel[]>([]);
    const [positions, setPositions] = useState<Map<number, CombatPlayerPositionModel[]>>(new Map());

    const [playing, setPlaying] = useState(false);
    const [selectedPlayerId, setSelectedPlayerId] = useState(0);

    const canvasRef = useRef<HTMLCanvasElement>(null);

    const { view, currentTime, setCurrentTime } = useCombatReply(selectedPlayerId, canvasRef, combatPlayerPositions, positions);

    const [getCombatPlayers] = useLazyGetCombatPlayersByCombatIdQuery();
    const [getCombatPlayerPositions] = useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery();

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const combatId = parseInt(searchParams.get("combat") ?? "1");
        const combatLogId = parseInt(searchParams.get("combatLog") ?? "1");

        setCombatId(combatId);
        setCombatLogId(combatLogId);
    }, []);

    useEffect(() => {
        if (combatId < 1) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatPlayers] = await Promise.all([
                    getCombatPlayers(combatId).unwrap(),
                ]);

                setCombatPlayers(combatPlayers);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [combatId]);

    useEffect(() => {
        if (combatPlayers.length < 1) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatPlayerPositions] = await Promise.all([
                    getCombatPlayerPositions(combatPlayers[0].id).unwrap(),
                ]);

                const positions = sortByTime(combatPlayerPositions);
                setCombatPlayerPositions(positions);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [combatPlayers]);

    useEffect(() => {
        if (!playing) {
            return;
        }

        const start = Date.now() - currentTime;

        const interval = setInterval(() => {
            const time = Date.now() - start;

            if (time >= timeToMs(combatPlayerPositions.at(-1)!.time)) {
                setCurrentTime(timeToMs(combatPlayerPositions.at(-1)!.time));
                setPlaying(false);
                return;
            }

            setCurrentTime(time);

        }, 16);

        return () => clearInterval(interval);
    }, [playing]);

    useEffect(() => {
        if (combatPlayers.length < 1) {
            return;
        }

        const loadData = async (combatPlayerId: number) => {
            try {
                const [combatPlayerPositions] = await Promise.all([
                    getCombatPlayerPositions(combatPlayerId).unwrap(),
                ]);

                const sortedPositions = sortByTime(combatPlayerPositions);
                positions.set(combatPlayerId, sortedPositions);
                setPositions(positions);
            } catch (e) {
                console.error(e);
            }
        }

        for (const player of combatPlayers) {
            loadData(player.id);
        }
    }, [combatPlayers]);

    const sortByTime = (combatPlayerPositions: CombatPlayerPositionModel[]): CombatPlayerPositionModel[] => {
        const sortedPositions = [...combatPlayerPositions].sort(
            (a, b) =>
                timeToMs(a.time) - timeToMs(b.time)
        );

        return sortedPositions;
    }

    const timeToMs = (time: string): number => {
        const [hours, minutes, seconds] = time.split(":").map(Number);

        return (
            hours * 3600 * 1000 +
            minutes * 60 * 1000 +
            seconds * 1000
        );
    }

    return (
        <div className="reply">
            <div className="reply__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectCombat")}</div>
                </div>
                <h5>{t("Combats")}</h5>
            </div>
            {(combatPlayerPositions !== undefined && combatPlayerPositions.length > 0) &&
                <>
                    <div className="reply__actions">
                        <button
                            onClick={() => setPlaying(!playing)}
                            className="play"
                        >
                            {playing ? t("Pause") : t("Play")}
                        </button>
                    </div>
                    <input
                        type="range"
                        min={0}
                        max={timeToMs(combatPlayerPositions.at(-1)!.time)}
                        value={currentTime}

                        onChange={(e) =>
                            setCurrentTime(
                                Number(e.target.value)
                            )
                        }
                    />
                    <div>
                        {Math.floor(currentTime / 1000)} сек
                    </div>
                    <canvas
                        ref={canvasRef}
                        width={view.width}
                        height={view.height}
                    />
                    <ul className="players">
                        {combatPlayers.map(item => (
                            <li key={item.id}>
                                <CombatReplyItem
                                    combatPlayer={item}
                                    selectedPlayerId={selectedPlayerId}
                                    setSelectedPlayerId={setSelectedPlayerId}
                                />
                            </li>
                        ))
                        }
                    </ul>
                </>
            }
        </div>
    );
}

export default CombatReply;