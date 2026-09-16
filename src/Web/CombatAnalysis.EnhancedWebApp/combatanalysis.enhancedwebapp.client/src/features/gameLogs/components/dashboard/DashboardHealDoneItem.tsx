import DashboardContext from '@/context/DashboardContext';
import { memo, useContext, useEffect } from 'react';

const DashboardHealDoneItem = () => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { dashboards, formatNumber, setFilter} = context;

    useEffect(() => {
        setFilter(1);
    }, []);

    return (
        <ul className="details">
            {dashboards.filter(x => x.averageHPS > 0).map((combat, index) => (
                <li key={index} className="details-item">
                    <div>{combat.username}</div>
                    <div>{formatNumber(combat.averageHPS)}</div>
                </li>
            ))}
        </ul>
    );
}

export default memo(DashboardHealDoneItem);