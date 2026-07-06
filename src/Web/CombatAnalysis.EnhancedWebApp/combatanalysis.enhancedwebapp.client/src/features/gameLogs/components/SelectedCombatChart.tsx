import { useEffect, useMemo, useState } from 'react';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import {
    ResponsiveContainer,
    LineChart,
    Line,
    XAxis,
    YAxis,
    Tooltip,
    CartesianGrid,
    Legend
} from 'recharts';
import { useLazyGetChartDamageDoneQuery } from '../api/DamageDone.api';
import type { ChartModel } from '../types/chart/ChartModel';

interface DetailsSpecificalCombatChartProps {
    combatPlayers: CombatPlayerModel[];
    colors: Array<string>;
}

const SelectedCombatChart: React.FC<DetailsSpecificalCombatChartProps> = ({ combatPlayers, colors }) => {
    const [combatPlayersData, setCombatPlayersData] = useState<Map<string, ChartModel[]>>(new Map());
    const [focusedPlayer, setFocusedPlayer] = useState<string | null>(null);

    const [getChart] = useLazyGetChartDamageDoneQuery();

    useEffect(() => {
        if (!combatPlayers || combatPlayers.length === 0) {
            return;
        }

        const loadCharts = () => {
            combatPlayers.forEach(async player => {
                const chart = await getChart(player.id).unwrap();
                combatPlayersData.set(player.player.username, chart);
                setCombatPlayersData(new Map(combatPlayersData));
            });
        }

        loadCharts();
    }, [combatPlayers]);

    const pivot = () => {
        const result = new Map<string, any>();

        for (const [player, values] of combatPlayersData.entries()) {
            for (const v of values) {
                const t = v.time;

                if (!result.has(t)) {
                    result.set(t, { time: t });
                }

                result.get(t)[player] = v.value;
            }
        }

        return Array.from(result.values())
            .sort((a, b) => a.time - b.time);
    }

    const chartData = useMemo(() => {
        const piv = pivot();
        return piv;
    }, [combatPlayersData]);

    const renderLegend = (props: any) => {
        return (
            <div className="legends">
                {props.payload.map((entry: any) => (
                    <span className="legend"
                        key={entry.value}
                        onClick={() => handlePlayerClick(entry.value)}
                        style={{
                            color: entry.color,
                            boxShadow: focusedPlayer === entry.value ? "1px 1px 1px 1px green" : "",
                        }}
                    >
                        {entry.value}
                    </span>
                ))}
            </div>
        );
    }

    const handlePlayerClick = (player: string) => {
        setFocusedPlayer(prev =>
            prev === player ? null : player
        );
    }

    if (combatPlayersData.size === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <ResponsiveContainer width="100%" height={350}>
            <LineChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" />

                <XAxis dataKey="time" />
                <YAxis />
                <Tooltip />
                <Legend content={renderLegend} />

                {Array.from(combatPlayersData.keys()).map((player, index) => (
                    <Line
                        key={player}
                        dataKey={player}
                        stroke={colors[index]}
                        dot={false}
                        strokeWidth={focusedPlayer === player ? 2 : 1}
                        opacity={focusedPlayer && focusedPlayer !== player ? 0.4 : 1}
                    />
                ))}
            </LineChart>
        </ResponsiveContainer>
    );
}

export default SelectedCombatChart;