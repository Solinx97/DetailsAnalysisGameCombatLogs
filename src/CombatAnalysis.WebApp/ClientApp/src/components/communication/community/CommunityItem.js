import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';

const CommunityItem = ({ currentMenuItem, community }) => {
    const { t } = useTranslation("communication/community/communityItem");

    const render = () => {
        return (
            <ul>
                <li className="menu-item">
                    {currentMenuItem === 6 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                            title={t("CurrentAction")}
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