import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { RadialBarChart, RadialBar, Legend } from 'recharts';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import DamageDoneDetails from './CombatDetails';
import { faHandFist, faGauge, faStopwatch20, faLocationCrosshairs, faMeteor, faShare, faCircleUp, faCircleDown } from '@fortawesome/free-solid-svg-icons';

import "../styles/combatGeneralDetails.scss";

const CombatGeneralDetails = () => {
    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [detailsType, setDetailsType] = useState("");
    const [userName, setUserName] = useState("");
    const [combatId, setCombatId] = useState(0);
    const [damageDoneRenderGeneral, setDamageDoneGeneralRender] = useState(null);
    const [showGeneralChart, setShowGeneralChart] = useState(false);
    const [spells, setSpells] = useState([]);
    const [tabIndex, setTabIndex] = useState(0);

    const style = {
        top: '50%',
        right: 0,
        transform: 'translate(0, -50%)',
        lineHeight: '24px',
    };

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
        setDetailsType(queryParams.get("detailsType"));
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getGeneralDetails = async () => {
                await getGeneralDetailsAsync();
            };

            getGeneralDetails();
        }
    }, [combatPlayerId]);

    const getDetailsTypeName = () => {
        let name = "";

        switch (detailsType) {
            case "DamageDone":
                name = "Нанесенный урон";
                break;
            case "HealDone":
                name = "Исцеление";
                break;
            case "DamageTaken":
                name = "Полученный урон";
                break;
            case "ResourceRecovery":
                name = "Восполнено ресурсов";
                break;
        }

        return name;
    }

    const getGeneralDetailsAsync = async () => {
        const response = await fetch(`${detailsType}General/${combatPlayerId}`);
        const generalDetailsData = await response.json();

        createBarChartData(generalDetailsData);

        await getCombatPlayerAsync();

        fillingGeneralDetailsList(generalDetailsData);
    }

    const getCombatPlayerAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/combatPlayerById/${combatPlayerId}`);
        const combatPlayer = await response.json();

        setUserName(combatPlayer.userName);

        await getCombatsAsync(combatPlayer.combatId);
    }

    const getCombatsAsync = async (id) => {
        const response = await fetch(`detailsSpecificalCombat/combatById/${id}`);
        const combat = await response.json();

        setCombatId(combat.id);
    }

    const createBarChartData = (generalDetailsData) => {
        let spellsRadialChartData = new Array(generalDetailsData.length);

        for (var i = 0; i < generalDetailsData.length; i++) {
            let color = '#' + (Math.random().toString(16) + '000000').substring(2, 8).toUpperCase();
            let spellsData = {
                name: generalDetailsData[i].spellOrItem,
                value: generalDetailsData[i].value,
                fill: color == "#fff" ? '#' + (Math.random().toString(16) + '00000  0').substring(2, 8).toUpperCase() : color
            };

            spellsRadialChartData[i] = spellsData;
        }

        setSpells(spellsRadialChartData);
    }

    const fillingGeneralDetailsList = (generalDetailsData) => {
        if (generalDetailsData.length > 0) {
            let list = <div></div>;

            switch (detailsType) {
                case "DamageDone":
                    list = generalDetailsData.map((element) => damageDoneList(element));
                    break;
                case "HealDone":
                    list = generalDetailsData.map((element) => healDoneList(element));
                    break;
                case "DamageTaken":
                    list = generalDetailsData.map((element) => damageTakenList(element));
                    break;
                case "ResourceRecovery":
                    list = generalDetailsData.map((element) => resourceRecoveryList(element));
                    break;
            }

            setDamageDoneGeneralRender(
                <ul>
                    {list}
                </ul>
            );
        }
        else {
            setDamageDoneGeneralRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const damageDoneList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего урона" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.damagePerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title="Кол-во критов" />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShare} className="list-group-item__miss-number" title="Кол-во промахов" />
                        <div>{element.missNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const healDoneList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего исцеления" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.healPerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title="Кол-во критов" />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const damageTakenList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего урона" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.damageTakenPerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title="Кол-во критов" />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShare} className="list-group-item__miss-number" title="Кол-во промахов" />
                        <div>{element.missNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const resourceRecoveryList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего урона" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.resourcePerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const generalDetailsDOM = () => {
        return <div>
            <div>
                <h3>Общая информация [{getDetailsTypeName()}]</h3>
                <h4>Игрок: {userName}</h4>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralChart(!showGeneralChart)} defaultChecked={showGeneralChart} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать диаграмму</label>
            </div>
            {showGeneralChart &&
                <div className="general-details__container_radial-chart">
                    <RadialBarChart
                        width={500}
                        height={450}
                        cx={150}
                        cy={200}
                        innerRadius={20}
                        outerRadius={160}
                        barSize={20}
                        data={spells}
                    >
                        <RadialBar
                            minAngle={15}
                            label={{ position: "insideStart", fill: "#fff", fontSize: "13px" }}
                            background
                            clockWise
                            dataKey="value"
                        />
                        <Legend
                            iconSize={15}
                            width={120}
                            height={400}
                            layout="vertical"
                            verticalAlign="middle"
                            wrapperStyle={style}
                        />
                    </RadialBarChart>
                    <div className="title">Заклинания</div>
                </div>
            }
            {damageDoneRenderGeneral}
        </div>;
    }

    const render = () => {
        return <div className="general-details__container">
            <div className="general-details__container_navigate">
                <div className="btn-group" role="group">
                    <button type="button" className="btn btn-primary" onClick={() => navigate(`/details-specifical-combat?id=${combatId}`)}>Выбор игрока</button>
                </div>
                <ul className="nav nav-tabs">
                    <li className="nav-item">
                        <a className={tabIndex == 0 ? "nav-link active" : "nav-link"} aria-current="page" onClick={() => setTabIndex(0)}>Общая информация</a>
                    </li>
                    <li className="nav-item">
                        <a className={tabIndex == 1 ? "nav-link active" : "nav-link"} onClick={() => setTabIndex(1)}>Подробная информация</a>
                    </li>
                </ul>
            </div>
            {tabIndex == 0
                ? generalDetailsDOM()
                : <DamageDoneDetails detailsTypeName={getDetailsTypeName()}/>
            }
        </div>;
    }

    return render();
}

export default CombatGeneralDetails;