import DashboardContext from '@/context/DashboardContext';
import { memo, useContext, useEffect } from 'react';

const DashboardDamageDoneItem = () => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { dashboards, formatNumber, setFilter} = context;

    useEffect(() => {
        setFilter(0);
    }, []);

    return (
        <ul className="details">
            {dashboards.filter(x => x.averageDPS > 0).map((combat, index) => (
                <li key={index} className="details-item">
                    <div>{combat.username}</div>
                    <div>{formatNumber(combat.averageDPS)}</div>
                </li>
            ))}
        </ul>
    );
}

export default memo(DashboardDamageDoneItem);