import React, { useEffect, useRef, useState } from 'react';
import { useLazyGetCombatPlayerPositionByCombatIdQuery, useLazyGetCombatPlayersByCombatIdQuery } from '../../../store/api/CombatParserApi';

import "../../../styles/playerMovements.scss";

const PlayerMovements = () => {
    const canvasRef = useRef(null);

    const [combatId, setCombatId] = useState(0);
    const [combatPlayerPositions, setCombatPlayerPositions] = useState([]);
    const [sortedPositions, setSortedPositions] = useState(new Map());
    const [combatPlayers, setCombatPlayers] = useState([]);
    const [playerColors, setPlayerColors] = useState({});

    const [getCombatPlayerPositionByCombatIdAsync] = useLazyGetCombatPlayerPositionByCombatIdQuery();
    const [getCombatPlayersByCombatIdAsync] = useLazyGetCombatPlayersByCombatIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const id = +queryParams.get("combatId");
        setCombatId(id);
    }, []);

    useEffect(() => {
        if (combatId <= 0) {
            return;
        }

        const getCombatPlayerPositionByCombatId = async () => {
            var response = await getCombatPlayerPositionByCombatIdAsync(combatId);
            if (response.data !== undefined) {
                setCombatPlayerPositions(response.data);
            }
        }

        const getCombatPlayersByCombatId = async () => {
            var response = await getCombatPlayersByCombatIdAsync(combatId);
            if (response.data !== undefined) {
                setCombatPlayers(response.data);
                assignColors(response.data);
            }
        }

        getCombatPlayerPositionByCombatId();
        getCombatPlayersByCombatId();
    }, [combatId]);

    useEffect(() => {
        if (combatPlayers.length === 0 || combatPlayerPositions.length === 0) {
            return;
        }

        sortPositions();
    }, [combatPlayerPositions]);

    useEffect(() => {
        if (sortedPositions.size === 0) {
            return;
        }

        const context = prepareCanvas();
        drawPlayers(context);
    }, [sortedPositions]);

    const assignColors = (players) => {
        const colors = {};

        players.forEach(player => {
            colors[player.id] = `#${Math.floor(Math.random() * 16777215).toString(16)}`;
        });

        setPlayerColors(colors);
    }

    const sortPositions = () => {
        const positionsMap = new Map();

        combatPlayers.forEach(player => {
            const positions = combatPlayerPositions.filter(position => position.combatPlayerId === player.id);
            positionsMap.set(player.id, positions);
        });

        setSortedPositions(positionsMap);
    }

    const prepareCanvas = () => {
        if (canvasRef.current == null) {
            return;
        }

        const canvas = canvasRef.current;
        const context = canvas.getContext("2d");

        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;

        return context;
    }

    const draw = (context, posX, posY, color) => {
        const canvas = canvasRef.current;

        // Move the origin to the bottom-left corner
        context.translate(0, canvas.height);
        // Flip the Y-axis
        context.scale(1, -1);

        context.fillStyle = color; // Set the color for the point

        context.beginPath();

        if (posX < canvas.width && posY < canvas.height) {
            context.arc(posX, posY, 5, 0, 2 * Math.PI);
            context.fill();
        }

        context.stroke();

        context.setTransform(1, 0, 0, 1, 0, 0);
    }

    const drawPlayers = (context) => {
        let frame = 0;
        const maxFrames = Math.max(...Array.from(sortedPositions.values()).map(positions => positions.length));
        const speed = 250; // Speed in milliseconds
        let lastTimestamp = 0;

        const animate = (timestamp) => {
            if (!lastTimestamp) lastTimestamp = timestamp;
            const elapsed = timestamp - lastTimestamp;

            if (elapsed > speed) {
                context.clearRect(0, 0, canvasRef.current.width, canvasRef.current.height);

                sortedPositions.forEach((positions, playerId) => {
                    const posX = positions.length > frame
                        ? Math.abs(positions[frame].positionX)
                        : Math.abs(positions[positions.length - 1].positionX);
                    const posY = positions.length > frame
                        ? Math.abs(positions[frame].positionY)
                        : Math.abs(positions[positions.length - 1].positionY);

                    draw(context, posX, posY, playerColors[playerId]);
                });

                frame++;
                lastTimestamp = timestamp;
            }

            if (frame < maxFrames) {
                requestAnimationFrame(animate);
            }
        }

        requestAnimationFrame(animate);
    }

    return (
        <canvas ref={canvasRef} className="drawing-canvas"></canvas>
    );
}

export default PlayerMovements;