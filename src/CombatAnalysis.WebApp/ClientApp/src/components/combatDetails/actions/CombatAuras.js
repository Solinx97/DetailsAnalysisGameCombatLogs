import React, { useEffect, useState } from 'react';
import { useLazyGetCombatAurasByCombatIdQuery, useLazyGetCombatByIdQuery } from '../../../store/api/CombatParserApi';
import CombatAuraItem from './CombatAuraItem';

import "../../../styles/combatAuras.scss";

const auraTypes = {
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

const CombatAuras = () => {
    const [combatId, setCombatId] = useState(0);
    const [combat, setCombat] = useState(null);
    const [combatAuras, setCombatAuras] = useState([]);
    const [creators, setCreators] = useState([]);
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
            }
        }

        getCombatAuras();
    }, [combat]);


    useEffect(() => {
        if (combatAuras.length === 0) {
            return;
        }

        getAuraCreators();
        getUniqueCombatAuras();
    }, [combatAuras]);

    const getAuraCreators = () => {
        const uniqueCreators = new Set();
        const creators = ["All"];

        combatAuras.forEach(aura => {
            if (!uniqueCreators.has(aura.creator)) {
                uniqueCreators.add(aura.creator);
                creators.push(aura.creator);
            }
        });

        setCreators(creators);
    }

    const getUniqueCombatAuras = () => {
        const auraMap = new Map();
        auraMap.set("All", combatAuras);

        combatAuras.forEach(aura => {
            if (auraMap.has(aura.creator)) {
                const creatorAuras = auraMap.get(aura.creator);
                creatorAuras.push(aura);

                auraMap.set(aura.creator, creatorAuras);
            } else {
                auraMap.set(aura.creator, [aura]);
            }
        });

        setCreatorCombatAuras(auraMap);
    }

    if (combat === null || creators.length === 0 || creatorCombatAuras.size === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <div>
            <select value={selectedCreator} onChange={(e) => setSelectedCreator(e.target.value)}>
                {creators.map((creator, index) => (
                    <option key={index + 1} value={creator}>{creator}</option>
                ))}
            </select>
            <CombatAuraItem
                combatAuras={creatorCombatAuras.get(selectedCreator)}
            />
        </div>
    )
}

export default CombatAuras;