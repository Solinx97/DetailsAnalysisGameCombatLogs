import { faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type React from 'react';
import type { ChangeEvent, SetStateAction } from 'react';

type QueryHook<TResult, TArg> = (arg: TArg) => { data?: TResult, isLoading: boolean };

interface DetailsFilterProps {
    combatPlayerId: number;
    setSelectedFilter: (value: SetStateAction<{ filter: string, value: string }>) => void;
    selectedFilter: { filter: string, value: string };
    filter: string;
    filterName: string;
    useGetUniqueFilterValuesQuery: QueryHook<string[], { combatPlayerId: number, filter: string }>;
    t: (key: string) => string;
}

const DetailsFilter: React.FC<DetailsFilterProps> = ({ combatPlayerId, setSelectedFilter, selectedFilter, filter, filterName, useGetUniqueFilterValuesQuery, t }) => {
    const defaultFilter = { filter: "None", value: "All" };

    const { data: uniqueFilterValues, isLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter });

    const handleSelectedFilter = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? "All" : e.target.value;

        if (value === "All") {
            setSelectedFilter(defaultFilter);
        }
        else {
            setSelectedFilter({ filter, value });
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
            <select className="form-control" value={selectedFilter.value} onChange={handleSelectedFilter}>
                <option key="-1" value="All">{t("All")}</option>
                {uniqueFilterValues?.map((target, index) => (
                    <option key={index} value={target}>{target}</option>
                ))}
            </select>
        </div>
    );
}

export default DetailsFilter;