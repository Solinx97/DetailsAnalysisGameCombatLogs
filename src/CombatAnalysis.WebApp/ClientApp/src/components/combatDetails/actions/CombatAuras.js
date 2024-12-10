import { faPlus, faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCombatAurasByCombatIdQuery, useLazyGetCombatByIdQuery } from '../../../store/api/CombatParserApi';
import CombatAuraItem from './CombatAuraItem';

import "../../../styles/combatAuras.scss";

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

const CombatAuras = () => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const [combatId, setCombatId] = useState(0);
    const [showFilters, setShowFilters] = useState(false);
    const [combat, setCombat] = useState(null);
    const [combatAuras, setCombatAuras] = useState([]);
    const [allCombatAuras, setAllCombatAuras] = useState([]);
    const [creators, setCreators] = useState([]);
    const [allCreators, setAllCreators] = useState([]);
    const [creatorCombatAuras, setCreatorCombatAuras] = useState(new Map());
    const [selectedCreator, setSelectedCreator] = useState("All");

    const [getCombatById] = useLazyGetCombatByIdQuery();
    const [getCombatAurasByCombatId] = useLazyGetCombatAurasByCombatIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const combatId = +queryParams.get("combat");
        setCombatId(combatId);
    }, []);

    useEffect(() => {
        if (combatId < 1) {
            return;
        }

        const getCombat = async () => {
            const response = await getCombatById(combatId);
            if (response.data !== undefined) {
                setCombat(response.data);
            }
        }

        getCombat();
    }, [combatId]);

    useEffect(() => {
        if (combat === null) {
            return;
        }

        const getCombatAuras = async () => {
            const response = await getCombatAurasByCombatId(combat?.id);
            if (response.data !== undefined) {
                setCombatAuras(response.data);
                setAllCombatAuras(response.data);
            }
        }

        getCombatAuras();
    }, [combat]);


    useEffect(() => {
        if (combatAuras.length === 0) {
            return;
        }

        getAuraCreators();
    }, [combatAuras]);

    useEffect(() => {
        if (creators.length === 0) {
            return;
        }

        getUniqueCombatAuras(-1);
    }, [creators]);

    const getAuraCreators = () => {
        const uniqueCreators = new Set();
        const creators = [{ creator: "None" }];

        combatAuras.forEach(aura => {
            if (!uniqueCreators.has(aura.creator)) {
                uniqueCreators.add(aura.creator);
                creators.push(aura);
            }
        });

        setAllCreators(creators);
        setCreators(creators);
    }

    const getUniqueCombatAuras = (auraType) => {
        const auraMap = prepareCombatAuraMap(creators);

        allCombatAuras.forEach(aura => {
            if (auraType < 0 || aura.auraType === auraType) {
                if (auraMap.has(aura.creator)) {
                    const creatorAuras = auraMap.get(aura.creator);
                    creatorAuras.push(aura);

                    auraMap.set(aura.creator, creatorAuras);
                }
            }
        });

        setCreatorCombatAuras(auraMap);
    }

    const prepareCombatAuraMap = (creators) => {
        const auraMap = new Map();
        auraMap.set("None", combatAuras);

        creators.forEach(creator => {
            auraMap.set(creator.creator, []);
        });

        setCreatorCombatAuras(auraMap);

        return auraMap;
    }

    const applyCreatorTypeFilter = (number) => {
        const newCreators = [{ creator: "None" }];
        const filteredCreators = allCreators.filter(creator => creator.auraCreatorType === number);

        setCreators(newCreators.concat(filteredCreators));
        setSelectedCreator({ creator: "None" });
    }

    const restartFiltersToDefault = () => {
        setCreators(allCreators);
        getUniqueCombatAuras(-1);
    }

    if (combat === null || creatorCombatAuras.size === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <div>
            <div className="creators">
                <select className="form-control" value={selectedCreator} onChange={(e) => setSelectedCreator(e.target.value)}>
                    {creators.map((creator, index) => (
                        <option key={index} value={creator.creator}>{creator.creator}</option>
                    ))}
                </select>
                <div className="btn-shadow select-logs" onClick={() => setShowFilters(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faPlus}
                    />
                    <div>{t("Filters")}</div>
                </div>
                <div className="creators__clear">
                    <FontAwesomeIcon
                        icon={faRotate}
                        onClick={restartFiltersToDefault}
                    />
                </div>
                <div className={`creators__aura-filters${showFilters ? '_show' : ''}`}>
                    <div className="creators__aura-type-filter">
                        <div>Aura types:</div>
                        <ul>
                            {Object.entries(auraType).map(([key, value]) => (
                                <li key={key} onClick={() => getUniqueCombatAuras(value)}>{key}</li>
                            ))}
                        </ul>
                    </div>
                    <div className="creators__aura-type-filter">
                        <div>Aura creator types:</div>
                        <ul>
                            {Object.entries(auraCreatorType).map(([key, value]) => (
                                <li key={key} onClick={() => applyCreatorTypeFilter(value)}>{key}</li>
                            ))}
                        </ul>
                    </div>
                </div>
            </div>
            {combatAuras.length > 0 &&
                <CombatAuraItem
                    combatAuras={creatorCombatAuras.get(selectedCreator) === undefined ? combatAuras : creatorCombatAuras.get(selectedCreator)}
                />
            }
        </div>
    )
}

export default CombatAuras;