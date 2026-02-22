import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { CombatPlayerPropertyModel } from '../types/CombatPlayerPropertyModel';
import DetailsPieChart from './DetailsPieChart';
import type { ChartPayloadModel } from '../types/ChartPayloadModel';

interface DetailsSpecificalCombatChartProps {
    combatPlayers: CombatPlayerModel[];
}

const SelectedCombatChart: React.FC<DetailsSpecificalCombatChartProps> = ({ combatPlayers }) => {
    const { t } = useTranslation("combatDetails/selectedCombat");

    const [damageDonePieChart, setDamageDonePieChart] = useState<ChartPayloadModel | null>(null);
    const [healDonePieChart, setHealDonePieChart] = useState<ChartPayloadModel | null>(null);
    const [damageTakenPieChart, setDamageTakenPieChart] = useState<ChartPayloadModel | null>(null);

    useEffect(() => {
        const data = createPieChardData();

        setDamageDonePieChart({
            title: t("Damage"),
            color: "#83B4FF",
            data: data.damageDone
        });
        setHealDonePieChart({
            title: t("Healing"),
            color: "#83B4FF",
            data: data.healDone
        });
        setDamageTakenPieChart({
            title: t("DamageTaken"),
            color: "#83B4FF",
            data: data.damageTaken
        });
    }, []);

    const compare = (a: CombatPlayerPropertyModel, b: CombatPlayerPropertyModel) => {
        if (a.value > b.value) {
            return -1;
        }
        if (a.value < b.value) {
            return 1;
        }

        return 0;
    }

    const createPieChardData = () => {
        const healDone = new Array<CombatPlayerPropertyModel>(combatPlayers.length);
        const damageTaken = new Array<CombatPlayerPropertyModel>(combatPlayers.length);
        const damageDone = new Array<CombatPlayerPropertyModel>(combatPlayers.length);

        for (let i = 0; i < combatPlayers.length; i++) {
            const realmNameIndex = combatPlayers[i].player.username.indexOf('-');
            const username = combatPlayers[i].player.username.substr(0, realmNameIndex);

            damageDone[i] = { name: "", value: 0 };
            damageDone[i].name = username
            damageDone[i].value = combatPlayers[i].damageDone;

            healDone[i] = { name: "", value: 0 };
            healDone[i].name = username
            healDone[i].value = combatPlayers[i].healDone;

            damageTaken[i] = { name: "", value: 0 };
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
        <div className="selected-combat__container_general-details-charts">
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

export default SelectedCombatChart;