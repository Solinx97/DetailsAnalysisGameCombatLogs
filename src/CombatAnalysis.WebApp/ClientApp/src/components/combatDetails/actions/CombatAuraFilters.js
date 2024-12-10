import { faPlus, faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useState } from 'react';

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

const CombatAuraFilters = ({ t, setCreators, selectedCreator, setSelectedCreator, allCreators, setSelectedCreatorAuras, getAuraCreators, defaultSelectedCreatorAuras }) => {
    const [selectedIncludeToFilter, setSelectedIncludeToFilter] = useState(-1);
    const [selectedExcludeFromFilter, setSelectedExcludeFromFilter] = useState(-1);
    const [selectedAuraCreatorType, setSelectedAuraCreatorType] = useState(-1);
    const [filterApplied, setFilterApplied] = useState(false);

    const [showFilters, setShowFilters] = useState(false);

    const applyFilterCreatorAuraType = (number) => {
        const newCreators = [{ creator: "None" }];
        const filteredCreators = allCreators.filter(creator => creator.auraCreatorType === number);

        setCreators(newCreators.concat(filteredCreators));
        setSelectedCreator({ creator: "None" });
        setSelectedCreatorAuras([]);
    }

    const applyFilterAuraType = (auraType, include = true) => {
        const auras = [];

        defaultSelectedCreatorAuras.forEach(aura => {
            const condition = include
                ? aura.auraType === auraType
                : aura.auraType !== auraType;

            if ((auraType < 0 || condition) && aura.creator === selectedCreator) {
                auras.push(aura);
            }
        });

        setSelectedCreatorAuras(auras);
    }

    const restoreFiltersToDefault = () => {
        setCreators(allCreators);
        setSelectedIncludeToFilter(-1);
        setSelectedExcludeFromFilter(-1);
        setSelectedAuraCreatorType(-1);
        applyFilterAuraType(-1);

        setSelectedCreatorAuras(defaultSelectedCreatorAuras);

        setFilterApplied(false);
    }

    const handleApplyAuraIncludeFilter = (number) => {
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

    const handleApplyAuraExcludeFilter = (number) => {
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

    const handleApplyCreatorAuraFilter = (number) => {
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
                    />
                </div>
            </div>
            <div className={`filters__aura-filters${showFilters ? '_show' : ''}`}>
                <div className="filters__aura-type-filter">
                    <div>Include:</div>
                    <ul>
                        {Object.entries(auraType).map(([key, value]) => (
                            <li className={selectedIncludeToFilter === value ? 'filter-selected' : ''} key={key}
                                onClick={() => handleApplyAuraIncludeFilter(value)}>{key}</li>
                        ))}
                    </ul>
                </div>
                <div className="filters__aura-type-filter">
                    <div>Exclude:</div>
                    <ul>
                        {Object.entries(auraType).map(([key, value]) => (
                            <li className={selectedExcludeFromFilter === value ? 'filter-selected' : ''} key={key}
                                onClick={() => handleApplyAuraExcludeFilter(value)}>{key}</li>
                        ))}
                    </ul>
                </div>
                <div className="filters__aura-type-filter">
                    <div>Creator:</div>
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