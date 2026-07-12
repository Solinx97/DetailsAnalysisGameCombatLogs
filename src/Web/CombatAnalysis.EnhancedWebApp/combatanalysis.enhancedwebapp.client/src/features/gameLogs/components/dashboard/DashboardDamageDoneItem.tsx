import { memo, useContext, useEffect, useState } from 'react';
import { DashboardContext } from './DashboardItem';
import type { DashboardModel } from '../../types/dashboard/DashboardModel';

const DashboardDamageDoneItem = () => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { dashboards, itemCount, setContentSize, formatNumber, compare, setFilter, filter } = context;

    const [filteredDashboardItem, setFilteredDashboardItem] = useState<DashboardModel[]>([]);

    useEffect(() => {
        setFilter(0);
        setContentSize(dashboards.length);
    }, []);

    useEffect(() => {
        if (!dashboards) {
            return;
        }

        setFilteredDashboardItem([...dashboards].sort(compare));
    }, [filter, dashboards]);

    return (
        <ul className="details">
            {filteredDashboardItem.slice(0, itemCount).map((combat, index) => (
                <li key={index} className="details-item">
                    <div>{combat.username}</div>
                    <div>{formatNumber(combat.averageDPS)}</div>
                </li>
            ))}
        </ul>
    );
}

export default memo(DashboardDamageDoneItem);