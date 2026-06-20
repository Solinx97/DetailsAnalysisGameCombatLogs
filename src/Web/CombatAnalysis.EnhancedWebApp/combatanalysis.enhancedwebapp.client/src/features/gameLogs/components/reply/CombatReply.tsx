import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery, useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerPositionModel } from '../../types/CombatPlayerPositionModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import CombatReplyItem from './CombatReplyItem';

import './CombatReply.scss';

const CombatReply: React.FC = () => {
    const { t } = useTranslation("combatDetails/auras");

    const navigate = useNavigate();
    const location = useLocation();

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combatPlayers, setCombatPlayers] = useState<CombatPlayerModel[]>([]);
    const [combatPlayerPositions, setCombatPlayerPositions] = useState<CombatPlayerPositionModel[]>([]);

    const [currentTime, setCurrentTime] = useState(0);
    const [playing, setPlaying] = useState(false);
    const [selectedPlayerId, setSelectedPlayerId] = useState(0);

    const canvasRef = useRef<HTMLCanvasElement>(null);
    const currentTimeRef = useRef(currentTime);

    const [getCombatPlayers] = useLazyGetCombatPlayersByCombatIdQuery();
    const [getCombatPlayerPositions] = useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery();

    const view = {
        width: 1300,
        height: 425,
    };

    const instanceBounds = {
        minX: -10000,
        maxX: 10000,
        minY: -4000,
        maxY: 4000
    };

    const playerIconSize = {
        width: 20,
        height: 20
    };

    const zoom = 30;

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const combatId = parseInt(searchParams.get("combat") ?? "1");
        const combatLogId = parseInt(searchParams.get("combatLog") ?? "1");

        setCombatId(combatId);
        setCombatLogId(combatLogId);
    }, []);

    useEffect(() => {
        currentTimeRef.current = currentTime;
    }, [currentTime]);

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

                sortByTime(combatPlayerPositions);
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
        const canvas = canvasRef.current;

        if (!combatPlayerPositions || !canvas) {
            return;
        }

        const ctx = canvas.getContext("2d");

        if (!ctx) return;

        let frameId: number;

        const render = () => {
            ctx.clearRect(
                0,
                0,
                canvas.width,
                canvas.height
            );

            frameId =
                requestAnimationFrame(render);
        }

        frameId = requestAnimationFrame(render);

        return () => {
            cancelAnimationFrame(frameId);
        }
    }, [combatPlayerPositions]);

    const sortByTime = (combatPlayerPositions: CombatPlayerPositionModel[]) => {
        const sortedPositions = [...combatPlayerPositions].sort(
            (a, b) =>
                timeToMs(a.time) - timeToMs(b.time)
        );

        setCombatPlayerPositions(sortedPositions);
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
            </div>
            {(combatPlayerPositions !== undefined && combatPlayerPositions.length > 0) &&
                <>
                    <button
                        onClick={() => setPlaying(!playing)}
                    >
                        {playing ? "Pause" : "Play"}
                    </button>
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
                            <CombatReplyItem
                                currentTimeRef={currentTimeRef}
                                combatPlayer={item}
                                container={{ zoom, view, instanceBounds, playerIconSize }}
                                selectedPlayerId={selectedPlayerId}
                                setSelectedPlayerId={setSelectedPlayerId}
                                canvasRef={canvasRef}
                            />
                        ))
                        }
                    </ul>
                </>
            }
        </div>
    );
}

export default CombatReply;