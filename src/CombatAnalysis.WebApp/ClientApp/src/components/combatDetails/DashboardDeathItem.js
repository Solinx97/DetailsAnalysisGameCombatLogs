import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetDamageTakenByPlayerIdQuery } from '../../store/api/CombatParserApi';

const minCount = 4;

const DashboardDeathItem = ({ playersDeath, players }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const [getDamageTakenByPlayerIdAsync] = useLazyGetDamageTakenByPlayerIdQuery();

    const [itemCount, setItemCount] = useState(minCount);
    const [extendedPlayersDeath, setExtendedPlayersDeath] = useState([]);

    useEffect(() => {
        if (!playersDeath || playersDeath.length === 0) {
            return;
        }

        const fetchPlayerDeaths = async () => {
            await fetchPlayerDeathsAsync()
        }

        fetchPlayerDeaths();
    }, [playersDeath, players, getDamageTakenByPlayerIdAsync]);

    const fetchPlayerDeathsAsync = async () => {
        const deathDetailsPromises = playersDeath.map(async (death) => {
            const player = players.find(player => player.id === death.combatPlayerId);
            if (!player) {
                return null;
            }

            const response = await getDamageTakenByPlayerIdAsync(player.id);
            if (response.error !== undefined) {
                return null;
            } 

            const lastHit = response.data[response.data.length - 1];

            return {
                ...death,
                username: player.userName,
                skill: lastHit.spellOrItem,
                value: lastHit.value,
            };
        });

        const deathDetails = (await Promise.all(deathDetailsPromises)).filter(Boolean);
        setExtendedPlayersDeath(deathDetails);
    }

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
                {extendedPlayersDeath?.slice(0, itemCount).map((death, index) => (
                    <li key={index} className="death-info__details">
                        <div>{death?.username}</div>
                        <div>{death?.skill}</div>
                        <div>{death?.value}</div>
                    </li>
                ))}
            </ul>
            <div className="extend" onClick={() => setItemCount(itemCount === minCount ? extendedPlayersDeath.length : minCount)}>
                {itemCount === minCount ? t("More") : t("Less")}
            </div>
        </div>
    );
}

export default DashboardDeathItem;