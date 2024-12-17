import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatAurasByCombatIdQuery, useLazyGetCombatByIdQuery } from '../../../store/api/core/CombatParser.api';
import CombatAuraFilters from './CombatAuraFilters';
import CombatAuraItem from './CombatAuraItem';
import CombatAuraTimes from './CombatAuraTimes';

import "../../../styles/combatAuras.scss";

const CombatAuras = () => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const navigate = useNavigate();
    const location = useLocation();

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combat, setCombat] = useState(null);
    const [combatAuras, setCombatAuras] = useState([]);
    const [allCombatAuras, setAllCombatAuras] = useState([]);
    const [creators, setCreators] = useState([]);
    const [allCreators, setAllCreators] = useState([]);
    const [selectedCreatorAuras, setSelectedCreatorAuras] = useState([]);
    const [defaultSelectedCreatorAuras, setDefaultSelectedCreatorAuras] = useState([]);
    const [selectedCreator, setSelectedCreator] = useState("All");

    const [getCombatById] = useLazyGetCombatByIdQuery();
    const [getCombatAurasByCombatId] = useLazyGetCombatAurasByCombatIdQuery();

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const combatId = +searchParams.get("combat");
        const combatLogId = +searchParams.get("combatLog");

        setCombatId(combatId);
        setCombatLogId(combatLogId);
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
        const creators = [];

        combatAuras.forEach(aura => {
            if (!uniqueCreators.has(aura.creator)) {
                uniqueCreators.add(aura.creator);
                creators.push(aura);
            }
        });

        setAllCreators(creators);
        setCreators(creators);

        initSelectedCreatorCombatAuras("All");
    }

    const initSelectedCreatorCombatAuras = (creator) => {
        const auras = [];

        allCombatAuras.forEach(aura => {
            if (creator === "All" || aura.creator === creator) {
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
            <div className="details-specifical-combat__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectCombat")}</div>
                </div>
            </div>
            <div className="creators__select-creator">
                <select className="form-control" value={selectedCreator} onChange={(e) => handleSelectCreator(e.target.value)}>
                    <option key="-1" value="All">{t("All")}</option>
                    {creators.map((creator, index) => (
                        <option key={index} value={creator.creator}>{creator.creator}</option>
                    ))}
                </select>
                <CombatAuraFilters
                    setCreators={setCreators}
                    selectedCreator={selectedCreator}
                    handleSelectCreator={handleSelectCreator}
                    allCreators={allCreators}
                    setSelectedCreatorAuras={setSelectedCreatorAuras}
                    getAuraCreators={getAuraCreators}
                    defaultSelectedCreatorAuras={defaultSelectedCreatorAuras}
                    t={t}
                />
                <CombatAuraTimes
                    setSelectedCreatorAuras={setSelectedCreatorAuras}
                    defaultSelectedCreatorAuras={defaultSelectedCreatorAuras}
                    t={t}
                />
            </div>
            {combatAuras.length > 0 &&
                <CombatAuraItem
                    selectedCreatorAuras={selectedCreatorAuras}
                    t={t}
                />
            }
        </div>
    )
}

export default CombatAuras;