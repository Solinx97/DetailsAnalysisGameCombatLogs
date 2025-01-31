import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetDamageTakenByCombatPlayerIdQuery } from '../../store/api/combatParser/DamageTaken.api';

const minCount = 4;

const DashboardDeathItem = ({ playersDeath, players }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const [getDamageTakenByCombatPlayerId] = useLazyGetDamageTakenByCombatPlayerIdQuery();

    const [itemCount, setItemCount] = useState(minCount);

    useEffect(() => {
        if (!playersDeath || playersDeath.length === 0) {
            return;
        }
    }, [playersDeath, players, getDamageTakenByCombatPlayerId]);

    if (playersDeath === undefined) {
        return (<div>Loading...</div>);
    }

    if (playersDeath.length === 0) {
        return (
            <div className="dashboard__statistics">
                <div>{t("PlayersDied")}</div>
                <div>{t("Empty")}</div>
            </div>
        );
    }

    return (
        <div className="dashboard__statistics">
            <div>{t("PlayersDied")}</div>
            <ul className="death-info">
                {playersDeath?.slice(0, itemCount).map((death, index) => (
                    <li key={index} className="death-info__details">
                        <div>{death?.username}</div>
                        <div>{death?.lastHitSpellOrItme}</div>
                        <div>{death?.lastHitValue}</div>
                    </li>
                ))}
            </ul>
            <div className="extend" onClick={() => setItemCount(itemCount === minCount ? playersDeath.length : minCount)}>
                {itemCount === minCount ? t("More") : t("Less")}
            </div>
        </div>
    );
}

export default DashboardDeathItem;