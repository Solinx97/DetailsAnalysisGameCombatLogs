import type { CharacterEquipmentStatModel } from '../../types/equipments/CharacterEquipmentStatModel';
import { faPowerOff } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const CharacterEquipmentStats: React.FC<{ stats: CharacterEquipmentStatModel[] }> = ({ stats }) => {
    return (
        <ul className="equipment__stats">
            {stats.map((stat, index) => (
                <li className="stat" key={index}>
                    <FontAwesomeIcon
                        icon={faPowerOff}
                    />
                    <div style={{ color: `rgb(${stat.display.color.red}, ${stat.display.color.green}, ${stat.display.color.blue}, ${stat.display.color.alfa})` }}>
                        {stat.display.displayString}
                    </div>
                </li>
            ))
            }
        </ul>
    );
}

export default CharacterEquipmentStats;