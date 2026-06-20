import { useEffect, useMemo, useState, type Dispatch, type RefObject, type SetStateAction } from 'react';
import { useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerPositionModel } from '../../types/CombatPlayerPositionModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';

interface Position {
    x: number;
    y: number;
}

interface Container {
    zoom: number,
    view: {
        width: number,
        height: number,
    },
    instanceBounds: {
        minX: number,
        maxX: number,
        minY: number,
        maxY: number
    },
    playerIconSize: {
        width: number,
        height: number
    },
}

interface CombatReplyItemProps {
    currentTimeRef: RefObject<number | null>;
    combatPlayer: CombatPlayerModel;
    container: Container;
    selectedPlayerId: number;
    setSelectedPlayerId: Dispatch<SetStateAction<number>>;
    canvasRef: RefObject<HTMLCanvasElement | null>;
}

const CombatReplyItem: React.FC<CombatReplyItemProps> = ({ currentTimeRef, combatPlayer, container, selectedPlayerId, setSelectedPlayerId, canvasRef }) => {
    const [combatPlayerPositions, setCombatPlayerPositions] = useState<CombatPlayerPositionModel[]>([]);

    const [getCombatPlayerPositions] = useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery();

    useEffect(() => {
        if (!combatPlayer) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatPlayerPositions] = await Promise.all([
                    getCombatPlayerPositions(combatPlayer.id).unwrap(),
                ]);

                sortByTime(combatPlayerPositions);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [combatPlayer]);

    useEffect(() => {
        const canvas = canvasRef.current;

        if (!combatPlayerPositions || !canvas) {
            return;
        }

        const ctx = canvas.getContext("2d");

        if (!ctx) return;

        let frameId: number;

        const render = () => {
            const pos = getPosition();

            const world =
                worldToContainer(
                    pos.x,
                    pos.y
                );

            const pixel =
                toPixel(
                    world.x,
                    world.y
                );

            const zoomed =
                applyZoom(
                    pixel.x,
                    pixel.y,
                    startPos.x,
                    startPos.y
                );

            drawPlayer(
                ctx,
                zoomed.x,
                zoomed.y,
                "red"
            );

            frameId =
                requestAnimationFrame(render);
        }

        frameId = requestAnimationFrame(render);

        return () => {
            cancelAnimationFrame(frameId);
        }
    }, [combatPlayerPositions, selectedPlayerId]);

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
        const rangeX = Math.max(1, container.instanceBounds.maxX - container.instanceBounds.minX);
        const rangeY = Math.max(1, container.instanceBounds.maxY - container.instanceBounds.minY);

        return {
            rangeX,
            rangeY
        };
    }, [bounds]);

    const drawPlayer = (
        ctx: CanvasRenderingContext2D,
        x: number,
        y: number,
        color: string
    ) => {

        ctx.beginPath();

        ctx.arc(
            x,
            y,
            10,
            0,
            Math.PI * 2
        );

        ctx.fillStyle = selectedPlayerId === combatPlayer.id ? "green" : color;

        ctx.fill();
    }

    const getPosition = (): Position => {
        if (combatPlayerPositions.length < 1) {
            return { x: 0, y: 0 };
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

    const worldToContainer = (x: number, y: number) => {
        if (combatPlayerPositions.length < 1) {
            return { x: 0, y: 0 };
        }

        const xNormalized =
            (x - container.instanceBounds.minX) / scale.rangeX;


        const yNormalized =
            (y - container.instanceBounds.minY) / scale.rangeY;

        return {
            x:
                container.instanceBounds.minX +
                xNormalized *
                (container.instanceBounds.maxX - container.instanceBounds.minX),

            y:
                container.instanceBounds.minY +
                yNormalized *
                (container.instanceBounds.maxY - container.instanceBounds.minY)
        };
    }

    const toPixel = (x: number, y: number) => {
        if (combatPlayerPositions.length < 1) {
            return { x: 0, y: 0 };
        }

        const rangeX =
            Math.max(
                1,
                container.instanceBounds.maxX -
                container.instanceBounds.minX
            );

        const rangeY =
            Math.max(
                1,
                container.instanceBounds.maxY -
                container.instanceBounds.minY
            );

        return {
            x:
                ((x - container.instanceBounds.minX) / rangeX)
                * (container.view.width - container.playerIconSize.width),

            y:
                (1 -
                    ((y - container.instanceBounds.minY) / rangeY)
                ) * (container.view.height - container.playerIconSize.height)
        };
    }

    const applyZoom = (x: number, y: number, startX: number, startY: number) => {
        return {
            x:
                startX +
                (x - startX) * container.zoom,

            y:
                startY +
                (y - startY) * container.zoom
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
    }

    return (
        <li className={`${selectedPlayerId === combatPlayer.id ? "selected" : ""}`} onClick={() => setSelectedPlayerId(combatPlayer.id)}>{combatPlayer.player.username}</li>
    );
}

export default CombatReplyItem;