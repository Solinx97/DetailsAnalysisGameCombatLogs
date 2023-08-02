import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import DetailsPieChart from '../childs/DetailsPieChart';

const GeneralDetailsChart = ({ combatPlayers }) => {
    const { t } = useTranslation("detailsSpecificalCombat");

    const [damageDonePieChart, setDamageDonePieChart] = useState({});
    const [healDonePieChart, setHealDonePieChart] = useState({});
    const [damageTakenPieChart, setDamageTakenPieChart] = useState({});

    useEffect(() => {
        let data = createPieChardData();

        setDamageDonePieChart({
            title: t("Damage"),
            color: "blue",
            data: data.damageDone
        });
        setHealDonePieChart({
            title: t("Healing"),
            color: "green",
            data: data.healDone
        });
        setDamageTakenPieChart({
            title: t("DamageTaken"),
            color: "orange",
            data: data.damageTaken
        });
    }, [])

    const compare = (a, b) => {
        if (a.value > b.value) {
            return -1;
        }
        if (a.value < b.value) {
            return 1;
        }

        return 0;
    }

    const createPieChardData = () => {
        const healDone = [];
        const damageTaken = [];
        const damageDone = new Array(combatPlayers.length);
        for (let i = 0; i < combatPlayers.length; i++) {
            const realmNameIndex = combatPlayers[i].userName.indexOf('-');
            const username = combatPlayers[i].userName.substr(0, realmNameIndex);

            damageDone[i] = {};
            damageDone[i].name = username
            damageDone[i].value = combatPlayers[i].damageDone;

            healDone[i] = {};
            healDone[i].name = username
            healDone[i].value = combatPlayers[i].healDone;

            damageTaken[i] = {};
            damageTaken[i].name = username
            damageTaken[i].value = combatPlayers[i].damageTaken;
        }

        return {
            damageDone: damageDone.sort(compare),
            healDone: healDone.sort(compare),
            damageTaken: damageTaken.sort(compare)
        };
    }

    return (
        <div className="details-specifical-combat__container_general-details-charts">
            <DetailsPieChart
                payload={damageDonePieChart}
            />
            <DetailsPieChart
                payload={healDonePieChart}
            />
            <DetailsPieChart
                payload={damageTakenPieChart}
            />
        </div>
    );
}

export default GeneralDetailsChart;