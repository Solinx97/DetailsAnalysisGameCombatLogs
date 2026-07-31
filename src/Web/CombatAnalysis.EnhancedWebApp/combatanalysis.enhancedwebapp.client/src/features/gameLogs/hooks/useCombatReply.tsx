import { useEffect, useRef, useState, type RefObject } from 'react';
import type { UnitPositionModel } from '../types/UnitPositionModel';
import type { CombatModel } from '../types/CombatModel';
import { useLazyGetCombatByIdQuery, useLazyGetBossMapByIdQuery } from '../api/GameLogs.api';

interface Position {
    x: number;
    y: number;
}

interface InstanceBounds {
    x0: number;
    x1: number;
    y0: number;
    y1: number;
}

interface WorldSize {
    height: number;
    width: number;
}

const useCombatReply = (
    selectedGameId: string,
    canvasRef: RefObject<HTMLCanvasElement | null>,
    unitPositions: Map<string, UnitPositionModel[]>,
    colors: Map<string, string>
) => {
    const zoom = 5;
    const otherElementsHeight = 250;

    const [currentTime, setCurrentTime] = useState(0);
    const [combatId, setCombatId] = useState(0);
    const [combat, setCombat] = useState<CombatModel>();
    const [instanceBounds, setInstanceBounds] = useState<InstanceBounds>({
        x0: 0,
        x1: 0,
        y0: 0,
        y1: 0
    });
    const [worldSize, setWorldSize] = useState<WorldSize>({
        width: 1,
        height: 1
    });
    const [view, setView] = useState<WorldSize>({
        width: window.innerWidth,
        height: window.innerHeight - otherElementsHeight,
    });

    const currentTimeRef = useRef(currentTime);
    const frameIdRef = useRef<number>(0);

    const [getCombat] = useLazyGetCombatByIdQuery();
    const [getBossMap] = useLazyGetBossMapByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(location.search);

        const combatId = parseInt(queryParams.get("id") ?? "0");

        setCombatId(combatId);
    }, []);

    useEffect(() => {
        const handleResize = () => {
            setView({
                width: window.innerWidth,
                height: window.innerHeight - otherElementsHeight,
            });
        };

        window.addEventListener("resize", handleResize);

        return () => {
            window.removeEventListener("resize", handleResize);
        }
    }, []);

    useEffect(() => {
        if (combatId < 1) {
            return;
        }

        const loadData = async () => {
            try {
                const [combat] = await Promise.all([
                    getCombat(combatId).unwrap(),
                ]);

                setCombat(combat);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [combatId]);

    useEffect(() => {
        if (!combat) {
            return;
        }

        const loadData = async () => {
            try {
                const [bossMap] = await Promise.all([
                    getBossMap(combat.boss.bossMapId).unwrap(),
                ]);

                const receivedInstanceBounds = {
                    x0: bossMap.x0,
                    x1: bossMap.x1,
                    y0: bossMap.y0,
                    y1: bossMap.y1,
                };

                setInstanceBounds(receivedInstanceBounds);
                setWorldSize({
                    width: receivedInstanceBounds.x0 - receivedInstanceBounds.x1,
                    height: receivedInstanceBounds.y0 - receivedInstanceBounds.y1,
                });
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [combat]);

    useEffect(() => {
        currentTimeRef.current = currentTime;
    }, [currentTime]);

    useEffect(() => {
        const canvas = canvasRef.current;

        if (!unitPositions || !canvas) {
            return;
        }

        const ctx = canvas.getContext("2d");

        if (!ctx) {
            return;
        }

        const render = () => {
            ctx.clearRect(
                0,
                0,
                canvas.width,
                canvas.height
            );

            ctx.save();

            unitPositions.forEach((positions, unit) => {
                const pos =
                    getPosition(
                        positions
                    );

                if (pos !== null) {
                    const pixel =
                        toPixel(
                            pos.x,
                            pos.y
                        );

                    const zoomed =
                        zoomPosition(
                            pixel.x,
                            pixel.y
                        );

                    drawUnit(
                        unit,
                        unit.startsWith("Player"),
                        ctx,
                        zoomed.x,
                        zoomed.y,
                        colors.get(unit) ?? "#000000"
                    );
                }

            });

            frameIdRef.current = requestAnimationFrame(render);
        }

        frameIdRef.current = requestAnimationFrame(render);

        return () => {
            cancelAnimationFrame(frameIdRef.current);
        }
    }, [unitPositions, selectedGameId, colors, view, instanceBounds]);

    const getPosition = (positions: UnitPositionModel[]): Position | null => {
        if (positions.length === 0) {
            return null;
        }

        const time = currentTimeRef.current;

        const firstTime = positions[0].timeMs;
        if (time < firstTime) {
            return null;
        }

        const last = positions[positions.length - 1];
        const lastTime = last.timeMs;

        if (time > lastTime) {
            return null;
        }

        let left = 0;
        let right = positions.length - 2;

        while (left <= right) {
            const mid = (left + right) >> 1;

            const before = positions[mid];
            const after = positions[mid + 1];

            const beforeMs = before.timeMs;
            const afterMs = after.timeMs;

            if (time < beforeMs) {
                right = mid - 1;
            }
            else if (time > afterMs) {
                left = mid + 1;
            }
            else {
                const progress = (time - beforeMs) / (afterMs - beforeMs);

                return {
                    x: before.x + (after.x - before.x) * progress,
                    y: before.y + (after.y - before.y) * progress
                };
            }
        }

        return null;
    }

    const toPixel = (worldX: number, worldY: number) => {
        const canvas = canvasRef.current;

        if (!canvas) {
            return { x: 0, y: 0 };
        }

        const scale = Math.min(
            canvas.width / worldSize.width,
            canvas.height / worldSize.height
        );

        const offsetX = (canvas.width - worldSize.width * scale) / 2;
        const offsetY = (canvas.height - worldSize.height * scale) / 2;

        return {
            x: (worldX - instanceBounds.x0) * scale + offsetX,
            y: (worldY - instanceBounds.y1) * scale + offsetY
        };
    }

    const zoomPosition = (x: number, y: number) => {
        const centerX = view.width / 2;
        const centerY = view.height / 2;

        return {
            x:
                centerX +
                (x - centerX) * zoom,

            y:
                centerY +
                (y - centerY) * zoom
        };
    }

    const drawUnit = (gameId: string, isPlayer: boolean, ctx: CanvasRenderingContext2D, x: number, y: number, color: string) => {
        ctx.beginPath();

        if (isPlayer) {
            ctx.arc(
                x,
                y,
                7,
                0,
                Math.PI * 2
            );
        }
        else {
            ctx.rect(
                x,
                y,
                15,
                15
            );
        }

        ctx.strokeStyle = color;
        ctx.lineWidth = 2;

        ctx.stroke();

        if (gameId === selectedGameId) {
            ctx.fillStyle = color;

            ctx.fill();
        }
    }

    const stop = () => {
        cancelAnimationFrame(frameIdRef.current);
    }

    return { view, currentTime, setCurrentTime, stop };
}

export default useCombatReply;