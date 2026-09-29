import type { WoWGameDataEntityModel } from '../../types/WoWGameDataEntityModel';

const CharacterProfessionsTier: React.FC<{ knownRecipes: WoWGameDataEntityModel[] }> = ({ knownRecipes }) => {
    return (
        <div className="character-profession-recipes">
            <ul>
                {knownRecipes.map(recipe => (
                    <li>{recipe.name}</li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterProfessionsTier;