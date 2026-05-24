import { faPlus, faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useState, type SetStateAction } from 'react';
import type { CombatPlayerAuraModel } from '../../types/CombatPlayerAuraModel';

const auraType = {
    myselfBuff: 0,
    allyBuff: 1,
    petBuff: 2,
    allyCreatureBuff: 3,
    myselfDebuff: 4,
    allyDebuff: 5,
    petDebuff: 6,
    enemyDebuff: 7,
    enemyBuff: 8,
    enemyAllyBuff: 9,
};

const auraCreatorType = {
    player: 0,
    pet: 1,
    allyCreature: 2,
    enemyCreature: 3
};

interface CombatAuraFiltersProps {
    setCreators: (value: SetStateAction<CombatPlayerAuraModel[]>) => void;
    selectedCreator: string;
    handleSelectCreator: (creator: string) => void;
    allCreators: CombatPlayerAuraModel[];
    setSelectedCreatorAuras: (value: SetStateAction<CombatPlayerAuraModel[]>) => void;
    getAuraCreators: () => void;
    defaultSelectedCreatorAuras: CombatPlayerAuraModel[];
    t: (key: string) => string;
}

const CombatAuraFilters: React.FC<CombatAuraFiltersProps> = ({ setCreators, selectedCreator, handleSelectCreator, allCreators, setSelectedCreatorAuras, getAuraCreators, defaultSelectedCreatorAuras, t }) => {
    const [selectedIncludeToFilter, setSelectedIncludeToFilter] = useState(-1);
    const [selectedExcludeFromFilter, setSelectedExcludeFromFilter] = useState(-1);
    const [selectedAuraCreatorType, setSelectedAuraCreatorType] = useState(-1);
    const [filterApplied, setFilterApplied] = useState(false);

    const [showFilters, setShowFilters] = useState(false);

    const applyFilterCreatorAuraType = (number: number): void => {
        const newCreators = new Array<CombatPlayerAuraModel>();
        const filteredCreators = allCreators.filter(creator => creator.auraCreatorType === number);

        setCreators(newCreators.concat(filteredCreators));
        handleSelectCreator("All");
    }

    const applyFilterAuraType = (auraType: number, include = true): void => {
        const auras = new Array<CombatPlayerAuraModel>();

        defaultSelectedCreatorAuras.forEach(aura => {
            const condition = include
                ? aura.auraType === auraType
                : aura.auraType !== auraType;

            if ((auraType < 0 || condition) && (aura.creator === selectedCreator || selectedCreator === "All")) {
                auras.push(aura);
            }
        });

        setSelectedCreatorAuras(auras);
    }

    const restoreFiltersToDefault = (): void => {
        setCreators(allCreators);
        setSelectedIncludeToFilter(-1);
        setSelectedExcludeFromFilter(-1);
        setSelectedAuraCreatorType(-1);
        applyFilterAuraType(-1);

        setSelectedCreatorAuras(defaultSelectedCreatorAuras);

        setFilterApplied(false);
    }

    const handleApplyAuraIncludeFilter = (number: number): void => {
        if (selectedIncludeToFilter === number) {
            applyFilterAuraType(-1);
            setSelectedIncludeToFilter(-1);
            setFilterApplied(false);
        }
        else {
            applyFilterAuraType(number);
            setSelectedIncludeToFilter(number);
            setSelectedExcludeFromFilter(-1);
            setFilterApplied(true);
        }
    }

    const handleApplyAuraExcludeFilter = (number: number): void => {
        if (selectedExcludeFromFilter === number) {
            applyFilterAuraType(-1);
            setSelectedExcludeFromFilter(-1);
            setFilterApplied(false);
        }
        else {
            applyFilterAuraType(number, false);
            setSelectedExcludeFromFilter(number);
            setSelectedIncludeToFilter(-1);
            setFilterApplied(true);
        }
    }

    const handleApplyCreatorAuraFilter = (number: number): void => {
        if (selectedAuraCreatorType === number) {
            applyFilterCreatorAuraType(-1);
            setSelectedAuraCreatorType(-1);
            getAuraCreators();
            setFilterApplied(false);
        }
        else {
            applyFilterCreatorAuraType(number);
            setSelectedAuraCreatorType(number);
            setSelectedIncludeToFilter(-1);
            setSelectedExcludeFromFilter(-1);
            setFilterApplied(true);
        }
    }

    return (
        <div className="filters">
            <div className="filters__controll-panel">
                <div className={`btn-shadow ${filterApplied ? 'filter-applied' : ''}`} onClick={() => setShowFilters(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faPlus}
                    />
                    <div>{t("Filters")}</div>
                </div>
                <div className="filters__clear">
                    <FontAwesomeIcon
                        icon={faRotate}
                        onClick={restoreFiltersToDefault}
                        title={t("Reset")}
                    />
                </div>
            </div>
            <div className={`filters__aura-filters${showFilters ? '_show' : ''}`}>
                <div className="filters__aura-type-filter">
                    <div>{t("Include")}</div>
                    <ul>
                        {Object.entries(auraType).map(([key, value]) => (
                            <li className={selectedIncludeToFilter === value ? 'filter-selected' : ''} key={key}
                                onClick={() => handleApplyAuraIncludeFilter(value)}>{key}</li>
                        ))}
                    </ul>
                </div>
                <div className="filters__aura-type-filter">
                    <div>{t("Exclude")}</div>
                    <ul>
                        {Object.entries(auraType).map(([key, value]) => (
                            <li className={selectedExcludeFromFilter === value ? 'filter-selected' : ''} key={key}
                                onClick={() => handleApplyAuraExcludeFilter(value)}>{key}</li>
                        ))}
                    </ul>
                </div>
                <div className="filters__aura-type-filter">
                    <div>{t("Creator")}</div>
                    <ul>
                        {Object.entries(auraCreatorType).map(([key, value]) => (
                            <li className={selectedAuraCreatorType === value ? 'filter-selected' : ''} key={key}
                                onClick={() => handleApplyCreatorAuraFilter(value)}>{key}</li>
                        ))}
                    </ul>
                </div>
            </div>
        </div>
    );
}

export default CombatAuraFilters;