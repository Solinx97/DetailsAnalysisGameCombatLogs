import React from 'react';
import { fa0, faClock, faCopy, faFire, faFlask, faHandFist, faHands, faPooStorm, faRightToBracket, faUserTie } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const DamageDoneHelper = ({ detailsData }) => {
    const [getTimeWithoutMs] = useTime();
    const { t, i18n } = useTranslation("useCombatDetailsHelper");

    const getUserNameWithoutRealm = (userName) => {
        let realmNameIndex = userName.indexOf('-');
        let userNameWithOutRealm = userName.substr(0, realmNameIndex);

        return userNameWithOutRealm;
    }

    const createItem = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {element.isCrit &&
                            <FontAwesomeIcon icon={faFire} title={t("CritDamage")} />
                        }
                        {element.isDodge &&
                            <FontAwesomeIcon icon={faCopy} title={t("Dodge")} />
                        }
                        {element.isMiss &&
                            <FontAwesomeIcon icon={faHands} title={t("Miss")} />
                        }
                        {element.isParry &&
                            <FontAwesomeIcon icon={fa0} title={t("Parry")} />
                        }
                        {element.isImmune &&
                            <FontAwesomeIcon icon={faPooStorm} title={t("Immune")} />
                        }
                        {element.isResist &&
                            <FontAwesomeIcon icon={faFlask} title={t("Resist")} />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title={t("Time")} />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("Value")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title={t("FromWho")} />
                        <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title={t("ToTarget")} />
                        <div>{element.toEnemy}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const render = () => {
        const list = detailsData.map((element) => createItem(element));

        return list;
    }

    return render();
}

export default DamageDoneHelper;