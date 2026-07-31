import { faPlay, faPause } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import useTime from '@/shared/hooks/useTime';
import CombatReplyContext from '@/context/CombatReplyContext';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetUnitPositionsByCombatIdQuery } from '../../api/GameLogs.api';
import type { UnitPositionModel } from '../../types/UnitPositionModel';
import useCombatReply from '../../hooks/useCombatReply';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import CombatReplyUnits from './CombatReplyUnits';

import './CombatReply.scss';

const CombatReply: React.FC = () => {
    const { t } = useTranslation('combatDetails/reply');

    const navigate = useNavigate();
    const location = useLocation();

    const [colors, setColors] = useState<Map<string, string>>(new Map());
    const [details, setDetails] = useState<CombatDetailsModel>({
        id: 0,
        detailsType: 0,
        combatLogId: 0,
        name: '',
        number: 0,
        isWin: false,
        duration: 0
    });
    const [unitPositions, setUnitPositions] = useState<Map<string, UnitPositionModel[]>>(new Map());

    const [playing, setPlaying] = useState(false);
    const [selectedGameId, setSelectedGameId] = useState<string>("");
    const [selectedTargetGameId, setSelectedTargetGameId] = useState<string>("");

    const playingRef = useRef(false);
    const canvasRef = useRef<HTMLCanvasElement>(null);
    const lastFrameRef = useRef<number>(0);

    const { view, currentTime, setCurrentTime, stop } = useCombatReply(selectedGameId, canvasRef, unitPositions, colors);
    const { formatSeconds, timeToMs } = useTime();

    const [getUnitPositions] = useLazyGetUnitPositionsByCombatIdQuery();

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
        if (details.id < 1) {
            return;
        }

        const loadData = async () => {
            try {
                const [unitPositions] = await Promise.all([
                    getUnitPositions(details.id).unwrap(),
                ]);

                const unitPositionsMap = new Map(Object.entries(unitPositions));
                const unitPositionsUpdated = setTimeToms(unitPositionsMap);
                setUnitPositions(unitPositionsUpdated);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [details]);

    useEffect(() => {
        if (unitPositions.size === 0) {
            return;
        }

        const randomColors = getRandomColors(unitPositions);
        setColors(randomColors);
    }, [unitPositions]);

    useEffect(() => {
        if (!playing) {
            lastFrameRef.current = 0;
            return;
        }

        let frameId: number;
        const animate = (timestamp: number) => {
            if (!playingRef.current) {
                return;
            }

            if (lastFrameRef.current === 0) {
                lastFrameRef.current = timestamp;
            }

            const delta = timestamp - lastFrameRef.current;
            lastFrameRef.current = timestamp;

            const duration = details.duration * 1000;

            setCurrentTime(prev => {
                const next = prev + delta;

                if (next >= duration) {
                    setPlaying(false);
                    return duration;
                }

                return next;
            });

            if (playingRef.current) {
                frameId = requestAnimationFrame(animate);
            }
        }

        frameId = requestAnimationFrame(animate);

        return () => {
            cancelAnimationFrame(frameId);
        }
    }, [playing, unitPositions]);

    useEffect(() => {
        playingRef.current = playing;
    }, [playing]);

    const setTimeToms = (combatPlayerPositions: Map<string, UnitPositionModel[]>): Map<string, UnitPositionModel[]> => {
        return new Map(
            [...combatPlayerPositions.entries()].map(([key, positions]) => [
                key,
                positions
                    .map(p => ({
                        ...p,
                        timeMs: timeToMs(p.time)
                    }))
            ])
        );
    }

    const getRandomColors = (positions: Map<string, UnitPositionModel[]>) => {
        const colors = new Map<string, string>();

        positions.forEach((_, key) => {
            colors.set(key, `hsl(${Math.floor(Math.random() * 360)}, 70%, 50%)`);
        });

        return colors;
    }

    const selectOtherCombat = () => {
        setPlaying(false);
        canvasRef.current = null;
        stop();

        navigate(`/general-analysis?id=${details.combatLogId}`);
    }

    return (
        <div className="reply">
            <div className="reply__navigate">
                <div className="btn-shadow select-combat" onClick={selectOtherCombat}>
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
            {(unitPositions !== undefined && unitPositions.size > 0) &&
                <>
                    <canvas
                        ref={canvasRef}
                        width={view.width}
                        height={view.height}
                    />
                    <div className="reply__actions">
                        <div className="details">
                            <div className="play btn-shadow"
                                onClick={() => setPlaying(prev => !prev)}>
                                <FontAwesomeIcon
                                    icon={playing ? faPause : faPlay}
                                />
                                <div>{playing ? t("Pause") : t("Play")}</div>
                            </div>
                        </div>
                        <input
                            type="range"
                            min={0}
                            max={details.duration * 1000}
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
                    <CombatReplyContext.Provider
                        value={{
                            t: t,
                            selectedGameId: selectedGameId,
                            setSelectedGameId: setSelectedGameId,
                            selectedTargetGameId: selectedTargetGameId,
                            setSelectedTargetGameId: setSelectedTargetGameId,
                            currentTime: currentTime,
                            colors: colors,
                        }}
                    >
                        <CombatReplyUnits
                            unitPositions={unitPositions}
                            details={details}
                        />
                    </CombatReplyContext.Provider>
                </>
            }
        </div>
    );
}

export default CombatReply;