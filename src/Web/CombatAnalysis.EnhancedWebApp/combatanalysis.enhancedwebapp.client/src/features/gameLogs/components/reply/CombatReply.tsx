import CombatReplyContext from '@/context/CombatReplyContext';
import useTime from '@/shared/hooks/useTime';
import { faDeleteLeft, faPause, faPlay } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import {
    useGetCombatUnitsByCombatIdQuery
} from '../../api/GameLogs.api';
import useCombatReply from '../../hooks/useCombatReply';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { UnitModel } from '../../types/UnitModel';
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
        duration: 0,
        gameVersion: -1,
    });

    const [playing, setPlaying] = useState(false);
    const [selectedGameId, setSelectedGameId] = useState<string>("");
    const [selectedTargetGameId, setSelectedTargetGameId] = useState<string>("");

    const playingRef = useRef(false);
    const canvasRef = useRef<HTMLCanvasElement>(null);
    const lastFrameRef = useRef<number>(0);

    const { data: combatUnits, isLoading } = useGetCombatUnitsByCombatIdQuery(details.id);

    const { view, currentTime, setCurrentTime, stop } = useCombatReply(selectedGameId, canvasRef, combatUnits, colors);
    const { formatSeconds } = useTime();

    useEffect(() => {
        const queryParams = new URLSearchParams(location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';
        const duration: number = parseInt(queryParams.get("duration") || "1");
        const gameVersion: number = parseInt(queryParams.get("gameVersion") || "-1");

        setDetails({
            id,
            detailsType: 0,
            combatLogId,
            name,
            number,
            isWin,
            duration,
            gameVersion,
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
        if (!combatUnits || combatUnits.length === 0) {
            return;
        }

        const randomColors = getRandomColors(combatUnits);
        setColors(randomColors);
    }, [combatUnits]);

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
    }, [playing, combatUnits]);

    useEffect(() => {
        playingRef.current = playing;
    }, [playing]);

    const getRandomColors = (units: UnitModel[]) => {
        const colors = new Map<string, string>();

        units.forEach(key => {
            colors.set(key.gameId, `hsl(${Math.floor(Math.random() * 360)}, 70%, 50%)`);
        });

        return colors;
    }

    const selectOtherCombat = () => {
        setPlaying(false);
        canvasRef.current = null;
        stop();

        navigate(`/general-analysis?id=${details.combatLogId}`);
    }

    if (!combatUnits || isLoading) {
        return (<div>Loading...</div>);
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
                    combatUnits={combatUnits}
                />
            </CombatReplyContext.Provider>
        </div>
    );
}

export default CombatReply;