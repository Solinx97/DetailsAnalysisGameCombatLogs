import { useTranslation } from 'react-i18next';

const fixedNumberUntil = 2;

const DamageTakenGeneralHelper = ({ generalData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const tableTitle = () => {
        return (
            <li className="player-general-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Spell")}
                    </li>
                    <li>
                        {t("TotalDamageTaken")}
                    </li>
                    <li>
                        {t("AverageValue")}
                    </li>
                    <li>
                        {t("DamageTakenPerSec")}
                    </li>
                    <li>
                        {t("CountOfSkills")}
                    </li>
                    <li>
                        {t("CountOfCrits")}
                    </li>
                    <li>
                        {t("CountOfMiss")}
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
                            {item.damageTakenPerSecond.toFixed(fixedNumberUntil)}
                        </li>
                        <li>
                            {item.castNumber}
                        </li>
                        <li>
                            {item.critNumber}
                        </li>
                        <li>
                            {item.missNumber}
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

export default DamageTakenGeneralHelper;