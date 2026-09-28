import type { CharacterModel } from '../../types/account/CharacterModel';
import Character from '../Character';

const SelectedRealmCharacters: React.FC<{ characters: CharacterModel[] }> = ({ characters }) => {
    return (
        <ul className="server-characters">
            {characters.map((character) => (
                <li key={character.id}>
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