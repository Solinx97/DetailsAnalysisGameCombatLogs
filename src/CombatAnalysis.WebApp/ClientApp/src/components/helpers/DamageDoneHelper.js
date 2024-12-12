import { faCopy, faFire, faFlask, faHands, faPooStorm, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React from 'react';
import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const damageType =
{
    Normal: 0,
    Crit: 1,
    Dodge: 2,
    Parry: 3,
    Miss: 4,
    Resist: 5,
    Immune: 6,
}

const DamageDoneHelper = ({ detailsData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const { getTimeWithoutMs } = useTime();

    const getIcon = (type) => {
        switch (type) {
            case damageType.Crit:
                return <FontAwesomeIcon
                    icon={faFire}
                    title={t("CritDamage")}
                    className="crit"
                />;
            case damageType.Dodge:
                return <FontAwesomeIcon
                    icon={faCopy}
                    title={t("Dodge")}
                    className="overvalue"
                />;
            case damageType.Parry:
                return <FontAwesomeIcon
                    icon={faXmark}
                    title={t("Parry")}
                    className="overvalue"
                />;
            case damageType.Miss:
                return <FontAwesomeIcon
                    icon={faHands}
                    title={t("Miss")}
                    className="overvalue"
                />;
            case damageType.Resist:
                return <FontAwesomeIcon
                    icon={faFlask}
                    title={t("Resist")}
                    className="overvalue"
                />;
            case damageType.Immune:
                return <FontAwesomeIcon
                    icon={faPooStorm}
                    title={t("Immune")}
                    className="overvalue"
                />;
            default:
                return <></>;
        }
    }

    const getClassNameByDamageType = (item) => {
        if (item.damageType === 1) {
            return "crit";
        }
        else if (item.damageType > 1) {
            return "overvalue";
        }
        else {
            return "";
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
                        {t("Target")}
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
                            <div className="extra-details">{getIcon(item.damageType)}</div>
                        </li>
                        <li>{getTimeWithoutMs(item.time)}</li>
                        <li className="extra-details">
                            <div className={getClassNameByDamageType(item)}>{item.value}</div>
                        </li>
                        <li>{item.target}</li>
                    </ul>
                </li>
            ))}
        </>
    );
}

export default DamageDoneHelper;