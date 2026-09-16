import DashboardContext from '@/context/DashboardContext';
import { useContext, useEffect, useState } from 'react';
import { useGetCombatsHealSpellsQuery } from '../../api/GameLogs.api';

export interface DashboardHealSpellsItemProps {
    combatLogId: number;
}

const DashboardHealSpellsItem: React.FC<DashboardHealSpellsItemProps> = ({ combatLogId }) => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { formatNumber, contentSize, setDashboardsSize } = context;

    const [sortedDashboardItem, setFilteredDashboardItem] = useState<Map<string, number>>(new Map());

    const { data, isLoading } = useGetCombatsHealSpellsQuery(combatLogId);

    useEffect(() => {
        if (!data) {
            return;
        }

        const sorted = new Map(
            Object.entries(data).sort(([, valueA], [, valueB]) => valueB - valueA)
        );

        setFilteredDashboardItem(sorted);
        setDashboardsSize(sorted.size);
    }, [data]);

    if (isLoading || !data) {
        return (<div>Loading...</div>);
    }

    return (
        <ul className="details">
            {Array.from(sortedDashboardItem.entries()).filter(key => key[1] > 0).slice(0, contentSize).map(([key, value]) => (
                <li key={key} className="details-item">
                    <div>{key}</div>
                    <div>{formatNumber(value)}</div>
                </li>
            ))}
        </ul>
    );
}

export default DashboardHealSpellsItem;