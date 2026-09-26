import type { CharacterEquipmentEnchantmentModel } from '../../types/equipments/CharacterEquipmentEnchantmentModel';
import { faScroll } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const CharacterEquipmentEnchantments: React.FC<{ enchantments: CharacterEquipmentEnchantmentModel[] }> = ({ enchantments }) => {
    return (
        <ul className="equipment__enchantments">
            {enchantments.map((ench, index) => (
                <li className="enchantment" key={index}>
                    <FontAwesomeIcon
                        icon={faScroll}
                    />
                    <div>{ench.displayString}</div>
                </li>
            ))
            }
        </ul>
    );
}

export default CharacterEquipmentEnchantments;