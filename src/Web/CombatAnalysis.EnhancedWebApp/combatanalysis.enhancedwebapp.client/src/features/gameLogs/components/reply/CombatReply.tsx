import { faPlay, faPause } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import useTime from '@/shared/hooks/useTime';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery, useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerPositionModel } from '../../types/CombatPlayerPositionModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import CombatReplyItem from './CombatReplyItem';
import useCombatReply from '../../hooks/useCombatReply';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';

import './CombatReply.scss';

const CombatReply: React.FC = () => {
    const { t } = useTranslation('combatDetails/reply');

    const navigate = useNavigate();
    const location = useLocation();

    const [colors, setColors] = useState<Map<number, string>>(new Map());
    const [details, setDetails] = useState<CombatDetailsModel>({
        id: 0,
        detailsType: 0,
        combatLogId: 0,
        name: '',
        number: 0,
        isWin: false,
        duration: 0
    });
    const [combatPlayers, setCombatPlayers] = useState<CombatPlayerModel[]>([]);
    const [combatPlayerPositions, setCombatPlayerPositions] = useState<CombatPlayerPositionModel[]>([]);
    const [positions, setPositions] = useState<Map<number, CombatPlayerPositionModel[]>>(new Map());

    const [playing, setPlaying] = useState(false);
    const [selectedPlayerId, setSelectedPlayerId] = useState(0);

    const canvasRef = useRef<HTMLCanvasElement>(null);
    const lastFrameRef = useRef<number>(0);

    const { view, currentTime, setCurrentTime } = useCombatReply(selectedPlayerId, canvasRef, combatPlayerPositions, positions, colors);
    const { formatSeconds, timeToMs } = useTime();

    const [getCombatPlayers] = useLazyGetCombatPlayersByCombatIdQuery();
    const [getCombatPlayerPositions] = useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';
        const duration: number = parseInt(queryParams.get("duration") || "1");

        setDetails({
            id,
            detailsType: 0,
            combatLogId,
            name,
            number,
            isWin,
            duration,
        });
    }, []);

    useEffect(() => {
        const handleKeyDown = (event: KeyboardEvent) => {
            if (event.code === "Space") {
                event.preventDefault();

                setPlaying(prev => !prev);
            }
        };

        window.addEventListener("keydown", handleKeyDown);

        return () => {
            window.removeEventListener("keydown", handleKeyDown);
        }
    }, []);

    useEffect(() => {
        if (details.id === 0) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatPlayers] = await Promise.all([
                    getCombatPlayers(details.id).unwrap(),
                ]);

                setCombatPlayers(combatPlayers);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [details]);

    useEffect(() => {
        if (positions.size === 0) {
            return;
        }

        setColors(getRandomColors(positions));
    }, [positions]);

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
                setPositions(new Map(positions));
            } catch (e) {
                console.error(e);
            }
        }

        for (const player of combatPlayers) {
            loadData(player.id);
        }
    }, [combatPlayers]);

    useEffect(() => {
        if (!playing) {
            return;
        }

        let frameId: number;
        const animate = (timestamp: number) => {
            if (lastFrameRef.current == null) {
                lastFrameRef.current = timestamp;
            }

            const delta = timestamp - lastFrameRef.current;

            lastFrameRef.current = timestamp;

            const duration = timeToMs(combatPlayerPositions.at(-1)!.time);

            setCurrentTime(prev => {
                const next = prev + delta;

                if (next >= duration) {
                    setPlaying(false);
                    return duration;
                }

                return next;
            });

            frameId = requestAnimationFrame(animate);
        }

        frameId = requestAnimationFrame(animate);

        return () => {
            cancelAnimationFrame(frameId);
        }
    }, [playing]);

    const sortByTime = (combatPlayerPositions: CombatPlayerPositionModel[]): CombatPlayerPositionModel[] => {
        const sortedPositions = [...combatPlayerPositions].sort(
            (a, b) =>
                timeToMs(a.time) - timeToMs(b.time)
        );

        return sortedPositions;
    }

    const getRandomColors = (positions: Map<number, CombatPlayerPositionModel[]>) => {
        const colors = new Map<number, string>();

        positions.forEach((_, key) => {
            colors.set(key, `hsl(${Math.floor(Math.random() * 360)}, 70%, 50%)`);
        });

        return colors;
    }

    return (
        <div className="reply">
            <div className="reply__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${details.combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectCombat")}</div>
                </div>
                <h5>{t("Combats")}</h5>
                <div className="boss-container">
                    <div className="boss">
                        <h5>{details.name}</h5>
                        <div className={`combat-number ${details.isWin ? 'win' : 'lose'}`}>{details.number}</div>
                    </div>
                </div>
            </div>
            {(combatPlayerPositions !== undefined && combatPlayerPositions.length > 0) &&
                <>
                    <canvas
                        ref={canvasRef}
                        width={view.width}
                        height={view.height}
                    />
                    <div className="reply__actions">
                        <div className="details">
                            <div className="play btn-shadow"
                                onClick={() => setPlaying(!playing)}>
                                <FontAwesomeIcon
                                    icon={playing ? faPause : faPlay}
                                />
                                <div>{playing ? t("Pause") : t("Play")}</div>
                            </div>
                        </div>
                        <input
                            type="range"
                            min={0}
                            max={timeToMs(combatPlayerPositions.at(-1)!.time)}
                            value={currentTime}
                            className="range"

                            onChange={(e) =>
                                setCurrentTime(
                                    Number(e.target.value)
                                )
                            }
                        />
                        <div className="time">
                            {formatSeconds(Math.floor(currentTime / 1000))}
                        </div>
                    </div>
                    <ul className="players">
                        {combatPlayers.map((item) => (
                            <li key={item.id}>
                                <CombatReplyItem
                                    combatPlayer={item}
                                    selectedPlayerId={selectedPlayerId}
                                    setSelectedPlayerId={setSelectedPlayerId}
                                    color={colors.get(item.id) ?? "#000000"}
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