import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery, useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerPositionModel } from '../../types/CombatPlayerPositionModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';

import '../auras/CombatAuras.scss';

interface Position {
    x: number;
    y: number;
};

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

    const [getCombatPlayers] = useLazyGetCombatPlayersByCombatIdQuery();
    const [getCombatPlayerPositions] = useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery();
    
    const view = {
        width: 1300,
        height: 700,
    };
    
    const instanceBounds = {
        minX: -9400,
        maxX: 9400,
        minY: -3300,
        maxY: 3300
    };

    const playerIconSize = {
        width: 20,
        height: 20
    };

    const zoom = 50;

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

    const bounds = useMemo(() => {
        const xs = combatPlayerPositions.map(p => p.positionX);
        const ys = combatPlayerPositions.map(p => p.positionY);

        return {
            minX: Math.min(...xs),
            maxX: Math.max(...xs),
            minY: Math.min(...ys),
            maxY: Math.max(...ys),
        };
    }, [combatPlayerPositions]);

    const scale = useMemo(() => {
        const rangeX = Math.max(1, instanceBounds.maxX - instanceBounds.minX);
        const rangeY = Math.max(1, instanceBounds.maxY - instanceBounds.minY);

        return {
            rangeX,
            rangeY
        };
    }, [bounds]);

    const getPosition = (): Position => {
        if (combatPlayerPositions.length < 1) {
            return { x: 0, y: 0 };
        }

        let before = combatPlayerPositions[0];
        let after = combatPlayerPositions[combatPlayerPositions.length - 1];

        for (let i = 0; i < combatPlayerPositions.length - 1; i++) {
            if (currentTime >= timeToMs(combatPlayerPositions[i].time) &&
                currentTime <= timeToMs(combatPlayerPositions[i + 1].time)) {
                before = combatPlayerPositions[i];
                after = combatPlayerPositions[i + 1];
                break;
            }
        }

        const progress =
            (currentTime - timeToMs(before.time)) /
            (timeToMs(after.time) - timeToMs(before.time));

        return {
            x:
                before.positionX +
                (after.positionX - before.positionX) * progress,

            y:
                before.positionY +
                (after.positionY - before.positionY) * progress
        };
    }

    const worldToContainer = (x: number, y: number) => {
        if (combatPlayerPositions.length < 1) {
            return { x: 0, y: 0 };
        }

        const xNormalized =
            (x - instanceBounds.minX) / scale.rangeX;


        const yNormalized =
            (y - instanceBounds.minY) / scale.rangeY;

        return {
            x:
                instanceBounds.minX +
                xNormalized *
                (instanceBounds.maxX - instanceBounds.minX),

            y:
                instanceBounds.minY +
                yNormalized *
                (instanceBounds.maxY - instanceBounds.minY)
        };
    }

    const toPixel = (x: number, y: number) => {
        if (combatPlayerPositions.length < 1) {
            return { x: 0, y: 0 };
        }

        const rangeX =
            Math.max(
                1,
                instanceBounds.maxX -
                instanceBounds.minX
            );

        const rangeY =
            Math.max(
                1,
                instanceBounds.maxY -
                instanceBounds.minY
            );

        return {
            x:
                ((x - instanceBounds.minX) / rangeX)
                * (view.width - playerIconSize.width),

            y:
                ((y - instanceBounds.minY) / rangeY)
                * (view.height - playerIconSize.height)
        };
    }

    const applyZoom = (x: number, y: number, startX: number, startY: number) => {
        return {
            x:
                startX +
                (x - startX) * zoom,

            y:
                startY +
                (y - startY) * zoom
        };
    }

    const startPos = useMemo(() => {
        if (combatPlayerPositions.length === 0) {
            return { x: 0, y: 0 };
        }

        const first = combatPlayerPositions[0];

        const world =
            worldToContainer(
                first.positionX,
                first.positionY
            );

        return toPixel(
            world.x,
            world.y
        );

    }, [combatPlayerPositions, bounds]);

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
    };

    const pos: Position = getPosition();
    const worldPos: Position = worldToContainer(pos.x, pos.y);
    const pixelPos: Position = toPixel(worldPos.x, worldPos.y);
    const zoomPos: Position = applyZoom(pixelPos.x, pixelPos.y, startPos.x, startPos.y);

    return (
        <div className="auras">
            <div className="auras__navigate">
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
                    <div
                        style={{
                            position: "relative",
                            width: view.width,
                            height: view.height,
                        }}
                    >
                        <div
                            style={{
                                width: playerIconSize.width,
                                height: playerIconSize.height,
                                background: "red",
                                position: "absolute",

                                transform:
                                    `translate(${zoomPos.x}px, ${zoomPos.y}px)`,

                                transition:
                                    "transform 0.05s linear"
                            }}
                        />
                    </div>
                </>
            }
        </div>
    );
}

export default CombatReply;