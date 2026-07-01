import { faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type React from 'react';
import { type ChangeEvent, type SetStateAction } from 'react';

type QueryHook<TResult, TArg> = (arg: TArg) => { data?: TResult, isLoading: boolean };

interface DetailsFilterProps {
    filters: string[];
    combatPlayerId: number;
    setSelectedFilter: (value: SetStateAction<{ filter: string, target: string, spell: string }>) => void;
    selectedFilter: { filter: string, target: string, spell: string };
    useGetUniqueFilterValuesQuery: QueryHook<string[], { combatPlayerId: number, filter: string }>;
    t: (key: string) => string;
}

const DetailsFilter: React.FC<DetailsFilterProps> = ({ filters, combatPlayerId, setSelectedFilter, selectedFilter, useGetUniqueFilterValuesQuery, t }) => {
    const defaultFilter = { filter: "None", target: "All", spell: "All" };

    const { data: uniqueTargets, isLoading: targetsIsLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter: filters[0] });
    const { data: uniqueSpells, isLoading: spellsIsLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter: filters[1] });

    const handleSelectedTarget = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? "All" : e.target.value;

        if (value === defaultFilter.target && selectedFilter.spell === defaultFilter.spell) {
            setSelectedFilter({ filter: "None", target: value, spell: defaultFilter.spell });
        }
        else if (value === defaultFilter.target && selectedFilter.filter === "All") {
            setSelectedFilter({ filter: "Spell", target: defaultFilter.target, spell: selectedFilter.spell });
        }
        else if (selectedFilter.spell !== "All") {
            setSelectedFilter({ filter: "All", target: value, spell: selectedFilter.spell });
        }
        else {
            setSelectedFilter({ filter: "Target", target: value, spell: defaultFilter.spell });
        }
    }

    const handleSelectedSpell = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? "All" : e.target.value;

        if (value === defaultFilter.spell && selectedFilter.target === defaultFilter.target) {
            setSelectedFilter({ filter: "None", target: defaultFilter.target, spell: value });
        }
        else if (value === defaultFilter.target && selectedFilter.filter === "All") {
            setSelectedFilter({ filter: "Target", target: selectedFilter.target, spell: defaultFilter.spell });
        }
        else if (selectedFilter.target !== "All") {
            setSelectedFilter({ filter: "All", target: selectedFilter.target, spell: value });
        }
        else {
            setSelectedFilter({ filter: "Spell", target: defaultFilter.target, spell: value });
        }
    }

    if (targetsIsLoading || spellsIsLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <div className="filter-type">
                <div className="title">
                    <div>{t("Target")}</div>
                    <FontAwesomeIcon
                        icon={faRotate}
                        onClick={() => setSelectedFilter(defaultFilter)}
                        title={t("FiltersReset")}
                    />
                </div>
                <select className="form-control" value={selectedFilter.target} onChange={handleSelectedTarget}>
                    <option key="-1" value="All">{t("All")}</option>
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
                    <option key="-1" value="All">{t("All")}</option>
                    {uniqueSpells?.map((target, index) => (
                        <option key={index} value={target}>{target}</option>
                    ))}
                </select>
            </div>
        </>
    );
}

export default DetailsFilter;