import { faEye, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import CombatAuraTargets from './CombatAuraTargets';

const CombatAuraItem = ({ selectedCreatorAuras, pinnedAuras, setPinnedAuras, selectedCreator, setDefaultAurasWhenPin, removeQuotes, t }) => {
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
        if (pinnedAuras.length === 0) {
            setAuras(defaultAuras);

            return;
        }

        const auraMap = makeCreatorAurasMap();
        updateAuras(auraMap);
    }, [pinnedAuras]);

    const makeCreatorAurasMap = () => {
        const auraMap = new Map();

        selectedCreatorAuras?.forEach(aura => {
            if (auraMap.has(aura.name)) {
                const creatorAuras = auraMap.get(aura.name);
                creatorAuras.push(aura);

                auraMap.set(aura.name, creatorAuras);
            } else {
                auraMap.set(aura.name, [aura]);
            }
        });

        setAuras(auraMap);

        return auraMap;
    }

    const updateAuras = (newAuras) => {
        const auraMap = new Map();
        let makeDefaultAuras = [];

        pinnedAuras?.forEach(pinnedAura => {
            pinnedAura.time?.forEach(pinnedAuraTime => {
                newAuras?.forEach((value, key) => {
                    const times = value.map(aura => ({ start: aura.startTime, finish: aura.finishTime, data: aura }));

                    const included = times.filter(auraTime => auraTime.start <= pinnedAuraTime.start && auraTime.finish <= pinnedAuraTime.finish);

                    const isContains = included.length > 0;
                    if (isContains) {
                        const newData = included.map(aura => aura.data);
                        makeDefaultAuras = makeDefaultAuras.concat(newData);
                        auraMap.set(key, newData);
                    }
                });
            });
        });

        setAuras(auraMap);
        setDefaultAurasWhenPin(makeDefaultAuras);
    }

    const handleSelectAura = (auraName) => {
        if (selectedAura === auraName && showTargets) {
            setShowTargets(false);
            setSelectedAura("");

            return;
        }

        setShowTargets(true);
        setSelectedAura(auraName);
    }

    const handlePinAura = (auraName, aura) => {
        const contains = pinnedAuras.filter(pin => pin.name === auraName).length > 0;
        if (contains) {
            return;
        }

        const pinned = Array.from(pinnedAuras);
        pinned.push({
            name: auraName,
            time: aura.map(aura => ({ start: aura.startTime, finish: aura.finishTime })),
        });

        setPinnedAuras(pinned);
    }

    return (
        <ul className="creator-auras">
            {Array.from(auras.entries()).map(([key, value]) => (
                <li key={key} className="creator-auras__details">
                    <ul className="details-collection">
                        <li className={`details-collection__spell${pinnedAuras.includes(aura => aura.name === key) ? '' : '_ready'}`}
                            onClick={() => pinnedAuras.includes(key) ? null : handlePinAura(key, value)}>
                            {!pinnedAuras.includes(key) &&
                                <FontAwesomeIcon
                                    icon={faPlus}
                                />
                            }
                            <div>{removeQuotes(key)}</div>
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