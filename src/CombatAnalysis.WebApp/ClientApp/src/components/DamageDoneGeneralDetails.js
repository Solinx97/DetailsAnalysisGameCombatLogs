import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { RadialBarChart, RadialBar, Legend } from 'recharts';

import "../styles/damageDoneGeneralDetails.scss";

const DamageDoneGeneralDetails = () => {
    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [combatId, setCombatId] = useState(0);
    const [damageDoneRenderGeneral, setDamageDoneGeneralRender] = useState(null);
    const [showGeneralDetails, setShowGeneralDetails] = useState(true);
    const [initialData, setiInitialData] = useState([]);

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
        let damageDoneGeneralBarChartData = new Array(damageDoneGenerals.length); 
        for (var i = 0; i < damageDoneGenerals.length; i++) {
            let color = '#' + (Math.random().toString(16) + '000000').substring(2, 8).toUpperCase();
            let data = {
                name: damageDoneGenerals[i].spellOrItem,
                value: damageDoneGenerals[i].value,
                fill: color == "#fff" ? '#' + (Math.random().toString(16) + '000000').substring(2, 8).toUpperCase() : color
            };
            damageDoneGeneralBarChartData[i] = data;
        }

        setiInitialData(damageDoneGeneralBarChartData);
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
                    <li className="list-group-item">Всего урона: {element.value}</li>
                    <li className="list-group-item">Урон в секунду: {element.damagePerSecond}</li>
                    <li className="list-group-item">Количество кастов: {element.castNumber}</li>
                    <li className="list-group-item">Среднее значение: {element.averageValue}</li>
                    <li className="list-group-item">Количество критов: {element.critNumber}</li>
                    <li className="list-group-item">Количество промахов: {element.missNumber}</li>
                    <li className="list-group-item">Максимальное значение: {element.maxValue}</li>
                    <li className="list-group-item">Минимальное значение: {element.minValue}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={`/damage-done-details?id=${combatPlayerId}`}>Подробнее</NavLink>
                </div>
            </div>
        </li>;
    }

    const render = () => {
        return <div className="damage-done-general-details__container">
            <div className="damage-done-general-details__container_navigate">
                <h3>Общая информаця об уроне</h3>
                <div className="btn-group" role="group">
                    <button type="button" className="btn btn-primary" onClick={() => navigate(`/details-specifical-combat?id=${combatId}`)}>Выбор игрока</button>
                </div>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetails(!showGeneralDetails)} defaultChecked="true" />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать общую статистику</label>
            </div>
            {showGeneralDetails &&
                <div>
                    <RadialBarChart
                        width={500}
                        height={500}
                        cx={150}
                        cy={200}
                        innerRadius={20}
                        outerRadius={160}
                        barSize={20}
                        data={initialData}
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
                </div>
            }
            {damageDoneRenderGeneral}
        </div>;
    }

    return render();
}

export default DamageDoneGeneralDetails;