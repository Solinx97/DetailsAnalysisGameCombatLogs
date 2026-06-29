import { faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type React from 'react';
import { type ChangeEvent, type SetStateAction } from 'react';

type QueryHook<TResult, TArg> = (arg: TArg) => { data?: TResult, isLoading: boolean };

interface DetailsFilterProps {
    combatPlayerId: number;
    setSelectedFilter: (value: SetStateAction<{ filter: string, target: string, spell: string }>) => void;
    selectedFilter: { filter: string, target: string, spell: string };
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

const DetailsFilter: React.FC<DetailsFilterProps> = ({ combatPlayerId, setSelectedFilter, selectedFilter, useGetUniqueFilterValuesQuery, t }) => {
    const defaultFilter = { filter: "None", target: "All", spell: "All" };

    const { data: uniqueTargets, isLoading: targetsIsLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter: filterTypes[1] });
    const { data: uniqueSpells, isLoading: spellsIsLoading } = useGetUniqueFilterValuesQuery({ combatPlayerId, filter: filterTypes[3] });

    const handleSelectedTarget = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? "All" : e.target.value;

        if (value === defaultFilter.target && selectedFilter.spell === "All") {
            setSelectedFilter({ filter: "None", target: value, spell: "All" });
        }
        else if (selectedFilter.spell !== "All") {
            setSelectedFilter({ filter: "All", target: value, spell: selectedFilter.spell });
        }
        else {
            setSelectedFilter({ filter: "Target", target: value, spell: "All" });
        }
    }

    const handleSelectedSpell = (e: ChangeEvent<HTMLSelectElement> | undefined) => {
        const value = e === undefined ? "All" : e.target.value;

        if (value === defaultFilter.spell && selectedFilter.target === "All") {
            setSelectedFilter({ filter: "None", target: "All", spell: value });
        }
        else if (selectedFilter.target !== "All") {
            setSelectedFilter({ filter: "All", target: selectedFilter.target, spell: value });
        }
        else {
            setSelectedFilter({ filter: "Spell", target: "All", spell: value });
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