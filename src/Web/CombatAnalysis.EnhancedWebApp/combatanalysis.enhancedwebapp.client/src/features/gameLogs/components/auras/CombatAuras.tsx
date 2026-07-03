import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState, type ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyGetCombatByIdQuery, useLazyGetCombatPlayersByCombatIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerAuraModel } from '../../types/CombatPlayerAuraModel';
import type { CombatModel } from '../../types/CombatModel';
import CombatAuraItem from './CombatAuraItem';
import CombatPreAuraItem from './CombatPreAuraItem';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';

import './CombatAuras.scss';

const CombatAuras: React.FC = () => {
    const { t } = useTranslation("combatDetails/auras");

    const navigate = useNavigate();
    const location = useLocation();

    const searchRef = useRef<HTMLInputElement | null>(null);

    const [combatId, setCombatId] = useState<number>(0);
    const [combatLogId, setCombatLogId] = useState<number>(0);
    const [combat, setCombat] = useState<CombatModel | null>(null);
    const [combatPlayers, setCombatPlayers] = useState<CombatPlayerModel[]>([]);
    const [selectedCombatPlayerId, setSelectedCombatPlayerId] = useState<number>(0);
    const [pinnedAuras, setPinnedAuras] = useState<Map<string, CombatPlayerAuraModel[]>>(new Map());
    const [onlyPinnedAuras, setOnlyPinnedAuras] = useState<boolean>(false);
    const [showSearch, setShowSearch] = useState<boolean>(false);
    const [searchAura, setSearchAura] = useState<string>("");

    const [getCombatById] = useLazyGetCombatByIdQuery();
    const [getCombatPlayersByCombatId] = useLazyGetCombatPlayersByCombatIdQuery();

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

        const loadData = async () => {
            try {
                const [combat, combatPLayers] = await Promise.all([
                    getCombatById(combatId).unwrap(),
                    getCombatPlayersByCombatId(combatId).unwrap(),
                ]);

                setCombat(combat);
                setCombatPlayers(combatPLayers);
            } catch (e) {
                console.error(e);
            }
        };

        loadData();
    }, [combatId]);

    useEffect(() => {
        handleCleanSearch();
    }, [showSearch]);

    const handleSelectCreator = (combatPlayerId: string): void => {
        const selected = Number.parseInt(combatPlayerId);
        setSelectedCombatPlayerId(selected);
    }

    const handleRemovePinAura = (auraName: string): void => {
        const pinned = new Map(pinnedAuras);
        pinned.delete(auraName);

        setPinnedAuras(pinned);
    }

    const handleCleanSearch = (): void => {
        if (searchRef.current !== null) {
            searchRef.current.value = "";
        }

        setPinnedAuras(new Map(pinnedAuras));
    }

    const handleSearchAura = (e: ChangeEvent<HTMLInputElement> | undefined): void => {
        const searchAura = e?.target.value;
        setSearchAura(searchAura ?? "");
    }

    const usePinnedAurasHandler = (e: ChangeEvent<HTMLInputElement> | undefined): void => {
        const checked = e?.target.checked;
        setOnlyPinnedAuras(checked || false);
    }

    if (!combat) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="auras">
            <div className="auras__navigate">
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
            <div className="auras__select-creator">
                <select className="form-control" value={selectedCombatPlayerId} onChange={(e) => handleSelectCreator(e.target.value)}>
                    <option key="-1" value="0">{t("All")}</option>
                    {combatPlayers.map((combatPlayer, index) => (
                        <option key={index} value={combatPlayer.id}>{combatPlayer.player.username}</option>
                    ))}
                </select>
                <div className="mb-3 form-check other-filters">
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
            <CombatPreAuraItem
                combatPlayerId={selectedCombatPlayerId}
                combatId={combatId}
            />
            <CombatAuraItem
                onlyPinnedAuras={onlyPinnedAuras}
                pinnedAuras={pinnedAuras}
                setPinnedAuras={setPinnedAuras}
                combatId={combatId}
                combatPlayerId={selectedCombatPlayerId}
                searchAura={searchAura}
                t={t}
            />
        </div>
    )
}

export default CombatAuras;