import useTime from '@/shared/hooks/useTime';
import DashboardContext from '@/context/DashboardContext';
import { useContext, useEffect, useState } from 'react';
import type { CombatModel } from '../../types/CombatModel';

export interface DashboardDurationItemProps {
    allUniqueCombats: Map<string, CombatModel[]>;
}

const DashboardDurationItem: React.FC<DashboardDurationItemProps> = ({ allUniqueCombats }) => {
    const context = useContext(DashboardContext);

    if (!context) {
        throw new Error("Child must be inside DashboardContext.Provider");
    }

    const { itemCount, setContentSize } = context;
    
    const { formatSeconds } = useTime();

    const [durations, setDuratiuons] = useState<Map<string, number>>(new Map());

    useEffect(() => {
        const allDurations = new Map<string, number>();

        allUniqueCombats.forEach((value, key) => {
            const averageDuration = value.reduce(
                (sum, x) => sum + parseDuration(x.duration),
                0
            ) / value.length;

            const round = Math.round(averageDuration);

            allDurations.set(key, round);
        });

        const sortedMap = new Map(
            [...allDurations.entries()]
                .sort((a, b) => a[1] - b[1])
        );

        setDuratiuons(sortedMap);
        setContentSize(sortedMap.size);
    }, []);

    const parseDuration = (duration: string): number => {
        const [hours, minutes, seconds] = duration.split(":").map(Number);

        return hours * 3600 + minutes * 60 + seconds;
    }

    if (allUniqueCombats.size === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <ul className="details">
            {Array.from(durations.entries()).slice(0, itemCount).map(([key, duration]) => (
                <li key={key} className="details-item">
                    <div>{key}</div>
                    <div>{formatSeconds(duration)}</div>
                </li>
            ))}
        </ul>
    );
}

export default DashboardDurationItem;