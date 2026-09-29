import { faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import type { CharacterProfessionTierModel } from '../../types/professions/CharacterProfessionTierModel';
import CharacterProfessionsTier from './CharacterProfessionsTier';

const CharacterProfessionsTiers: React.FC<{ tiers: CharacterProfessionTierModel[] }> = ({ tiers }) => {
    const [selectedTierId, setSelectedTierId] = useState<number>(0);

    return (
        <div className="character-profession-tiers">
            <ul className="character-profession-tiers__tiers">
                {tiers.map(tier => (
                    <li key={tier.tier.id}>
                        <div className="tier">
                            <div className="btn-shadow"
                                onClick={() => setSelectedTierId(prev => prev === tier.tier.id ? 0 : tier.tier.id)}>
                                <FontAwesomeIcon
                                    icon={faLocationCrosshairs}
                                />
                                <div>{tier.tier.name}</div>
                            </div>
                        </div>
                        {selectedTierId === tier.tier.id &&
                            <CharacterProfessionsTier
                                knownRecipes={tier.knownRecipes}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterProfessionsTiers;