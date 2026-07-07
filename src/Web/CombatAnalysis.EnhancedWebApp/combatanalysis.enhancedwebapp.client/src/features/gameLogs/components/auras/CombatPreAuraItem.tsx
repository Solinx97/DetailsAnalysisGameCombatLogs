import { faFlask, faHourglass, faAppleWhole, faVial, faBolt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import type { CombatPlayerPreAuraModel } from '../../types/CombatPlayerPreAuraModel';
import { useGetCombatByPreAuraQuery } from '../../api/GameLogs.api';
import Loading from '@/shared/components/Loading';

interface CombatPreAuraItemProps {
    combatPlayerId: number;
    combatId: number;
}

const CombatPreAuraItem: React.FC<CombatPreAuraItemProps> = ({ combatPlayerId, combatId }) => {
    const [combatPlayerPreAuras, setCombatPlayerPreAuras] = useState<CombatPlayerPreAuraModel[]>([]);

    const { data: allPreAuras, isLoading } = useGetCombatByPreAuraQuery({ combatPlayerId, combatId });

    useEffect(() => {
        if (!allPreAuras) {
            return;
        }

        makeCreatorAurasMap();
    }, [allPreAuras]);

    const makeCreatorAurasMap = () => {
        let unique: CombatPlayerPreAuraModel[] = [];
        const selectedCombatPlayerPreAuras = allPreAuras!.filter(x => x.combatPlayerId === combatPlayerId);

        if (selectedCombatPlayerPreAuras.length > 0) {
            unique = [...new Map(
                selectedCombatPlayerPreAuras.map(item => [item.name, item])
            ).values()];
            setCombatPlayerPreAuras(unique);
        } else {
            unique = [...new Map(
                allPreAuras!.map(item => [item.name, item])
            ).values()];
        }

        setCombatPlayerPreAuras(unique);
    }

    if (isLoading) {
        return (
            <div>
                <Loading />
            </div>
        );
    }

    return (
        <div className="creator-pre-auras">
            <ul className="creator-pre-auras__content">
                {combatPlayerPreAuras.map((value) => (
                    <li key={value.id} className="creator-pre-auras pre-aura-item">
                        {value.abilityType === 1 &&
                            <FontAwesomeIcon
                                icon={faVial}
                            />
                        }
                        {value.abilityType === 0 &&
                            <FontAwesomeIcon
                                icon={faFlask}
                            />
                        }
                        {value.abilityType === 7 &&
                            <FontAwesomeIcon
                                icon={faHourglass}
                            />
                        }
                        {value.abilityType === 9 &&
                            <FontAwesomeIcon
                                icon={faAppleWhole}
                            />
                        }
                        {value.abilityType === 10 &&
                            <FontAwesomeIcon
                                icon={faBolt}
                            />
                        }
                        <div>{value.name}</div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default CombatPreAuraItem;