import { faBolt, faBoxOpen, faCircleNodes, faHandFist, faPlusCircle, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useMemo, useState } from "react";
import { useNavigate } from 'react-router-dom';
import {
    PolarAngleAxis, PolarGrid, PolarRadiusAxis, Radar,
    RadarChart
} from "recharts";

const DetailsPlayer = ({ combatPlayers }) => {
    const compatPlayersMemo = useMemo(() => {
        return combatPlayers;
    }, [combatPlayers])

    const navigate = useNavigate();

    const [combatPlayersRender, setCombatPlayersRender] = useState(<></>);
    const [selectedCharts, setSelectedCharts] = useState([]);
    const [activeUserIndex, setActiveUserIndex] = useState(-1);

    useEffect(() => {
        if (compatPlayersMemo.length === 0) {
            return;
        }

        fillingCombatPlayerList();
    }, [compatPlayersMemo, activeUserIndex]);

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

    const switchRadarChart = index => (event) => {
        for (let i = 0; i < selectedCharts.length; i++) {
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

    const fillingCombatPlayerList = () => {
        if (compatPlayersMemo.length === 0) {
            setCombatPlayersRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
            return;
        }

        const list = compatPlayersMemo.map((element, index) => combatPlayerList(element, index));

        setCombatPlayersRender(
            <ul className="combat-players__container">
                {list}
            </ul>
        );
    }

    const combatPlayerList = (element, index) => {
        return (<li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.userName}</h5>
                </div>
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={switchRadarChart(index)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать статистику</label>
                </div>
                {activeUserIndex === index &&
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
        </li>);
    }

    const render = () => {
        return (combatPlayersRender);
    }

    return render();
}

export default DetailsPlayer;