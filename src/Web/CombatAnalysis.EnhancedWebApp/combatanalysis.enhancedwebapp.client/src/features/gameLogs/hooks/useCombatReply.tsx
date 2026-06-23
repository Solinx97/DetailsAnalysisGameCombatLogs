import { useEffect, useRef, useState, type RefObject } from 'react';
import type { CombatPlayerPositionModel } from '../types/CombatPlayerPositionModel';

interface Position {
    x: number;
    y: number;
}

const useCombatReply = (selectedPlayerId: number, canvasRef: RefObject<HTMLCanvasElement | null>, combatPlayerPositions: CombatPlayerPositionModel[], positions: Map<number, CombatPlayerPositionModel[]>) => {
    const [currentTime, setCurrentTime] = useState(0);

    const currentTimeRef = useRef(currentTime);

    const view = {
        width: 1300,
        height: 425,
    };

    const zoom = 1;

    const instanceBounds = {
        minX: -4837,
        maxX: -4237,
        minY: 1500,
        maxY: 1900
    };

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
    }, [combatPlayerPositions, selectedPlayerId]);

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
                before.positionX +
                (after.positionX - before.positionX) * progress,

            y:
                before.positionY +
                (after.positionY - before.positionY) * progress
        };
    }

    const toPixel = (x: number, y: number) => {
        const rangeX =
            instanceBounds.maxX - instanceBounds.minX;

        const rangeY =
            instanceBounds.maxY - instanceBounds.minY;

        return {
            x:
                ((x - instanceBounds.minX) / rangeX)
                * view.width,

            y:
                (1 -
                    ((y - instanceBounds.minY) / rangeY)
                )
                * view.height
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