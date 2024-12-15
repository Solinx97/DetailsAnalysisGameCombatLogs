import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCombatAurasByCombatIdQuery, useLazyGetCombatByIdQuery } from '../../../store/api/core/CombatParser.api';
import CombatAuraFilters from './CombatAuraFilters';
import CombatAuraItem from './CombatAuraItem';
import CombatAuraTimes from './CombatAuraTimes';

import "../../../styles/combatAuras.scss";

const CombatAuras = () => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const [combatId, setCombatId] = useState(0);
    const [combat, setCombat] = useState(null);
    const [combatAuras, setCombatAuras] = useState([]);
    const [allCombatAuras, setAllCombatAuras] = useState([]);
    const [creators, setCreators] = useState([]);
    const [allCreators, setAllCreators] = useState([]);
    const [selectedCreatorAuras, setSelectedCreatorAuras] = useState([]);
    const [defaultSelectedCreatorAuras, setDefaultSelectedCreatorAuras] = useState([]);
    const [selectedCreator, setSelectedCreator] = useState("None");

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

    const initSelectedCreatorCombatAuras = (creator) => {
        const auras = [];

        allCombatAuras.forEach(aura => {
            if (aura.creator === creator) {
                auras.push(aura);
            }
        });

        setSelectedCreatorAuras(auras);
        setDefaultSelectedCreatorAuras(auras);
    }

    const handleSelectCreator = (creator) => {
        setSelectedCreator(creator);
        initSelectedCreatorCombatAuras(creator);
    }

    if (combat === null) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="creators">
            <div className="creators__select-creator">
                <select className="form-control" value={selectedCreator} onChange={(e) => handleSelectCreator(e.target.value)}>
                    {creators.map((creator, index) => (
                        <option key={index} value={creator.creator}>{creator.creator}</option>
                    ))}
                </select>
                <CombatAuraFilters
                    t={t}
                    setCreators={setCreators}
                    selectedCreator={selectedCreator}
                    setSelectedCreator={setSelectedCreator}
                    allCreators={allCreators}
                    setSelectedCreatorAuras={setSelectedCreatorAuras}
                    getAuraCreators={getAuraCreators}
                    defaultSelectedCreatorAuras={defaultSelectedCreatorAuras}
                />
                <CombatAuraTimes
                    t={t}
                    setSelectedCreatorAuras={setSelectedCreatorAuras}
                    defaultSelectedCreatorAuras={defaultSelectedCreatorAuras}
                />
            </div>
            {combatAuras.length > 0 &&
                <CombatAuraItem
                    t={t}
                    selectedCreatorAuras={selectedCreatorAuras}
                    defaultSelectedCreatorAuras={defaultSelectedCreatorAuras}
                />
            }
        </div>
    )
}

export default CombatAuras;