import { faRotate, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type React from 'react';
import { useState, type ChangeEvent, type SetStateAction } from 'react';

type QueryHook<TResult, TArg> = (arg: TArg) => { data?: TResult, isLoading: boolean };

interface DetailsFilterProps {
    filters: string[];
    combatPlayerId: number;
    setSelectedFilter: (value: SetStateAction<{ target: string, creator: string, spell: string, from: string, to: string }>) => void;
    selectedFilter: { target: string, creator: string, spell: string, from: string, to: string };
    useGetUniqueFilterValuesQuery: QueryHook<string[], { combatPlayerId: number, filter: string }>;
    t: (key: string) => string;
}

const DetailsFilter: React.FC<DetailsFilterProps> = ({ filters, combatPlayerId, setSelectedFilter, selectedFilter, useGetUniqueFilterValuesQuery, t }) => {
    const NONE_VALUE = "NONE";
    const ZERO_TIME_VALUE = "00:00:00";

    const defaultFilter = { target: NONE_VALUE, creator: NONE_VALUE, spell: NONE_VALUE, from: ZERO_TIME_VALUE, to: ZERO_TIME_VALUE };

    const [timeFrom, setTimeFrom] = useState(ZERO_TIME_VALUE);
    const [timeTo, setTimeTo] = useState(ZERO_TIME_VALUE);

    const { data: uniqueTargets, isLoading: targetsIsLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter: filters[0] });
    const { data: uniqueSpells, isLoading: spellsIsLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter: filters[1] });

    const handleSelectedTarget = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? NONE_VALUE : e.target.value;

        if (filters[0] == "Target") {
            setSelectedFilter({ target: value, creator: selectedFilter.creator, spell: selectedFilter.spell, from: selectedFilter.from, to: selectedFilter.to });
        }
        else {
            setSelectedFilter({ target: selectedFilter.target, creator: value, spell: selectedFilter.spell, from: selectedFilter.from, to: selectedFilter.to });
        }
    }

    const handleSelectedSpell = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? NONE_VALUE : e.target.value;

        setSelectedFilter({ target: selectedFilter.target, creator: selectedFilter.creator, spell: value, from: selectedFilter.from, to: selectedFilter.to });
    }

    const handleSelectedFromTime = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        const value = e === undefined ? NONE_VALUE : e.target.value;

        setTimeFrom(value);
    }

    const handleSelectedToTime = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        const value = e === undefined ? NONE_VALUE : e.target.value;

        setTimeTo(value);
    }

    const handleApplyTime = () => {
        setSelectedFilter({ target: selectedFilter.target, creator: selectedFilter.creator, spell: selectedFilter.spell, from: timeFrom, to: timeTo });
    }

    if (targetsIsLoading || spellsIsLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <div className="filter-type">
                <div className="title">
                    <div>{filters[0] == "Target" ? t("Target") : t("Creator")}</div>
                    <FontAwesomeIcon
                        icon={faRotate}
                        onClick={() => setSelectedFilter(defaultFilter)}
                        title={t("FiltersReset")}
                    />
                </div>
                <select className="form-control" value={filters[0] == "Target" ? selectedFilter.target : selectedFilter.creator} onChange={handleSelectedTarget}>
                    <option key="-1" value={NONE_VALUE}>{t("All")}</option>
                    {uniqueTargets?.map((target, index) => (
                        <option key={index} value={target}>{target}</option>
                    ))}
                </select>
            </div>
            <div className="filter-type">
                <div className="title">
                    <div>{t("Spell")}</div>
                    <FontAwesomeIcon
                        icon={faRotate}
                        onClick={() => setSelectedFilter(defaultFilter)}
                        title={t("FiltersReset")}
                    />
                </div>
                <select className="form-control" value={selectedFilter.spell} onChange={handleSelectedSpell}>
                    <option key="-1" value={NONE_VALUE}>{t("All")}</option>
                    {uniqueSpells?.map((target, index) => (
                        <option key={index} value={target}>{target}</option>
                    ))}
                </select>
            </div>
            <div className="filter-type">
                <div className="title">
                    <div>{t("Time")}</div>
                    <FontAwesomeIcon
                        icon={faRotate}
                        onClick={() => setSelectedFilter(defaultFilter)}
                        title={t("FiltersReset")}
                    />
                </div>
                <div className="time">
                    <div>
                        <input type="text" value={timeFrom} placeholder="Start time" onChange={handleSelectedFromTime} />
                        <input type="text" value={timeTo} placeholder="Finish time" onChange={handleSelectedToTime} />
                    </div>
                    <div className="btn-shadow" onClick={handleApplyTime}>
                        <FontAwesomeIcon
                            icon={faPlus}
                        />
                        <div>{t("Apply")}</div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default DetailsFilter;