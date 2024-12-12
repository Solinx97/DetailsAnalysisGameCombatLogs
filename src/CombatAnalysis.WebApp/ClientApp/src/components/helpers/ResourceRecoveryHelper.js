import { useTranslation } from 'react-i18next';
import useTime from '../../hooks/useTime';

const ResourceRecoveryHelper = ({ detailsData }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const { getTimeWithoutMs } = useTime();

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
                            {item.spell}
                        </li>
                        <li>
                            <div>{getTimeWithoutMs(item.time)}</div>
                        </li>
                        <li>
                            {item.value}
                        </li>
                    </ul>
                </li>
            ))}
        </>
    );
}

export default ResourceRecoveryHelper;