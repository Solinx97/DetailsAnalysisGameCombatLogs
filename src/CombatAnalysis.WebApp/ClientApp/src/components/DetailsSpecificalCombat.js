import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import usePieChart from './hooks/usePieChart';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBoxOpen, faPlusCircle, faShieldHalved, faHandFist, faBolt, faCircleNodes } from '@fortawesome/free-solid-svg-icons';
import {
    Radar,
    RadarChart,
    PolarGrid,
    PolarAngleAxis,
    PolarRadiusAxis,
} from "recharts";

import "../styles/detailsSpecificalCombat.scss";

const DetailsSpecificalCombat = () => {
    const navigate = useNavigate();

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [activeUserIndex, setActiveUserIndex] = useState(-1);
    const [combatPlayers, setCombatPlayers] = useState([]);
    const [showGeneralDetails, setShowGeneralDetails] = useState(false);
    const [combatPlayersRender, setCombatPlayersRender] = useState(null);
    const [selectedCharts, setSelectedCharts] = useState([]);

    const [damageDonePieChart, setDamageDonePieChart] = usePieChart({});
    const [healDonePieChart, setHealDonePieChart] = usePieChart({});
    const [damageTakenPieChart, setDamageTakenPieChart] = usePieChart({});

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatId > 0) {
            const getCombatPlayers = async () => {
                await getCombatPlayersAsync();
            };

            getCombatPlayers();
        }
    }, [combatId]);

    useEffect(() => {
        if (combatPlayers.length > 0) {
            fillingCombatPlayerList(combatPlayers);
        }
    }, [activeUserIndex]);

    useEffect(() => {
        if (combatPlayers.length > 0) {
            var data = createPieChardData();

            setDamageDonePieChart({
                title: "Урон",
                color: "blue",
                data: data.damageDone
            });
            setHealDonePieChart({
                title: "Исцеление",
                color: "green",
                data: data.healDone
            });
            setDamageTakenPieChart({
                title: "Полученный урон",
                color: "orange",
                data: data.damageTaken
            });
        }
    }, [combatPlayers]);

    const getCombatPlayersAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/combatPlayersByCombatId/${combatId}`);
        const combatPlayersData = await response.json();
        setCombatPlayers(combatPlayersData);

        await getCombatsAsync();

        fillingCombatPlayerList(combatPlayersData);
    }

    const getCombatsAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/combatById/${combatId}`);
        const combat = await response.json();

        setCombatLogId(combat.combatLogId);
    }

    const fillingCombatPlayerList = (combatPlayers) => {
        if (combatPlayers.length > 0) {
            const list = combatPlayers.map((element, index) => combatPlayerList(element, index));

            setCombatPlayersRender(
                <ul className="combat-players__container">
                    {list}
                </ul>
            );
        }
        else {
            setCombatPlayersRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const createUserRadarChartData = (playerData) => {
        return [
            {
                subject: "Урон",
                A: playerData.damageDone,
            },
            {
                subject: "Исцеление",
                A: playerData.healDone,
            },
            {
                subject: "Полученный урон",
                A: playerData.damageTaken,
            },
            {
                subject: "Ресурсы",
                A: playerData.energyRecovery,
            },
            {
                subject: "Бафы",
                A: playerData.usedBuffs,
            },
        ];
    }

    const createPieChardData = () => {
        let damageDone = [];
        let healDone = [];
        let damageTaken = [];

        if (combatPlayers != null) {
            damageDone = new Array(combatPlayers.length);
            for (var i = 0; i < combatPlayers.length; i++) {
                let realmNameIndex = combatPlayers[i].userName.indexOf('-');
                let userName = combatPlayers[i].userName.substr(0, realmNameIndex);

                damageDone[i] = {};
                damageDone[i].name = userName
                damageDone[i].value = combatPlayers[i].damageDone;

                healDone[i] = {};
                healDone[i].name = userName
                healDone[i].value = combatPlayers[i].healDone;

                damageTaken[i] = {};
                damageTaken[i].name = userName
                damageTaken[i].value = combatPlayers[i].damageTaken;
            }
        }

        return {
            damageDone: damageDone,
            healDone: healDone,
            damageTaken: damageTaken
        };
    }

    const switchRadarChart = (index, event) => {
        for (var i = 0; i < selectedCharts.length; i++) {
            selectedCharts[i].checked = false;
            selectedCharts.pop();

            setActiveUserIndex(-1);
        }

        if (event.target.checked) {
            let charts = selectedCharts;
            charts.push(event.target);
            setSelectedCharts(charts);

            setActiveUserIndex(index);
        }
    }

    const combatPlayerList = (element, index) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.userName}</h5>
                </div>
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => switchRadarChart(index, event)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать статистику</label>
                </div>
                {activeUserIndex == index &&
                    <RadarChart
                        cx={350}
                        cy={250}
                        outerRadius={150}
                        width={600}
                        height={500}
                        data={createUserRadarChartData(element)}
                    >
                        <PolarGrid />
                        <PolarAngleAxis dataKey="subject" />
                        <PolarRadiusAxis />
                        <Radar
                            name={element.userName}
                            dataKey="A"
                            stroke="#8884d8"
                            fill="#8884d8"
                            fillOpacity={0.6}
                        />
                    </RadarChart>
                }
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__damage-done" title="Нанесенный урон" />
                        <div>{element.damageDone}</div>
                        {element.damageDone > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                            onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=DamageDone`)} title="Открыть анализ урон" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faPlusCircle} className="list-group-item__heal-done" title="Исцеление" />
                        <div>{element.healDone}</div>
                        {element.healDone > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                                onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=HealDone`)} title="Открыть анализ исцеления" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShieldHalved} className="list-group-item__damage-taken" title="Полученный урон" />
                        <div>{element.damageTaken}</div>
                        {element.damageTaken > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                                onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=DamageTaken`)} title="Открыть анализ полученного урона" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faBolt} className="list-group-item__energy-recovery" title="Затрачено ресурсов" />
                        <div>{element.energyRecovery}</div>
                        {element.energyRecovery > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                                onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=ResourceRecovery`)} title="Открыть анализ затраченных ресурсов" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleNodes} className="list-group-item__used-buffs" title="Бафы" />
                        <div>{element.usedBuffs}</div>
                        {element.usedBuffs > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                            onClick={() => navigate(`/combat-general-details?id=${element.id}`)} title="Открыть анализ бафов" />
                        }
                    </li>
                </ul>
            </div>
        </li>;
    }

    const render = () => {
        return <div className="details-specifical-combat__container">
            <div className="details-specifical-combat__container_navigate">
                <h3>Игроки, участвующие в бою</h3>
                <div className="btn-group" role="group">
                    <button type="button" className="btn btn-primary" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>Выбор боя</button>
                </div>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetails(!showGeneralDetails)} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать общую статистику</label>
            </div>
            {showGeneralDetails &&
                <div className="details-specifical-combat__container_general-details-charts">
                    {damageDonePieChart()}
                    {healDonePieChart()}
                    {damageTakenPieChart()}
                </div>
            }
            {combatPlayersRender}
        </div>;
    }

    return render();
}

export default DetailsSpecificalCombat;