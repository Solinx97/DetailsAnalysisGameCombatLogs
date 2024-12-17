import React from 'react';

const DetailsFilter = ({ combatPlayerId, setSelectedTarget, selectedTarget, useGetUniqueTargetsQuery, t }) => {
    const { data: uniqueTargets, isLoading } = useGetUniqueTargetsQuery(combatPlayerId);

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="player-filter-details">
            <div>
                <div>{t("Target")}</div>
                <select className="form-control" value={selectedTarget} onChange={(e) => setSelectedTarget(e.target.value)}>
                    <option key="-1" value="All">{t("All")}</option>
                    {uniqueTargets?.map((target, index) => (
                        <option key={index} value={target}>{target}</option>
                    ))}
                </select>
            </div>
        </div>
    );
}

export default DetailsFilter;