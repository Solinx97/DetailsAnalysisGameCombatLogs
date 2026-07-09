import { faEye, faPlus, faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type SetStateAction } from 'react';
import type { CombatPlayerAuraModel } from '../../types/CombatPlayerAuraModel';
import CombatAuraTargets from './CombatAuraTargets';
import { useGetCombatPlayerAurasByCombatIdQuery } from '../../api/GameLogs.api';
import Loading from '@/shared/components/Loading';
import CombatAuraFilters from './CombatAuraFilters';
import CombatAuraTimes from './CombatAuraTimes';

interface CombatAuraItemProps {
    onlyPinnedAuras: boolean;
    pinnedAuras: Map<string, CombatPlayerAuraModel[]>;
    setPinnedAuras: (value: SetStateAction<Map<string, CombatPlayerAuraModel[]>>) => void;
    combatId: number;
    combatPlayerId: number;
    searchAura: string;
    t: (key: string) => string;
}

const CombatAuraItem: React.FC<CombatAuraItemProps> = ({ onlyPinnedAuras, pinnedAuras, setPinnedAuras, combatId, combatPlayerId, searchAura, t }) => {
    const [auras, setAuras] = useState<Map<string, CombatPlayerAuraModel[]>>(new Map());
    const [defaultAuras, setDefaultAuras] = useState<Map<string, CombatPlayerAuraModel[]>>(new Map());
    const [selectedAura, setSelectedAura] = useState("");
    const [showTargets, setShowTargets] = useState(false);
    const [seeBuffs, setSeeBuffs] = useState(true);

    const { data: allAuras, isLoading } = useGetCombatPlayerAurasByCombatIdQuery({ combatId, combatPlayerId });

    useEffect(() => {
        if (!allAuras) {
            return;
        }

        getCreatorAurasMap(allAuras);
    }, [allAuras, combatPlayerId, searchAura]);

    useEffect(() => {
        if (searchAura.length === 0) {
            return;
        }

        getSearchMap(searchAura);
    }, [searchAura]);

    useEffect(() => {
        if (pinnedAuras.size === 0 || !onlyPinnedAuras) {
            setAuras(defaultAuras);
            return;
        }

        setAuras(pinnedAuras);
    }, [pinnedAuras, onlyPinnedAuras]);

    const getCreatorAurasMap = (auras: CombatPlayerAuraModel[]) => {
        const auraMap = new Map<string, CombatPlayerAuraModel[]>();

        auras?.forEach(aura => {
            if (auraMap.has(aura.name)) {
                const creatorAuras = auraMap.get(aura.name);
                if (creatorAuras) {
                    creatorAuras.push(aura);

                    auraMap.set(aura.name, creatorAuras);
                }
            } else {
                auraMap.set(aura.name, [aura]);
            }
        });

        setAuras(auraMap);
        setDefaultAuras(auraMap);
    }

    const getSearchMap = (prefix: string) => {
        const filteredMap = new Map<string, CombatPlayerAuraModel[]>();

        for (const [key, value] of defaultAuras) {
            if (key.startsWith(prefix)) {
                filteredMap.set(key, value);
            }
        }

        setAuras(filteredMap);
    }

    const handleSelectAura = (auraName: string) => {
        if (selectedAura === auraName && showTargets) {
            setShowTargets(false);
            setSelectedAura("");

            return;
        }

        setShowTargets(true);
        setSelectedAura(auraName);
    }

    const handlePinAura = (auraName: string, aura: CombatPlayerAuraModel[]) => {
        const contains = Array.from(pinnedAuras.keys()).filter(pin => pin === auraName).length > 0;
        if (contains) {
            return;
        }

        pinnedAuras.set(auraName, aura);
        setPinnedAuras(new Map(pinnedAuras));
    }

    if (isLoading) {
        return (
            <div>
                <Loading />
            </div>
        );
    }

    return (
        <div className="creator-auras">
            <div className="creator-auras__title">
                <div className="title" onClick={() => setSeeBuffs(prev => !prev)}>
                    <div>{t("AllBuffs")}</div>
                    {seeBuffs
                        ? <FontAwesomeIcon
                            icon={faArrowDown}
                            title={t("Hide")}
                        />
                        : <FontAwesomeIcon
                            icon={faArrowUp}
                            title={t("See")}
                        />
                    }
                </div>
                <CombatAuraTimes
                    defaultAuras={defaultAuras}
                    setAuras={setAuras}
                    t={t}
                />
                <CombatAuraFilters
                    combatPlayerId={combatPlayerId}
                    defaultAuras={defaultAuras}
                    auras={auras}
                    setAuras={setAuras}
                    t={t}
                />
            </div>
            {seeBuffs &&
                <ul className="creator-auras__content">
                    {Array.from(auras.entries()).map(([key, value]) => (
                        <li key={key} className="creator-auras details">
                            <ul className="details-collection">
                                <li className={`details-collection__spell${Array.from(pinnedAuras.keys()).includes(key) ? '' : '_ready'}`}
                                    key={`${key}-1`} onClick={() => Array.from(pinnedAuras.keys()).includes(key) ? null : handlePinAura(key, value)}>
                                    {!Array.from(pinnedAuras.keys()).includes(key) &&
                                        <FontAwesomeIcon
                                            icon={faPlus}
                                        />
                                    }
                                    <div>{key}</div>
                                </li>
                                <li key={`${key}-2`}>{value.length}</li>
                                <li key={`${key}-3`}>
                                    <div className={`btn-shadow ${selectedAura === key ? 'details-opened' : ''}`} onClick={() => handleSelectAura(key)}>
                                        <FontAwesomeIcon
                                            icon={faEye}
                                        />
                                        <div>{t("Targets")}</div>
                                    </div>
                                    {(showTargets && selectedAura === key) &&
                                        <CombatAuraTargets
                                            combatAuras={value}
                                        />
                                    }
                                </li>
                            </ul>
                        </li>
                    ))}
                </ul>
            }
        </div>
    );
}

export default CombatAuraItem;