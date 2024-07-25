import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';

const CombatLogItemDiscussion = ({ isAuth, getChatsByUserIdAsync }) => {
    const { t } = useTranslation("combatDetails/mainInformation");

    return (
        <div className="communication disabled">
            <div className="btn-shadow">
                <FontAwesomeIcon
                    icon={faCirclePlus}
                />
                <div>{t("Chat")}</div>
            </div>
            <div className="btn-shadow">
                <FontAwesomeIcon
                    icon={faCirclePlus}
                />
                <div>{t("Discussion")}</div>
            </div>
        </div>
    );
}

export default CombatLogItemDiscussion;