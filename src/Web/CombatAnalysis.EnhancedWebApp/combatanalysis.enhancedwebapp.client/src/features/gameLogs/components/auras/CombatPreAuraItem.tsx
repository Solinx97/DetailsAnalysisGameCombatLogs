import { useEffect, useState } from 'react';
import type { CombatPlayerPreAuraModel } from '../../types/CombatPlayerPreAuraModel';

interface CombatPreAuraItemProps {
    preAuras: CombatPlayerPreAuraModel[];
    combatPlayerId: number;
}

const CombatPreAuraItem: React.FC<CombatPreAuraItemProps> = ({ preAuras, combatPlayerId }) => {
    const [combatPlayerPreAuras, setCombatPlayerPreAuras] = useState<CombatPlayerPreAuraModel[]>(preAuras);

    useEffect(() => {
        makeCreatorAurasMap();
    }, [combatPlayerId]);

    const makeCreatorAurasMap = () => {
        let unique: CombatPlayerPreAuraModel[] = [];
        const selectedCombatPlayerPreAuras = preAuras.filter(x => x.combatPlayerId === combatPlayerId);

        if (selectedCombatPlayerPreAuras.length > 0) {
            unique =  [...new Map(
                selectedCombatPlayerPreAuras.map(item => [item.name, item])
                ).values()];
            setCombatPlayerPreAuras(unique);
        } else {
            unique =  [...new Map(
                preAuras.map(item => [item.name, item])
                ).values()];
        }

        setCombatPlayerPreAuras(unique);
    }

    return (
        <ul className="creator-pre-auras">
            {combatPlayerPreAuras.map((value) => (
                <li key={value.id} className="creator-auras__details">
                    <div>{value.name}</div>
                </li>
            ))}
        </ul>
    );
}

export default CombatPreAuraItem;