import WoWGameDataContext from '@/context/WoWGameDataContext';
import type { CharacterModel } from '../../types/account/CharacterModel';
import { faPlus, faCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext } from 'react';

const SelectedRealmCharacters: React.FC<{ characters: CharacterModel[] }> = ({ characters }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { username, setUsername, setServerName } = context;

    const selectCharacterHandle = (character: CharacterModel) => {
        let selectedUsername = ""
        let selectedServerSlug = "";
        let selectedServerName = "";

        if (character.name !== username) {
            selectedUsername = character.name ?? "";
            selectedServerSlug = character.realm.slug;
            selectedServerName = character.realm.name ? character.realm.name : "";
        }

        setUsername(selectedUsername);
        setServerName({
            value: selectedServerSlug,
            label: selectedServerName
        });
    }

    return (
        <ul className="server-characters">
            {characters.map((character) => (
                <li className="server-characters__character">
                    <div className={`character ${username === character.name ? 'selected' : ''}`}>{character.name}</div>
                    <FontAwesomeIcon
                        icon={username === character.name ? faCheck : faPlus}
                        color={username === character.name ? 'green' : 'white'}
                        onClick={() => selectCharacterHandle(character)}
                    />
                </li>
            ))
            }
        </ul>
    );
}

export default SelectedRealmCharacters;