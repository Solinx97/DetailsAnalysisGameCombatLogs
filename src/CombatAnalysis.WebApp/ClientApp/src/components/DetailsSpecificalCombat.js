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
    const [activeUserIndex, setActiveUserIndex] = useState(0);
    const [combatPlayers, setCombatPlayers] = useState({});
    const [showRadarChart, setShowRadarChart] = useState(false);
    const [combatPlayersRender, setCombatPlayersRender] = useState(null);

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
        if (combatPlayers != null) {
            fillingCombatPlayerList(combatPlayers);
        }
    }, [showRadarChart]);

    useEffect(() => {
        if (combatPlayers.length > 0) {
            setDamageDonePieChart({
                title: "Урон",
                color: "blue",
                data: createDamageDonePieChardData()
            });
            setHealDonePieChart({
                title: "Исцеление",
                color: "green",
                data: createHealDonePieChardData()
            });
            setDamageTakenPieChart({
                title: "Полученный урон",
                color: "orange",
                data: createDamageTakenPieChardData()
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

    const createDamageDonePieChardData = () => {
        let data = [];
        if (combatPlayers != null) {
            data = new Array(combatPlayers.length);
            for (var i = 0; i < combatPlayers.length; i++) {
                let realmNameIndex = combatPlayers[i].userName.indexOf('-');
                data[i] = {};
                data[i].name = combatPlayers[i].userName.substr(0, realmNameIndex);
                data[i].value = combatPlayers[i].damageDone;
            }
        }

        return data;
    }

    const createHealDonePieChardData = () => {
        let data = [];
        if (combatPlayers != null) {
            data = new Array(combatPlayers.length);
            for (var i = 0; i < combatPlayers.length; i++) {
                let realmNameIndex = combatPlayers[i].userName.indexOf('-');
                data[i] = {};
                data[i].name = combatPlayers[i].userName.substr(0, realmNameIndex);
                data[i].value = combatPlayers[i].healDone;
            }
        }

        return data;
    }

    const createDamageTakenPieChardData = () => {
        let data = [];
        if (combatPlayers != null) {
            data = new Array(combatPlayers.length);
            for (var i = 0; i < combatPlayers.length; i++) {
                let realmNameIndex = combatPlayers[i].userName.indexOf('-');
                data[i] = {};
                data[i].name = combatPlayers[i].userName.substr(0, realmNameIndex);
                data[i].value = combatPlayers[i].damageTaken;
            }
        }

        return data;
    }

    const switchRadarChart = (index) => {
        setActiveUserIndex(index);
        setShowRadarChart(!showRadarChart);
    }

    const combatPlayerList = (element, index) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.userName}</h5>
                </div>
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" id="flexSwitchCheckChecked" onChange={() => switchRadarChart(index)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать график</label>
                </div>
                {(showRadarChart && activeUserIndex == index) &&
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
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details" onClick={() => navigate(`/damage-done-details?id=${element.id}`)} title="Открыть подробный анализ урон" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faPlusCircle} className="list-group-item__heal-done" title="Исцеление" />
                        <div>{element.healDone}</div>
                        {element.healDone > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details" onClick={() => navigate(`/heal-done-details?id=${element.id}`)} title="Открыть подробный анализ исцеления" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShieldHalved} className="list-group-item__damage-taken" title="Полученный урон" />
                        <div>{element.damageTaken}</div>
                        {element.damageTaken > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details" onClick={() => navigate(`/damage-taken-details?id=${element.id}`)} title="Открыть подробный анализ полученного урона" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faBolt} className="list-group-item__energy-recovery" title="Затрачено ресусрво" />
                        <div>{element.energyRecovery}</div>
                        {element.energyRecovery > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details" onClick={() => navigate(`/resource-recovery-details?id=${element.id}`)} title="Открыть подробный анализ затраченных ресурсов" />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleNodes} className="list-group-item__used-buffs" title="Бафы" />
                        <div>{element.usedBuffs}</div>
                        {element.usedBuffs > 0 &&
                            <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details" onClick={() => navigate(`/buffs-details-details?id=${element.id}`)} title="Открыть подробный анализ бафов" />
                        }
                    </li>
                </ul>
            </div>
        </li>;
    }

    const render = () => {
        return <div className="details-specifical-combat__container">
            <h2>Игроки, участвующие в бою</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>Выбор боя</button>
            <div className="details-specifical-combat__container_general-details-charts">
                {damageDonePieChart}
                {healDonePieChart}
                {damageTakenPieChart}
            </div>
            {combatPlayersRender}
        </div>;
    }

    return render();
}

export default DetailsSpecificalCombat;