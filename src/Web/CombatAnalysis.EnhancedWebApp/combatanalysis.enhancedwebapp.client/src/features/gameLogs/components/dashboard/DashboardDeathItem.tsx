import DashboardContext from '@/context/DashboardContext';
import { useContext, useEffect } from 'react';

const DashboardDeathItem = () => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { dashboards, setFilter} = context;

    useEffect(() => {
        setFilter(2);
    }, []);

    return (
        <ul className="details">
            {dashboards.map((combat, index) => (
                <li key={index} className="details-item">
                    <div>{combat.username}</div>
                    <div>{combat.deathCount}</div>
                </li>
            ))}
        </ul>
    );
}

export default DashboardDeathItem;