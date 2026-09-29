import type { CharacterModel } from '../../types/account/CharacterModel';
import Character from '../Character';
import SelectedRealmCharacterProfessions from './SelectedRealmCharacterProfessions';

const SelectedRealmCharacters: React.FC<{ characters: CharacterModel[], isLoadingProfessions: boolean }> = ({ characters, isLoadingProfessions }) => {
    return (
        <ul className="server-characters">
            {characters.map((character) => (
                <li className="server-characters__character" key={character.id}>
                    <SelectedRealmCharacterProfessions
                        username={character.name ?? ""}
                        serverNameSlug={character.realm.slug}
                        isLoadingProfessions={isLoadingProfessions}
                    />
                    <Character
                        character={character}
                    />
                </li>
            ))
            }
        </ul>
    );
}

export default SelectedRealmCharacters;