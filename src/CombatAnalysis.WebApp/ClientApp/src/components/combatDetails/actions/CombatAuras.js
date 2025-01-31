import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatAurasByCombatIdQuery, useLazyGetCombatByIdQuery } from '../../../store/api/core/CombatParser.api';
import CombatAuraFilters from './CombatAuraFilters';
import CombatAuraItem from './CombatAuraItem';
import CombatAuraTimes from './CombatAuraTimes';

import "../../../styles/combatAuras.scss";

const CombatAuras = () => {
    const { t } = useTranslation("combatDetails/auras");

    const navigate = useNavigate();
    const location = useLocation();

    const searchRef = useRef(null);

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combat, setCombat] = useState(null);
    const [combatAuras, setCombatAuras] = useState([]);
    const [allCombatAuras, setAllCombatAuras] = useState([]);
    const [creators, setCreators] = useState([]);
    const [allCreators, setAllCreators] = useState([]);
    const [selectedCreatorAuras, setSelectedCreatorAuras] = useState([]);
    const [defaultSelectedCreatorAuras, setDefaultSelectedCreatorAuras] = useState([]);
    const [selectedCreator, setSelectedCreator] = useState("");
    const [pinnedAuras, setPinnedAuras] = useState([]);
    const [defaultWhenPinnedAuras, setDefaultPinnedAuras] = useState([]);
    const [showSearch, setShowSearch] = useState(false);

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
        handleSelectCreator("All");
    }, [combatAuras]);

    useEffect(() => {
        handleCleanSearch();
    }, [showSearch]);

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
        setDefaultPinnedAuras(auras);
    }

    const handleSelectCreator = (creator) => {
        setSelectedCreator(creator);
        initSelectedCreatorCombatAuras(creator);
    }

    const handleRemovePinAura = (aura) => {
        const pinned = Array.from(pinnedAuras).filter(x => x !== aura);

        setPinnedAuras(pinned);
    }

    const handleCleanSearch = () => {
        if (searchRef.current !== null) {
            searchRef.current.value = "";
        }

        setSelectedCreatorAuras(defaultSelectedCreatorAuras);
        setPinnedAuras(Array.from(pinnedAuras));
    }

    const handleSearchAura = (e) => {
        let selectedAuras = [];
        const searchAura = e.target.value;
        const defaultAura = pinnedAuras.length > 0 ? defaultWhenPinnedAuras : defaultSelectedCreatorAuras;

        if (searchAura === "") {
            selectedAuras = Array.from(defaultAura);
        }
        else {
            selectedAuras = Array.from(defaultAura).filter(aura => removeQuotes(aura.name).toLowerCase().startsWith(searchAura.toLowerCase()));
        }

        setSelectedCreatorAuras(selectedAuras);
    }

    const removeQuotes = (str) => {
        const newStr = str.slice(1, -1);

        return newStr;
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
                <div className="btn-shadow" onClick={() => setShowSearch(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={showSearch ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    />
                    <div>{t("Search")}</div>
                </div>
            </div>
            <div>{t("Creator")}</div>
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
            {showSearch &&
                <div className="mb-3 search">
                    <label htmlFor="inputAura" className="form-label">{t("Search")}</label>
                    <div className="search__aura">
                        <input type="text" className="form-control" placeholder={t("TypeAuraName")} id="inputAura" ref={searchRef} onChange={handleSearchAura} />
                        <FontAwesomeIcon
                            icon={faXmark}
                            title={t("Clean")}
                            onClick={handleCleanSearch}
                        />
                    </div>
                </div>
            }
            {pinnedAuras.length > 0 &&
                <ul className="pinned-auras">
                    {pinnedAuras.map((aura, index) => (
                        <li key={index} onClick={() => handleRemovePinAura(aura)}>
                            <div>{removeQuotes(aura.name)}</div>
                        </li>
                    ))}
                </ul>
            }
            {combatAuras.length > 0 &&
                <CombatAuraItem
                    selectedCreatorAuras={selectedCreatorAuras}
                    pinnedAuras={pinnedAuras}
                    setPinnedAuras={setPinnedAuras}
                    removeQuotes={removeQuotes}
                    selectedCreator={selectedCreator}
                    setDefaultAurasWhenPin={setDefaultPinnedAuras}
                    t={t}
                />
            }
        </div>
    )
}

export default CombatAuras;