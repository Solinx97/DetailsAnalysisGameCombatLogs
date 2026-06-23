import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import type { CombatPlayerPreAuraModel } from '../../types/CombatPlayerPreAuraModel';
import { useGetCombatByPreAuraQuery } from '../../api/GameLogs.api';
import Loading from '@/shared/components/Loading';

interface CombatPreAuraItemProps {
    combatPlayerId: number;
    combatId: number;
    t: (key: string) => string;
}

const CombatPreAuraItem: React.FC<CombatPreAuraItemProps> = ({ combatPlayerId, combatId, t }) => {
    const [combatPlayerPreAuras, setCombatPlayerPreAuras] = useState<CombatPlayerPreAuraModel[]>([]);
    const [seeBuffs, setSeeBuffs] = useState(false);

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
            <div className="creator-auras__title">
                <div className="title" onClick={() => setSeeBuffs(prev => !prev)}>
                    <div>All raid buffs</div>
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
            </div>
            {seeBuffs &&
                <ul className="creator-pre-auras__content">
                    {combatPlayerPreAuras.map((value) => (
                        <li key={value.id} className="creator-auras__details">
                            <div>{value.name}</div>
                        </li>
                    ))}
                </ul>
            }
        </div>
    );
}

export default CombatPreAuraItem;