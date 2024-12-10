import { faEye } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import CombatAuraTargets from './CombatAuraTargets';

const CombatAuraItem = ({ t, selectedCreatorAuras }) => {
    const [informationSelectedCreatorAuras, setInformationSelectedCreatorAuras] = useState(new Map());
    const [selectedAura, setSelectedAura] = useState("");
    const [showTargets, setShowTargets] = useState(false);

    useEffect(() => {
        getCreatorAurasCount();
    }, [selectedCreatorAuras]);

    const getCreatorAurasCount = () => {
        const auraMap = new Map();

        selectedCreatorAuras?.forEach(aura => {
            if (auraMap.has(aura.name)) {
                const creatorAurasCount = auraMap.get(aura.name).count;
                const creatorAuras = auraMap.get(aura.name).data;
                creatorAuras.push(aura);

                auraMap.set(aura.name, { count: creatorAurasCount + 1, data: creatorAuras });
            } else {
                auraMap.set(aura.name, { count: 1, data: [aura] });
            }
        });

        setInformationSelectedCreatorAuras(auraMap);
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

    return (
        <ul className="creator-auras">
            {Array.from(informationSelectedCreatorAuras.entries()).map(([key, value]) => (
                <li key={key} className="creator-auras__details">
                    <ul className="details-collection">
                        <li>{key}</li>
                        <li>{value.count}</li>
                        <li>
                            <div className={`btn-shadow ${selectedAura === key ? 'details-opened' : ''}`} onClick={() => handleSelectAura(key)}>
                                <FontAwesomeIcon
                                    icon={faEye}
                                />
                                <div>{t("Targets")}</div>
                            </div>
                            {(showTargets && selectedAura === key) &&
                                <CombatAuraTargets
                                    combatAuras={value.data}
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