import {
    ResponsiveContainer,
    LineChart,
    Line,
    XAxis,
    YAxis,
    Tooltip,
    CartesianGrid,
    Brush
} from 'recharts';
import type { ChartModel } from '../../types/chart/ChartModel';

type QueryHook<TResult, TArg> = (arg: TArg) => { data?: TResult, isLoading: boolean };

interface CombatPlayerGenericChartProps {
    combatPlayerId: number;
    useGetChartQuery: QueryHook<ChartModel[], number>;
}

const CombatPlayerGenericChart: React.FC<CombatPlayerGenericChartProps> = ({ combatPlayerId, useGetChartQuery }) => {
    const { data, isLoading } = useGetChartQuery(combatPlayerId);

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="general-details__radial-chart">
            <ResponsiveContainer width="100%" height={200}>
                <LineChart data={data}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="time" />
                    <YAxis />
                    <Tooltip />
                    <Line
                        type="monotone"
                        dataKey="value"
                        dot={false}
                        strokeWidth={2}
                        isAnimationActive={false}
                    />
                    <Brush dataKey="time" />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}

export default CombatPlayerGenericChart;