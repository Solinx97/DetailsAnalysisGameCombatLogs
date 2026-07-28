import useNumber from '@/shared/hooks/useNumber';
import {
    ResponsiveContainer,
    BarChart,
    Bar,
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
    name: string;
    useGetChartQuery: QueryHook<ChartModel[], number>;
}

const CombatPlayerGenericChart: React.FC<CombatPlayerGenericChartProps> = ({ combatPlayerId, name, useGetChartQuery }) => {
    const { data, isLoading } = useGetChartQuery(combatPlayerId);

    const { formatNumber } = useNumber();

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="general-details__radial-chart">
            <ResponsiveContainer width="100%" height={350}>
                <BarChart data={data}>
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
                    />
                    <Bar
                        dataKey="value"
                        name={name}
                        fill="#06d6a0"
                        isAnimationActive={false}
                    />
                    <Brush dataKey="time" />
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
}

export default CombatPlayerGenericChart;