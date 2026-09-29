import { faLocationCrosshairs, faHammer } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import type { CharacterModel } from '../../types/account/CharacterModel';
import SelectedRealmCharacters from './SelectedRealmCharacters';

interface RealmCharactersProps {
    characters: Map<string, CharacterModel[]>;
}

const RealmCharacters: React.FC<RealmCharactersProps> = ({ characters }) => {
    const [selectedRealm, setSelectedRealm] = useState("0");
    const [isLoadingProfessions, setIsLoadingProfessions] = useState(false);

    const selectRealmHamdle = (realmName: string) => {
        setSelectedRealm(prev => prev === realmName ? "0" : realmName);
        setIsLoadingProfessions(false);
    }

    return (
        <ul className="servers">
            {Object.entries(characters).map(([key, characters]: [string, CharacterModel[]]) => (
                <li key={key}>
                    <div className="name">
                        <div className="btn-shadow"
                            onClick={() => selectRealmHamdle(key)}>
                            <FontAwesomeIcon
                                icon={faLocationCrosshairs}
                            />
                            <div>{key}</div>
                            <div className="count">{characters.length}</div>
                        </div>
                        <FontAwesomeIcon
                            icon={faHammer}
                            color={(selectedRealm === key && isLoadingProfessions) ? 'green' : 'white'}
                            onClick={() => setIsLoadingProfessions(prev => !prev)}
                            className="load-professions"
                        />
                    </div>
                    {selectedRealm === key &&
                        <SelectedRealmCharacters
                            characters={characters}
                            isLoadingProfessions={selectedRealm === key && isLoadingProfessions}
                        />
                    }
                </li>
            ))
            }
        </ul>
    );
}

export default RealmCharacters;