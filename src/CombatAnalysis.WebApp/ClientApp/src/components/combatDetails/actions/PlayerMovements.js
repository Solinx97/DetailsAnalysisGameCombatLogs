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
        if (combatPlayerPositions.length === 0) {
            return;
        }

        sortPositions();
    }, [combatPlayerPositions]);

    useEffect(() => {
        if (sortedPositions.size === 0 || canvasRef.current === null) {
            return;
        }

        const maxFrames = Math.max(...Array.from(sortedPositions.values()).map(positions => positions.length));
        const context = prepareCanvas();
        drawPlayers(context, maxFrames);
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

        const scaleX = 3; // Adjust this value to control horizontal spacing
        const scaleY = 3; // Adjust this value to control vertical spacing
        context.scale(scaleX, scaleY);

        return context;
    }

    const draw = (context, posX, posY, color) => {
        context.save(); // Save the current state of the context

        context.fillStyle = color; // Set the color for the point

        context.beginPath();
        context.arc(posX + 300, posY / 5, 5, 0, 2 * Math.PI); // Adjust the radius as needed
        context.fill();
        context.stroke();

        context.restore(); // Restore the context to its original state
    }

    const drawPlayers = (context, maxFrames) => {
        let frame = 0;
        const speed = 350; // Speed in milliseconds
        let lastTimestamp = 0;

        const animate = (timestamp) => {
            if (canvasRef.current == null) {
                return;
            }

            if (!lastTimestamp) {
                lastTimestamp = timestamp;
            }

            const elapsed = timestamp - lastTimestamp;

            if (elapsed > speed) {
                context.clearRect(0, 0, canvasRef.current.width, canvasRef.current.height);

                sortedPositions.forEach((positions, playerId) => {
                    const startX = positions.length > frame ? positions[frame].positionX : positions[positions.length - 1].positionX;
                    const startY = positions.length > frame ? positions[frame].positionY : positions[positions.length - 1].positionY;
                    const endX = positions.length > frame + 1 ? positions[frame + 1].positionX : startX;
                    const endY = positions.length > frame + 1 ? positions[frame + 1].positionY : startY;

                    const posX = startX + (endX - startX) * (elapsed / speed);
                    const posY = startY + (endY - startY) * (elapsed / speed);

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
        <div>
            <canvas ref={canvasRef} className="drawing-canvas"></canvas>
        </div>
    );
}

export default PlayerMovements;