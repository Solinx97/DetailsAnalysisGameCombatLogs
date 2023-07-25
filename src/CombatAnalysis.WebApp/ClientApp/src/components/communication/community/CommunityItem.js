import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const CommunityItem = ({ currentMenuItem, community }) => {
    const render = () => {
        return (
            <ul>
                <li className="menu-item">
                    {currentMenuItem === 6 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                            title="Current action"
                        />
                    }
                    <div className="title">{community.name}</div>
                </li>
            </ul>
        );
    }

    return render();
}

export default CommunityItem;