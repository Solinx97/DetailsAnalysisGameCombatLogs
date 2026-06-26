import { useEffect, useRef, useState, type RefObject } from 'react';
import type { CombatPlayerPositionModel } from '../types/CombatPlayerPositionModel';
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
    selectedPlayerId: number,
    canvasRef: RefObject<HTMLCanvasElement | null>,
    combatPlayerPositions: CombatPlayerPositionModel[],
    positions: Map<number, CombatPlayerPositionModel[]>
) => {
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

    const currentTimeRef = useRef(currentTime);

    const [getCombat] = useLazyGetCombatByIdQuery();
    const [getBossMap] = useLazyGetBossMapByIdQuery();

    const zoom = 1;
    const view = {
        width: 1300,
        height: 425,
    };

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const combatId = parseInt(searchParams.get("combat") ?? "1");

        setCombatId(combatId);
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

        if (!combatPlayerPositions || !canvas) {
            return;
        }

        const ctx = canvas.getContext("2d");

        if (!ctx) {
            return;
        }

        let frameId: number;

        const render = () => {
            ctx.clearRect(
                0,
                0,
                canvas.width,
                canvas.height
            );

            ctx.save();

            positions.forEach((position, key) => {
                const pos =
                    getPosition(
                        position
                    );

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

                drawPlayer(
                    key,
                    ctx,
                    zoomed.x,
                    zoomed.y,
                    "red"
                );
            });

            frameId =
                requestAnimationFrame(render);
        }

        frameId = requestAnimationFrame(render);

        return () => {
            cancelAnimationFrame(frameId);
        }
    }, [combatPlayerPositions, selectedPlayerId, instanceBounds]);

    const getPosition = (combatPlayerPositions: CombatPlayerPositionModel[]): Position => {
        if (combatPlayerPositions.length < 1) {
            return { x: 1, y: 1 };
        }

        let before = combatPlayerPositions[0];
        let after = combatPlayerPositions[combatPlayerPositions.length - 1];
        const time = currentTimeRef.current!;

        for (let i = 0; i < combatPlayerPositions.length - 1; i++) {
            if (time >= timeToMs(combatPlayerPositions[i].time) &&
                time <= timeToMs(combatPlayerPositions[i + 1].time)) {
                before = combatPlayerPositions[i];
                after = combatPlayerPositions[i + 1];
                break;
            }
        }

        const progress =
            (time - timeToMs(before.time)) /
            (timeToMs(after.time) - timeToMs(before.time));

        return {
            x:
                before.x +
                (after.x - before.x) * progress,

            y:
                before.y +
                (after.y - before.y) * progress
        };
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
            y: (instanceBounds.y0 - worldY) * scale + offsetY
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

    const drawPlayer = (combatPlayerId: number, ctx: CanvasRenderingContext2D, x: number, y: number, color: string) => {
        ctx.beginPath();

        ctx.arc(
            x,
            y,
            5,
            0,
            Math.PI * 2
        );

        ctx.fillStyle = selectedPlayerId === combatPlayerId ? "green" : color;

        ctx.fill();
    }

    const timeToMs = (time: string): number => {
        const [hours, minutes, seconds] = time.split(":").map(Number);

        return (
            hours * 3600 * 1000 +
            minutes * 60 * 1000 +
            seconds * 1000
        );
    }

    return { view, currentTime, setCurrentTime };
}

export default useCombatReply;