import { faClock, faHandFist, faRightToBracket, faUserTie } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const ResourceRecoveryHelper = ({ detailsData, startDate }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const [getTimeWithoutMs, , getDuration] = useTime();

    return detailsData.map((item) => (
        <li key={item.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{item.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faClock}
                            className="list-group-item__value"
                            title={t("Time")}
                        />
                        <div>{getDuration(getTimeWithoutMs(item.time), getTimeWithoutMs(startDate))}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faHandFist}
                            className="list-group-item__value"
                            title={t("Value")}
                        />
                        <div>{item.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faUserTie}
                            className="list-group-item__value"
                            title={t("FromWho")}
                        />
                        <div>{item.spellOrItem}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faRightToBracket}
                            className="list-group-item__value"
                            title={t("ToTarget")}
                        />
                        <div>{item.toEnemy}</div>
                    </li>
                </ul>
            </div>
        </li>
    ));
}

export default ResourceRecoveryHelper;