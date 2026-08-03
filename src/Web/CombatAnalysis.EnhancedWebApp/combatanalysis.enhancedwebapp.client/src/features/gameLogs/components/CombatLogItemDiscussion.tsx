import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const CombatLogItemDiscussion: React.FC<{ t: (key: string) => string }>= ({ t }) => {
    return (
        <div className="item disabled">
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