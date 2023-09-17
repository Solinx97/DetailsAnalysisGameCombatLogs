import React, { useState } from "react";
import { useTranslation } from 'react-i18next';
import {
    PolarAngleAxis, PolarGrid, PolarRadiusAxis, Radar,
    RadarChart
} from "recharts";
import CommonPlayerInform from './CommonPlayerInform';

const PlayerInformation = ({ combatPlayers, combatId, combatLogId }) => {
    const { t } = useTranslation("childs/playerInformation");

    const [selectedCharts, setSelectedCharts] = useState([]);
    const [activeUserIndex, setActiveUserIndex] = useState(-1);

    const createUserRadarChartData = (playerData) => {
        return [
            {
                subject: t("Damage"),
                A: playerData.damageDone,
            },
            {
                subject: t("Healing"),
                A: playerData.healDone,
            },
            {
                subject: t("DamageTaken"),
                A: playerData.damageTaken,
            },
            {
                subject: t("ResourcesRecovery"),
                A: playerData.energyRecovery,
            },
            {
                subject: t("Buffs"),
                A: playerData.usedBuffs,
            },
        ];
    }

    const switchRadarChart = index => (event) => {
        for (let i = 0; i < selectedCharts.length; i++) {
            selectedCharts[i].checked = false;
            selectedCharts.pop();

            setActiveUserIndex(-1);
        }

        if (event.target.checked) {
            const charts = selectedCharts;
            charts.push(event.target);
            setSelectedCharts(charts);

            setActiveUserIndex(index);
        }
    }

    return (
        <ul>
            {combatPlayers?.map((item, index) => (
                    <li key={item.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">{item.userName}</h5>
                        </div>
                        <div className="form-check form-switch">
                            <input className="form-check-input" type="checkbox" role="switch" id={`flexSwitchCheckChecked-${index}`} onChange={switchRadarChart(index)} />
                            <label className="form-check-label" htmlFor={`flexSwitchCheckChecked-${index}`}>{t("ShowStatistics")}</label>
                        </div>
                        {activeUserIndex === index &&
                            <RadarChart
                                cx={350}
                                cy={250}
                                outerRadius={150}
                                width={600}
                                height={500}
                                data={createUserRadarChartData(item)}
                            >
                                <PolarGrid/>
                                <PolarAngleAxis
                                    dataKey="subject"
                                />
                                <PolarRadiusAxis/>
                                <Radar
                                    name={item.userName}
                                    dataKey="A"
                                    stroke="#8884d8"
                                    fill="#8884d8"
                                    fillOpacity={0.6}
                                />
                            </RadarChart>
                        }
                        <CommonPlayerInform
                            combatPlayer={item}
                            combatId={combatId}
                            combatLogId={combatLogId}
                        />
                    </li>
                ))}
        </ul>
    );
}

export default PlayerInformation;