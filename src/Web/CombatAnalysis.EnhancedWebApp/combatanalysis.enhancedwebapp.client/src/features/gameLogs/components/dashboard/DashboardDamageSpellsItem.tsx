import DashboardContext from '@/context/DashboardContext';
import { useContext, useEffect, useState } from 'react';
import { useGetCombatsDamageSpellsQuery } from '../../api/GameLogs.api';

export interface DashboardDamageSpellsItemProps {
    combatLogId: number;
}

const DashboardDamageSpellsItem: React.FC<DashboardDamageSpellsItemProps> = ({ combatLogId }) => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { itemCount, setContentSize, formatNumber } = context;

    const [sortedDashboardItem, setFilteredDashboardItem] = useState<Map<string, number>>(new Map());

    const { data, isLoading } = useGetCombatsDamageSpellsQuery(combatLogId);

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

export default DashboardDamageSpellsItem;