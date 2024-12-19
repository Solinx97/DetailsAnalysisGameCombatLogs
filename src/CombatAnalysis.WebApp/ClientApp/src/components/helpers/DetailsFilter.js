import { faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React from 'react';

const DetailsFilter = ({ combatPlayerId, setSelectedFilter, selectedFilter, filter, filterName, useGetUniqueFilterValuesQuery, t }) => {
    const defaultFilter = { filter: "None", value: -1 };

    const { data: uniqueFilterValues, isLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter });

    const handleSelecteFilter = (e) => {
        if (e.target.value === "All") {
            setSelectedFilter(defaultFilter);
        }
        else {
            setSelectedFilter({ filter, value: e.target.value });
        }
    }

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="player-filter-details__filter">
            <div>
                <div>{filterName}</div>
                <FontAwesomeIcon
                    icon={faRotate}
                    onClick={() => setSelectedFilter(defaultFilter)}
                    title={t("FiltersReset")}
                />
            </div>
            <select className="form-control" value={selectedFilter.value} onChange={handleSelecteFilter}>
                <option key="-1" value="All">{t("All")}</option>
                {uniqueFilterValues?.map((target, index) => (
                    <option key={index} value={target}>{target}</option>
                ))}
            </select>
        </div>
    );
}

export default DetailsFilter;