import type { CharacterEquipmentSocketModel } from '../../types/equipments/CharacterEquipmentSocketModel';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const CharacterEquipmentSockets: React.FC<{ sockets: CharacterEquipmentSocketModel[] }> = ({ sockets }) => {
    return (
        <ul className="equipment__sockets">
            {sockets.map((socket, index) => (
                <li className="socket" key={index}>
                    <FontAwesomeIcon
                        icon={faCircle}
                    />
                    <div>{socket.displayString}</div>
                </li>
            ))
            }
        </ul>
    );
}

export default CharacterEquipmentSockets;