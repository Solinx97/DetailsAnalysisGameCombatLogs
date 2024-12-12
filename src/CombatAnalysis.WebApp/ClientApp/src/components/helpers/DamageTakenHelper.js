import { faCopy, faFire, faFlask, faHands, faPooStorm, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const damageTakenType =
{
    Normal: 0,
    Crushing: 1,
    Dodge: 2,
    Parry: 3,
    Miss: 4,
    Resist: 5,
    Immune: 6,
    Absorb: 7
}

const DamageTakenHelper = ({ detailsData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const { getTimeWithoutMs } = useTime();

    const getIcon = (type) => {
        switch (type) {
            case damageTakenType.Crushing:
                return <FontAwesomeIcon
                    icon={faFire}
                    title={t("Crushing")}
                    className="overvalue"
                />;
            case damageTakenType.Dodge:
                return <FontAwesomeIcon
                    icon={faCopy}
                    title={t("Dodge")}
                    className="overvalue"
                />;
            case damageTakenType.Parry:
                return <FontAwesomeIcon
                    icon={faXmark}
                    title={t("Parry")}
                    className="overvalue"
                />;
            case damageTakenType.Miss:
                return <FontAwesomeIcon
                    icon={faHands}
                    title={t("Miss")}
                    className="overvalue"
                />;
            case damageTakenType.Resist:
                return <FontAwesomeIcon
                    icon={faFlask}
                    title={t("Resist")}
                    className="overvalue"
                />;
            case damageTakenType.Immune:
                return <FontAwesomeIcon
                    icon={faPooStorm}
                    title={t("Immune")}
                    className="overvalue"
                />;
            case damageTakenType.Absorb:
                return <FontAwesomeIcon
                    icon={faPooStorm}
                    title={t("Absorb")}
                    className="overvalue"
                />;
            default:
                return <></>;
        }
    }

    const tableTitle = () => {
        return (
            <li className="player-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Spell")}
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
                            <div>{item.spell}</div>
                            <div className="extra-details">{getIcon(item.damageTakenType)}</div>
                        </li>
                        <li>
                            {getTimeWithoutMs(item.time)}
                        </li>
                        <li>
                            {item.value}
                        </li>
                        <li>
                            {item.creator}
                        </li>
                    </ul>
                </li>
            ))}
        </>
    );
}

export default DamageTakenHelper;