import { faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import type { CharacterModel } from '../../types/account/CharacterModel';
import SelectedRealmCharacters from './SelectedRealmCharacters';

const RealmCharacters: React.FC<{ characters: Map<string, CharacterModel[]> }> = ({ characters }) => {
    const [selectedRealm, setSelectedRealm] = useState("0");

    return (
        <ul className="servers">
            {Object.entries(characters).map(([key, characters]: [string, CharacterModel[]]) => (
                <li>
                    <div className="btn-shadow"
                        onClick={() => setSelectedRealm(prev => prev === key ? "0" : key)}>
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{key}</div>
                        <div className="count">{characters.length}</div>
                    </div>
                    {selectedRealm === key &&
                        <SelectedRealmCharacters
                            characters={characters}
                        />
                    }
                </li>
            ))
            }
        </ul>
    );
}

export default RealmCharacters;