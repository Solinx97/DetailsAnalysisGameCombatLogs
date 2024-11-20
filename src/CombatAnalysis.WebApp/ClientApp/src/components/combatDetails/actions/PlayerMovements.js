import React, { useEffect, useRef, useState } from 'react';

const PlayerMovements = () => {
    const canvasRef = useRef(null);

    useEffect(() => {
        const canvas = canvasRef.current;
        const context = canvas.getContext('2d');

        context.translate(100, 100);

        // Example drawing: a line from (10, 10) to (100, 100)
        context.beginPath();
        context.moveTo(91.13, -62.89);
        context.lineTo(74.78, -68.88);
        context.stroke();
    }, []);

    return (
        <div>
            <canvas ref={canvasRef} width={500} height={500} className="drawing-canvas"></canvas>
        </div>
    );
}

export default PlayerMovements;