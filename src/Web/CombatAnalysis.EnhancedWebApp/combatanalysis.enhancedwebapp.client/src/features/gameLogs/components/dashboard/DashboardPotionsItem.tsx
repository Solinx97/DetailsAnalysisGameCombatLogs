import { useContext, useEffect, useState } from 'react';
import { DashboardContext } from './DashboardItem';
import { useGetPotionsQuery } from '../../api/GameLogs.api';

export interface DashboardPotionsItemProps {
    combatLogId: number;
}

const DashboardPotionsItem: React.FC<DashboardPotionsItemProps> = ({ combatLogId }) => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { itemCount, setContentSize, formatNumber } = context;

    const [sortedDashboardItem, setFilteredDashboardItem] = useState<Map<string, number>>(new Map());

    const { data, isLoading } = useGetPotionsQuery(combatLogId);

    useEffect(() => {
        if (!data) {
            return;
        }

        const sorted = new Map(
            Object.entries(data).sort(([, valueA], [, valueB]) => valueB - valueA)
        );

        setFilteredDashboardItem(sorted);
        setContentSize(sorted.size);
    }, [data]);

    if (isLoading || !data) {
        return (<div>Loading...</div>);
    }

    return (
        <ul className="details">
            {Array.from(sortedDashboardItem.entries()).filter(key => key[1] > 0).slice(0, itemCount).map(([key, value]) => (
                <li key={key} className="details-item">
                    <div>{key}</div>
                    <div>{formatNumber(value)}</div>
                </li>
            ))}
        </ul>
    );
}

export default DashboardPotionsItem;