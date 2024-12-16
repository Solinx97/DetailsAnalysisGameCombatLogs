import React from 'react';
import { useGetUniqueTargetsQuery } from '../../store/api/combatParser/DamageDone.api';

const DamageDoneDetailsFilter = ({ combatPlayerId, setSelectedTarget, selectedTarget }) => {
    const { data: uniqueTargets, isLoading } = useGetUniqueTargetsQuery(combatPlayerId);

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div>
            <div>Targets:</div>
            <select value={selectedTarget} onChange={(e) => setSelectedTarget(e.target.value)}>
                <option key="-1" value="All">All</option>
                {uniqueTargets?.map((target, index) => (
                    <option key={index} value={target}>{target}</option>
                ))}
            </select>
        </div>
    );
}

export default DamageDoneDetailsFilter;