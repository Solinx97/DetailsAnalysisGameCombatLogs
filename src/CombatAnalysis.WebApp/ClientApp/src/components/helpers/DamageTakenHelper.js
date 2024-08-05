import { fa0, faClock, faCopy, faFire, faFlask, faHandFist, faHands, faPooStorm, faRightToBracket, faUserTie } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const DamageTakenHelper = ({ detailsData, startDate }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    console.log(startDate);

    const [getTimeWithoutMs, , getDuration] = useTime();

    const getUserNameWithoutRealm = (username) => {
        let realmNameIndex = username.indexOf('-');
        let userNameWithOutRealm = username.substr(0, realmNameIndex);

        return userNameWithOutRealm;
    }

    return detailsData?.map((item) => (
        <li key={item.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{item.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {item.isCrushing &&
                            <FontAwesomeIcon
                                icon={faFire}
                                title={t("Crushing")}
                            />
                        }
                        {item.isDodge &&
                            <FontAwesomeIcon
                                icon={faCopy}
                                title={t("Dodge")}
                            />
                        }
                        {item.isMiss &&
                            <FontAwesomeIcon
                                icon={faHands}
                                title={t("Miss")}
                            />
                        }
                        {item.isParry &&
                            <FontAwesomeIcon
                                icon={fa0}
                                title={t("Parry")}
                            />
                        }
                        {item.isImmune &&
                            <FontAwesomeIcon
                                icon={faPooStorm}
                                title={t("Immune")}
                            />
                        }
                        {item.isResist &&
                            <FontAwesomeIcon
                                icon={faFlask}
                                title={t("Resist")}
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
                            icon={faRightToBracket}
                            className="list-group-item__value"
                            title={t("ToTarget")}
                        />
                        <div>{item.fromEnemy}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faUserTie}
                            className="list-group-item__value"
                            title={t("FromWho")}
                        />
                        <div>{getUserNameWithoutRealm(item.toPlayer)}</div>
                    </li>
                </ul>
            </div>
        </li>
    ));
}

export default DamageTakenHelper;