import { useGetDamageDoneCountTargetsByPlayerIdQuery, useGetUniqueTargetsQuery, useGetDamageDoneTargetByPlayerIdQuery } from '../../store/api/combatParser/DamageDone.api';
import React, { useEffect } from 'react';

const DetailsFilter = ({ combatPlayerId, setSelectedTarget, selectedTarget, setCount, setDamageDone, page, pageSize }) => {
    const { data: uniqueTargets, isLoading } = useGetUniqueTargetsQuery(combatPlayerId);
    const { data: count, isLoading: countIsLoading } = useGetDamageDoneCountTargetsByPlayerIdQuery({ combatPlayerId, target: selectedTarget });
    //const { data: filteredData, isLoading: filterIsLoading } = useGetDamageDoneTargetByPlayerIdQuery({ combatPlayerId, target: selectedTarget === "All" ? "-1": selectedTarget, page, pageSize });

    useEffect(() => {
        if (count === undefined || count === 0) {
            return;
        }

        setCount(count);
    }, [count]);

    //useEffect(() => {
    //    if (filteredData === undefined) {
    //        return;
    //    }

    //    setDamageDone(filteredData);
    //}, [filteredData]);

    if (isLoading || countIsLoading) {
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

export default DetailsFilter;