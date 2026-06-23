import { faPlus, faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState, type Dispatch, type SetStateAction } from 'react';
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
    combatPlayerId: number;
    defaultAuras: Map<string, CombatPlayerAuraModel[]>;
    auras: Map<string, CombatPlayerAuraModel[]>;
    setAuras: Dispatch<SetStateAction<Map<string, CombatPlayerAuraModel[]>>>;
    t: (key: string) => string;
}

const CombatAuraFilters: React.FC<CombatAuraFiltersProps> = ({ combatPlayerId, defaultAuras, auras, setAuras, t }) => {
    const [selectedIncludeToFilter, setSelectedIncludeToFilter] = useState(-1);
    const [selectedExcludeFromFilter, setSelectedExcludeFromFilter] = useState(-1);
    const [selectedAuraCreatorType, setSelectedAuraCreatorType] = useState(-1);
    const [filterApplied, setFilterApplied] = useState(false);

    const [showFilters, setShowFilters] = useState(false);

    useEffect(() => {
        setShowFilters(false);
        restoreFiltersToDefault();
    }, [combatPlayerId]);

    useEffect(() => {
        if (selectedIncludeToFilter === -1) {
            setAuras(defaultAuras);
            return;
        }

        const applyFilterAuraTypeAsync = async () => {
            await applyFilterAuraType(selectedIncludeToFilter, true);
        }

        applyFilterAuraTypeAsync();
    }, [selectedIncludeToFilter]);

    useEffect(() => {
        if (selectedExcludeFromFilter === -1) {
            setAuras(defaultAuras);
            return;
        }

        const applyFilterAuraTypeAsync = async () => {
            await applyFilterAuraType(selectedExcludeFromFilter, false);
        }

        applyFilterAuraTypeAsync();
    }, [selectedExcludeFromFilter]);

    useEffect(() => {
        if (selectedAuraCreatorType === -1) {
            setAuras(defaultAuras);
            return;
        }

        const applyFilterCreatorAuraTypeAsync = async () => {
            await applyFilterCreatorAuraType(selectedAuraCreatorType);
        }

        applyFilterCreatorAuraTypeAsync();
    }, [selectedAuraCreatorType]);

    const applyFilterCreatorAuraType = (number: number): void => {
        const filteredAuras = new Map<string, CombatPlayerAuraModel[]>();

        auras.forEach((value, key) => {
            const condition = value[0].auraCreatorType === number;

            if (condition && (value[0].combatPlayerId === combatPlayerId || combatPlayerId === 0)) {
                filteredAuras.set(key, value);
            }
        });

        setAuras(filteredAuras);
        setFilterApplied(true);
    }

    const applyFilterAuraType = async (auraType: number, include: boolean) => {
        const filteredAuras = new Map<string, CombatPlayerAuraModel[]>();

        auras.forEach((value, key) => {
            const condition = include
                ? value[0].auraType === auraType
                : value[0].auraType !== auraType;

            if ((auraType < 0 || condition) && (value[0].combatPlayerId === combatPlayerId || combatPlayerId === 0)) {
                filteredAuras.set(key, value);
            }
        });

        setAuras(filteredAuras);
        setFilterApplied(true);
    }

    const restoreFiltersToDefault = (): void => {
        setSelectedIncludeToFilter(-1);
        setSelectedExcludeFromFilter(-1);
        setSelectedAuraCreatorType(-1);
        setFilterApplied(false);
    }

    const handleApplyAuraIncludeFilter = (number: number) => {
        setSelectedIncludeToFilter(prev => prev == -1 ? number : -1);
    }

    const handleApplyAuraExcludeFilter = (number: number) => {
        setSelectedExcludeFromFilter(prev => prev == -1 ? number : -1);
    }

    const handleApplyCreatorAuraFilter = (number: number): void => {
        setSelectedAuraCreatorType(prev => prev == -1 ? number : -1);
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