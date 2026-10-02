import WoWGameDataContext from '@/context/WoWGameDataContext';
import { faPlus, faCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext } from 'react';
import type { DungeonCharacterModel } from '../types/mythicKeystone/DungeonCharacterModel';

const Character: React.FC<{ character: DungeonCharacterModel  }> = ({ character }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { username, serversOptions, setUsername, setServerName } = context;

    const selectCharacterHandle = (character: DungeonCharacterModel) => {
        let selectedUsername = ""
        let selectedServerSlug = "";
        let selectedServerName = "";

        if (character.name !== username) {
            const findServerName = serversOptions.find(x => x.value === character.realm.slug);

            selectedUsername = character.name ?? "";
            selectedServerSlug = character.realm.slug;
            selectedServerName = findServerName ? findServerName.label : "";
        }

        setUsername(selectedUsername);
        setServerName({
            value: selectedServerSlug,
            label: selectedServerName
        });
    }

    return (
        <div className="select-character">
            <div className={`character ${username === character.name ? 'selected' : ''}`}>{character.name}</div>
            <FontAwesomeIcon
                icon={username === character.name ? faCheck : faPlus}
                color={username === character.name ? '#10e38f' : '#fffff'}
                onClick={() => selectCharacterHandle(character)}
            />
        </div>
    );
}

export default Character;