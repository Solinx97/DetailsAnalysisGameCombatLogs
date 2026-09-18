import { faAppleWhole, faBolt, faFlask, faHourglass, faVial } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetCombatPreAurasQuery, useGetUnitPreAurasQuery } from '../../api/GameLogs.api';

interface CombatPreAuraItemProps {
    combatId: number;
    unitId?: string;
}

const CombatPreAuraItem: React.FC<CombatPreAuraItemProps> = ({ combatId, unitId }) => {
     const { data: allPreAuras, isLoading } = unitId !== undefined 
                                                ? useGetUnitPreAurasQuery({ combatId, unitId })
                                                :  useGetCombatPreAurasQuery(combatId);
 
    if (isLoading || !allPreAuras) {
        return (<></>);
    }

    return (
        <div className="creator-pre-auras">
            <ul className="creator-pre-auras__content">
                {allPreAuras.map((value) => (
                    <li key={value.id} className="creator-pre-auras pre-aura-item" title={value.name}>
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
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default CombatPreAuraItem;