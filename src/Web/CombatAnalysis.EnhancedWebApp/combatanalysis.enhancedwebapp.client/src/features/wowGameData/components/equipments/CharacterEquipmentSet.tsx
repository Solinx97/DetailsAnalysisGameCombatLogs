import { faStream, faCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { CharacterEquipmentSetModel } from '../../types/equipments/CharacterEquipmentSetModel';

const CharacterEquipmentSet: React.FC<{ set: CharacterEquipmentSetModel }> = ({ set }) => {
    return (
        <ul className="equipment__set">
            {set.effects.map((set, index) => (
                <li className="set" key={index}>
                    <FontAwesomeIcon
                        icon={set.isActive ? faCheck : faStream}
                    />
                    <div className={`${set.isActive ? 'active' : 'not-active'}`}>{set.displayString}</div>
                </li>
            ))
            }
        </ul>
    );
}

export default CharacterEquipmentSet;