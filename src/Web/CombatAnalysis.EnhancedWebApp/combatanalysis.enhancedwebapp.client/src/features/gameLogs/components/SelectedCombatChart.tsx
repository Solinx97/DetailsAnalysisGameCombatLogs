import useNumber from '@/shared/hooks/useNumber';
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
import type { ChartModel } from '../types/chart/ChartModel';

type QueryHook<TResult, TArg> = (arg: TArg) => { data?: TResult, isLoading: boolean };

interface DetailsSpecificalCombatChartProps {
    combatPlayers: CombatPlayerModel[];
    combatId: number;
    colors: Array<string>;
    useGetGenericChartQuery: QueryHook<Map<string, ChartModel[]>, number>;
}

const SelectedCombatChart: React.FC<DetailsSpecificalCombatChartProps> = ({ combatPlayers, combatId, colors, useGetGenericChartQuery }) => {
    const [combatPlayersData, setCombatPlayersData] = useState<Map<string, ChartModel[]>>(new Map());
    const [focusedPlayer, setFocusedPlayer] = useState<string | null>(null);

    const { formatNumber } = useNumber();
    
    const { data, isLoading } = useGetGenericChartQuery(combatId);

    useEffect(() => {
        if (!data || combatPlayers.length === 0) {
            return;
        }

        setCombatPlayersData(new Map(Object.entries(data)));
    }, [data]);

    const pivot = () => {
        if (!combatPlayersData) {
            return;
        }

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

    if (isLoading || !combatPlayersData) {
        return (<div>Loading...</div>);
    }

    return (
        <ResponsiveContainer className="generic-chart" width="100%" height={350}>
            <LineChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" />

                <XAxis dataKey="time" />
                <YAxis tickFormatter={formatNumber} />
                <Tooltip
                    contentStyle={{
                        backgroundColor: "#1f1f1f",
                        border: "1px solid #555",
                        borderRadius: "20px",
                        color: "#fff"
                    }}
                    formatter={(value, name) => [
                        formatNumber(Number(value)),
                        name,
                    ]}
                    itemSorter={(item) => -Number(item.value)}
                />
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