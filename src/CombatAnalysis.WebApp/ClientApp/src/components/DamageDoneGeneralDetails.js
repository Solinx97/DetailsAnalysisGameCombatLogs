import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { RadialBarChart, RadialBar, Legend } from 'recharts';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import DamageDoneDetails from './DamageDoneDetails';
import { faHandFist, faGauge, faStopwatch20, faLocationCrosshairs, faMeteor, faShare, faCircleUp, faCircleDown } from '@fortawesome/free-solid-svg-icons';

import "../styles/damageDoneGeneralDetails.scss";

const DamageDoneGeneralDetails = () => {
    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
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
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getDamageDoneGenerals = async () => {
                await getDamageDoneGeneralsAsync();
            };

            getDamageDoneGenerals();
        }
    }, [combatPlayerId]);

    const getDamageDoneGeneralsAsync = async () => {
        const response = await fetch(`damageDoneGeneral/${combatPlayerId}`);
        const damageDoneGenerals = await response.json();

        createBarChartData(damageDoneGenerals);

        await getCombatPlayerAsync();

        fillingDamageDoneGeneralList(damageDoneGenerals);
    }

    const getCombatPlayerAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/combatPlayerById/${combatPlayerId}`);
        const combatPlayer = await response.json();

        await getCombatsAsync(combatPlayer.combatId);
    }

    const getCombatsAsync = async (id) => {
        const response = await fetch(`detailsSpecificalCombat/combatById/${id}`);
        const combat = await response.json();

        setCombatId(combat.id);
    }

    const createBarChartData = (damageDoneGenerals) => {
        let spellsRadialChartData = new Array(damageDoneGenerals.length);

        for (var i = 0; i < damageDoneGenerals.length; i++) {
            let color = '#' + (Math.random().toString(16) + '000000').substring(2, 8).toUpperCase();
            let spellsData = {
                name: damageDoneGenerals[i].spellOrItem,
                value: damageDoneGenerals[i].value,
                fill: color == "#fff" ? '#' + (Math.random().toString(16) + '00000  0').substring(2, 8).toUpperCase() : color
            };

            spellsRadialChartData[i] = spellsData;
        }

        setSpells(spellsRadialChartData);
    }

    const fillingDamageDoneGeneralList = (damageDoneGenerals) => {
        if (damageDoneGenerals.length > 0) {
            const list = damageDoneGenerals.map((element) => damageDoneGeneralList(element));

            setDamageDoneGeneralRender(
                <ul className="damage-done__container">
                    {list}
                </ul>
            );
        }
        else {
            setDamageDoneGeneralRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const damageDoneGeneralList = (element) => {
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

    const damageDoneGeneralDetailsDOM = () => {
        return <div>
            <div>
                <h3>Общая информаця об уроне</h3>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralChart(!showGeneralChart)} defaultChecked={showGeneralChart} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать диаграмму</label>
            </div>
            {showGeneralChart &&
                <div className="damage-done-general-details__container_radial-chart">
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
                    <div className="title">Урон от заклинаний</div>
                </div>
            }
            {damageDoneRenderGeneral}
        </div>;
    }

    const render = () => {
        return <div className="damage-done-general-details__container">
            <div className="damage-done-general-details__container_navigate">
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
                ? damageDoneGeneralDetailsDOM()
                : <DamageDoneDetails />
            }
        </div>;
    }

    return render();
}

export default DamageDoneGeneralDetails;