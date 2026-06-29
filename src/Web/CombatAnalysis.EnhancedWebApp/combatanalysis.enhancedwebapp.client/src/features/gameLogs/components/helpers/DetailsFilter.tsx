import { faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type React from 'react';
import { useEffect, useState, type ChangeEvent, type SetStateAction } from 'react';

type QueryHook<TResult, TArg> = (arg: TArg) => { data?: TResult, isLoading: boolean };

interface DetailsFilterProps {
    number: number;
    combatPlayerId: number;
    setSelectedFilter: (value: SetStateAction<{ filter: string, value: string }>) => void;
    selectedFilter: { filter: string, value: string };
    filter: string;
    filterName: string;
    useGetUniqueFilterValuesQuery: QueryHook<string[], { combatPlayerId: number, filter: string }>;
    t: (key: string) => string;
}

const filterTypes = {
    0: "None",
    1: "Target",
    2: "Creator",
    3: "Spell",
    4: "All"
}

const DetailsFilter: React.FC<DetailsFilterProps> = ({ number, combatPlayerId, setSelectedFilter, selectedFilter, filter, filterName, useGetUniqueFilterValuesQuery, t }) => {
    const defaultFilter = { filter: "None", value: "All" };

    const { data: uniqueFilterValues, isLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter });

    const [newSelectedFilter, setNewSelectedFilter] = useState(defaultFilter);

    useEffect(() => {
        const splitValue = selectedFilter.value.split(';');
        if (splitValue.length === 2) {
            setNewSelectedFilter({ filter: selectedFilter.filter, value: splitValue[number] });
        }
        else {
            setNewSelectedFilter({ filter: selectedFilter.filter, value: selectedFilter.value });
        }
    }, [selectedFilter]);

    const handleSelectedFilter = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? "All" : e.target.value;

        if (value === defaultFilter.value) {
            setSelectedFilter(defaultFilter);
        }
        else if ((selectedFilter.filter !== filterTypes[0] && selectedFilter.filter !== filter)
            || selectedFilter.filter === filterTypes[4]) {
            const splitValue = selectedFilter.value.split(';');
            if (splitValue.length === 1) {
                if (number === 1) {
                    setSelectedFilter({ filter: filterTypes[4], value: selectedFilter.value + ';' + value });
                }
                else {
                    setSelectedFilter({ filter: filterTypes[4], value: value + ';' + selectedFilter.value });
                }
            }
            else if (splitValue.length === 2) {
                if (number === 1) {
                    setSelectedFilter({ filter: filterTypes[4], value: splitValue[0] + ';' + value });
                }
                else {
                    setSelectedFilter({ filter: filterTypes[4], value: value + ';' + splitValue[1] });
                }
            }
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
            <select className="form-control" value={newSelectedFilter.value} onChange={handleSelectedFilter}>
                <option key="-1" value="All">{t("All")}</option>
                {uniqueFilterValues?.map((target, index) => (
                    <option key={index} value={target}>{target}</option>
                ))}
            </select>
        </div>
    );
}

export default DetailsFilter;