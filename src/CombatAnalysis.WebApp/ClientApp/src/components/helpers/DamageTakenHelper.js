import { fa0, faCopy, faFire, faFlask, faHands, faPooStorm } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const DamageTakenHelper = ({ detailsData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const { getTimeWithoutMs } = useTime();

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
                        {t("WhoDamage")}
                    </li>
                </ul>
            </li>
        );
    }

    return (
        <>
            {tableTitle()}
            {detailsData?.map((item) => (
                <li className="player-data-details__item" key={item.id}>
                    <ul>
                        <li>
                            <div>{item.spellOrItem}</div>
                            <div>
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
                        </li>
                        <li>
                            {getTimeWithoutMs(item.time)}
                        </li>
                        <li>
                            {item.value}
                        </li>
                        <li>
                            {item.fromEnemy}
                        </li>
                    </ul>
                </li>
            ))}
        </>
    );
}

export default DamageTakenHelper;