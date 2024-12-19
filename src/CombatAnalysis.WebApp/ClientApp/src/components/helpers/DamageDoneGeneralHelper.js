import React from 'react';
import { useTranslation } from 'react-i18next';

const fixedNumberUntil = 2;

const DamageDoneGeneralHelper = ({ generalData, getProcentage }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const tableTitle = () => {
        return (
            <li className="player-general-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Spell")}
                    </li>
                    <li>
                        {t("Total")}
                    </li>
                    <li>
                        {t("AverageValue")}
                    </li>
                    <li>
                        {t("DamagePerSec")}
                    </li>
                    <li>
                        {t("CountOfSkills")}
                    </li>
                    <li>
                        {t("Crit")}, %
                    </li>
                    <li>
                        {t("Miss")}, %
                    </li>
                    <li>
                        {t("MaxValue")}
                    </li>
                    <li>
                        {t("MinValue")}
                    </li>
                </ul>
            </li>
        );
    }

    return (
        <>
            {tableTitle()}
            {generalData?.map((item) => (
                <li className="player-general-data-details__item" key={item.id}>
                    <ul>
                        <li>
                            {item.spell}
                        </li>
                        <li>
                            {item.value}
                        </li>
                        <li>
                            {item.averageValue.toFixed(fixedNumberUntil)}
                        </li>
                        <li>
                            {item.damagePerSecond.toFixed(fixedNumberUntil)}
                        </li>
                        <li>
                            {item.castNumber}
                        </li>
                        <li>
                            {getProcentage(item.critNumber, item.castNumber)}%
                        </li>
                        <li>
                            {getProcentage(item.missNumber, item.castNumber)}%
                        </li>
                        <li>
                            {item.maxValue}
                        </li>
                        <li>
                            {item.minValue}
                        </li>
                    </ul>
                </li>
            ))}
        </>
    );
}

export default DamageDoneGeneralHelper;