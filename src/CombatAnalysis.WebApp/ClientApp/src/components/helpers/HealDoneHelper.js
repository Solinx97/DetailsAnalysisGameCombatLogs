import { faClock, faFire, faFlask, faHandFist, faRightToBracket, faUserTie } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const HealDoneHelper = ({ detailsData }) => {
    const [getTimeWithoutMs, , getDuration] = useTime();
    const { t, i18n } = useTranslation("helpers/combatDetailsHelper");

    const getUserNameWithoutRealm = (username) => {
        let realmNameIndex = username.indexOf('-');
        let userNameWithOutRealm = username.substr(0, realmNameIndex);

        return userNameWithOutRealm;
    }

    const createItem = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {element.isCrit &&
                            <FontAwesomeIcon icon={faFire} title={t("CritHealing")} />
                        }
                        {element.isFullOverheal &&
                            <FontAwesomeIcon icon={faFlask} title={t("AllToOverHeal")} />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title={t("Time")} />
                        <div>{getDuration(getTimeWithoutMs(element.time), getTimeWithoutMs(detailsData[0].time))}</div>
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
                        <div>{element.toPlayer}</div>
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

export default HealDoneHelper;