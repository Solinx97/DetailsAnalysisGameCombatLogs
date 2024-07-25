import { faClock, faFire, faFlask, faHandFist, faRightToBracket, faUserTie } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const HealDoneHelper = ({ detailsData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const [getTimeWithoutMs, , getDuration] = useTime();

    const getUserNameWithoutRealm = (username) => {
        let realmNameIndex = username.indexOf('-');
        let userNameWithOutRealm = username.substr(0, realmNameIndex);

        return userNameWithOutRealm;
    }

    return detailsData.map((item) => (
        <li key={item.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{item.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {item.isCrit &&
                            <FontAwesomeIcon
                                icon={faFire}
                                title={t("CritHealing")}
                            />
                        }
                        {item.isFullOverheal &&
                            <FontAwesomeIcon
                                icon={faFlask}
                                title={t("AllToOverHeal")}
                            />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faClock}
                            className="list-group-item__value"
                            title={t("Time")}
                        />
                        <div>{getDuration(getTimeWithoutMs(item.time), getTimeWithoutMs(detailsData[0].time))}</div>
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
                        <div>{getUserNameWithoutRealm(item.fromPlayer)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faRightToBracket}
                            className="list-group-item__value"
                            title={t("ToTarget")}
                        />
                        <div>{item.toPlayer}</div>
                    </li>
                </ul>
            </div>
        </li>
    ));
}

export default HealDoneHelper;