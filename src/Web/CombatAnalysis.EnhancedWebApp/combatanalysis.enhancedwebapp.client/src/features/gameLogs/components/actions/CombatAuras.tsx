import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState, type ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayerAurasByCombatIdQuery, useLazyGetCombatByIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerAuraModel } from '../../types/CombatPlayerAuraModel';
import type { CombatModel } from '../../types/CombatModel';
import CombatAuraFilters from './CombatAuraFilters';
import CombatAuraItem from './CombatAuraItem';
import CombatAuraTimes from './CombatAuraTimes';

import './CombatAuras.scss';

const CombatAuras: React.FC = () => {
    const { t } = useTranslation("combatDetails/auras");

    const navigate = useNavigate();
    const location = useLocation();

    const searchRef = useRef<HTMLInputElement | null>(null);

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combat, setCombat] = useState<CombatModel | null>(null);
    const [combatAuras, setCombatAuras] = useState<CombatPlayerAuraModel[]>([]);
    const [allCombatAuras, setAllCombatAuras] = useState<CombatPlayerAuraModel[]>([]);
    const [creatorsAuras, setCreatorsAuras] = useState<CombatPlayerAuraModel[]>([]);
    const [allCreators, setAllCreators] = useState<CombatPlayerAuraModel[]>([]);
    const [selectedCreatorAuras, setSelectedCreatorAuras] = useState<CombatPlayerAuraModel[]>([]);
    const [defaultSelectedCreatorAuras, setDefaultSelectedCreatorAuras] = useState<CombatPlayerAuraModel[]>([]);
    const [selectedCreator, setSelectedCreator] = useState("");
    const [pinnedAuras, setPinnedAuras] = useState<Map<string, CombatPlayerAuraModel[]>>(new Map());
    const [defaultWhenPinnedAuras, setDefaultPinnedAuras] = useState<CombatPlayerAuraModel[]>([]);
    const [showSearch, setShowSearch] = useState(false);
    const [onlyPinnedAuras, setOnlyPinnedAuras] = useState(false);

    const [getCombatById] = useLazyGetCombatByIdQuery();
    const [getCombatAurasByCombatId] = useLazyGetCombatPlayerAurasByCombatIdQuery();

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const combatId = parseInt(searchParams.get("combat") ?? "1");
        const combatLogId = parseInt(searchParams.get("combatLog") ?? "1");

        setCombatId(combatId);
        setCombatLogId(combatLogId);
    }, []);

    useEffect(() => {
        if (combatId < 1) {
            return;
        }

        const getCombat = async (): Promise<void> => {
            try {
                const result = await getCombatById(combatId).unwrap();
                setCombat(result);
            } catch (e) {
                console.error(e);
            }
        }

        getCombat();
    }, [combatId]);

    useEffect(() => {
        if (combat === null) {
            return;
        }

        const getCombatAuras = async () => {
            try {
                const result = await getCombatAurasByCombatId(combat?.id).unwrap();
                setCombatAuras(result);
                setAllCombatAuras(result);
            } catch (e) {
                console.error(e);
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

    useEffect(() => {
        if (onlyPinnedAuras) {
            const pinnedAurasNames = Array.from(pinnedAuras.keys());
            const filteredAuras = combatAuras.filter(aura => pinnedAurasNames.includes(aura.name));

            setSelectedCreatorAuras(filteredAuras);
            setDefaultSelectedCreatorAuras(filteredAuras);
        }
        else {
            initSelectedCreatorCombatAuras(selectedCreator);
        }
    }, [onlyPinnedAuras]);

    const getAuraCreators = (): void => {
        const uniqueCreators = new Set();
        const creators = new Array<CombatPlayerAuraModel>();

        combatAuras.forEach(aura => {
            if (!uniqueCreators.has(aura.creator)) {
                uniqueCreators.add(aura.creator);
                creators.push(aura);
            }
        });

        setAllCreators(creators);
        setCreatorsAuras(creators);

        initSelectedCreatorCombatAuras("All");
    }

    const initSelectedCreatorCombatAuras = (creator: string): void => {
        const availableAuras = onlyPinnedAuras ? Array.from(allCombatAuras.filter(aura => pinnedAuras.has(aura.name))) : Array.from(allCombatAuras);

        const auras = availableAuras.filter(
            value => creator === "All" || value.creator === creator
        );

        setSelectedCreatorAuras(auras);
        setDefaultSelectedCreatorAuras(auras);
        setDefaultPinnedAuras(auras);
    }

    const handleSelectCreator = (creator: string): void => {
        setSelectedCreator(creator);
        initSelectedCreatorCombatAuras(creator);
    }

    const handleRemovePinAura = (auraName: string) => {
        const pinned = new Map(pinnedAuras);
        pinned.delete(auraName);

        setPinnedAuras(pinned);
    }

    const handleCleanSearch = () => {
        if (searchRef.current !== null) {
            searchRef.current.value = "";
        }

        setSelectedCreatorAuras(defaultSelectedCreatorAuras);
        setPinnedAuras(new Map(pinnedAuras));
    }

    const handleSearchAura = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        let selectedAuras = [];
        const searchAura = e?.target.value;
        const defaultAura = pinnedAuras.size > 0 ? defaultWhenPinnedAuras : defaultSelectedCreatorAuras;

        if (!searchAura) {
            return;
        }

        if (searchAura === "") {
            selectedAuras = Array.from(defaultAura);
        }
        else {
            selectedAuras = Array.from(defaultAura).filter(aura => removeQuotes(aura.name).toLowerCase().startsWith(searchAura.toLowerCase()));
        }

        setSelectedCreatorAuras(selectedAuras);
    }

    const removeQuotes = (str: string): string => {
        const newStr = str?.slice(0, -1);

        return newStr;
    }

    const usePinnedAurasHandler = (e: ChangeEvent<HTMLInputElement> | undefined): void => {
        const checked = e?.target.checked;
        setOnlyPinnedAuras(checked || false);
    }

    if (combat === null) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="creators">
            <div className="creators__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectCombat")}</div>
                </div>
                <div className={`btn-shadow ${showSearch ? 'active' : ''}`} onClick={() => setShowSearch(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={showSearch ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    />
                    <div>{t("Search")}</div>
                </div>
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
            <div>{t("Creator")}</div>
            <div className="creators__select-creator">
                <select className="form-control" value={selectedCreator} onChange={(e) => handleSelectCreator(e.target.value)}>
                    <option key="-1" value="All">{t("All")}</option>
                    {creatorsAuras.map((creatorsAura, index) => (
                        <option key={index} value={creatorsAura.creator}>{creatorsAura.creator}</option>
                    ))}
                </select>
                <CombatAuraFilters
                    setCreators={setCreatorsAuras}
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
                <div className="mb-3 form-check">
                    <input type="checkbox" className="form-check-input" id="exampleCheck1" defaultChecked={false} onChange={usePinnedAurasHandler} />
                    <label className="form-check-label" htmlFor="exampleCheck1">{t("SeeOnlyPinnedAuras")}</label>
                </div>
            </div>
            {pinnedAuras.size > 0 &&
                <ul className="pinned-auras">
                    {Array.from(pinnedAuras.entries()).map(([key]) => (
                        <li key={key} onClick={() => handleRemovePinAura(key)}>
                            <div>{key}</div>
                        </li>
                    ))}
                </ul>
            }
            {combatAuras.length > 0 &&
                <CombatAuraItem
                    selectedCreatorAuras={selectedCreatorAuras}
                    pinnedAuras={pinnedAuras}
                    setPinnedAuras={setPinnedAuras}
                    selectedCreator={selectedCreator}
                    t={t}
                />
            }
        </div>
    )
}

export default CombatAuras;