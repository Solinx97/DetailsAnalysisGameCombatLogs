import { useEffect, useState } from 'react';
import type { CombatPlayerPreAura } from '../../types/CombatPlayerPreAura';

interface CombatPreAuraItemProps {
    preAuras: CombatPlayerPreAura[];
    combatPlayerId: number;
}

const CombatPreAuraItem: React.FC<CombatPreAuraItemProps> = ({ preAuras, combatPlayerId }) => {
    const [combatPlayerPreAuras, setCombatPlayerPreAuras] = useState<CombatPlayerPreAura[]>(preAuras);

    useEffect(() => {
        makeCreatorAurasMap();
    }, [combatPlayerId]);

    const makeCreatorAurasMap = () => {
        let unique: CombatPlayerPreAura[] = [];
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
        <ul className="creator-auras">
            {combatPlayerPreAuras.map((value) => (
                <li key={value.id} className="creator-auras__details">
                    <div>{value.name}</div>
                </li>
            ))}
        </ul>
    );
}

export default CombatPreAuraItem;