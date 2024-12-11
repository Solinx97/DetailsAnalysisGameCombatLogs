import { faFire, faFlask } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const HealDoneHelper = ({ detailsData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const { getTimeWithoutMs } = useTime();

    const getUserNameWithoutRealm = (username) => {
        if (!username.includes('-')) {
            return username;
        }

        const realmNameIndex = username.indexOf('-');
        const userNameWithoutRealm = username.substr(0, realmNameIndex);

        return userNameWithoutRealm;
    }

    const tableTitle = () => {
        return (
            <li className="player-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Skill")}
                    </li>
                    <li>
                        {t("Time")}
                    </li>
                    <li>
                        {t("Value")}
                    </li>
                    <li>
                        {t("Target")}
                    </li>
                </ul>
            </li>
        );
    }

    return (
        <>
            {tableTitle()}
            {detailsData.map((item) => (
                <li className="player-data-details__item" key={item.id}>
                    <ul>
                        <li>
                            <div>{item.spellOrItem}</div>
                            <div>
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
                        </li>
                        <li>
                            {getTimeWithoutMs(item.time)}
                        </li>
                        <li>
                            {item.value}
                        </li>
                        <li>
                            {getUserNameWithoutRealm(item.toPlayer)}
                        </li>
                    </ul>
                </li>
            ))}
        </>
    );
}

export default HealDoneHelper;