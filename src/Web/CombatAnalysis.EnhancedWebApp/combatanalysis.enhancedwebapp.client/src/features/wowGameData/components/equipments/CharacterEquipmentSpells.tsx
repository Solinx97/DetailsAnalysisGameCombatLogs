import { faMagic } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { CharacterEquipmentSpellModel } from '../../types/equipments/CharacterEquipmentSpellModel';

const CharacterEquipmentSpells: React.FC<{ spells: CharacterEquipmentSpellModel[] }> = ({ spells }) => {
    return (
        <ul className="equipment__spells">
            {spells.map((spell, index) => (
                <li className="socket" key={index}>
                    <FontAwesomeIcon
                        icon={faMagic}
                    />
                    <div style={{ color: `rgb(${spell.color?.red}, ${spell.color?.green}, ${spell.color?.blue}, ${spell.color?.alfa})` }}>
                        {spell.description}
                        </div>
                </li>
            ))
            }
        </ul>
    );
}

export default CharacterEquipmentSpells;