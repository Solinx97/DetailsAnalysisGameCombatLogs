import { faEye, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type SetStateAction } from 'react';
import type { CombatAuraModel } from '../../types/CombatAuraModel';
import CombatAuraTargets from './CombatAuraTargets';

interface CombatAuraItemProps {
    selectedCreatorAuras: CombatAuraModel[];
    pinnedAuras: Map<string, CombatAuraModel[]>;
    setPinnedAuras: (value: SetStateAction<Map<string, CombatAuraModel[]>>) => void;
    selectedCreator: string;
    t: (key: string) => string;
}

const CombatAuraItem: React.FC<CombatAuraItemProps> = ({ selectedCreatorAuras, pinnedAuras, setPinnedAuras, selectedCreator, t }) => {
    const [auras, setAuras] = useState(new Map());
    const [defaultAuras, setDefaultAuras] = useState(new Map());
    const [selectedAura, setSelectedAura] = useState("");
    const [showTargets, setShowTargets] = useState(false);

    useEffect(() => {
        makeCreatorAurasMap();
    }, [selectedCreatorAuras]);

    useEffect(() => {
        if (selectedCreatorAuras.length === 0) {
            return;
        }

        const auraMap = makeCreatorAurasMap();
        setDefaultAuras(auraMap);
    }, [selectedCreator]);

    useEffect(() => {
        if (Array.from(pinnedAuras.keys()).length === 0) {
            setAuras(defaultAuras);

            return;
        }
    }, [pinnedAuras]);

    const makeCreatorAurasMap = (): Map<string, CombatAuraModel[]> => {
        const auraMap = new Map<string, CombatAuraModel[]>();

        selectedCreatorAuras?.forEach(aura => {
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

        return auraMap;
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

    const handlePinAura = (auraName: string, aura: CombatAuraModel) => {
        const contains = Array.from(pinnedAuras.keys()).filter(pin => pin === auraName).length > 0;
        if (contains) {
            return;
        }

        pinnedAuras.set(auraName, [aura]);
        setPinnedAuras(new Map(pinnedAuras));
    }

    return (
        <ul className="creator-auras">
            {Array.from(auras.entries()).map(([key, value]) => (
                <li key={key} className="creator-auras__details">
                    <ul className="details-collection">
                        <li className={`details-collection__spell${Array.from(pinnedAuras.keys()).includes(key) ? '' : '_ready'}`}
                            onClick={() => Array.from(pinnedAuras.keys()).includes(key) ? null : handlePinAura(key, value)}>
                            {!Array.from(pinnedAuras.keys()).includes(key) &&
                                <FontAwesomeIcon
                                    icon={faPlus}
                                />
                            }
                            <div>{key}</div>
                        </li>
                        <li>{value.length}</li>
                        <li>
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
    );
}

export default CombatAuraItem;