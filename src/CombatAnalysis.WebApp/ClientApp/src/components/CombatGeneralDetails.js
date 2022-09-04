import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { RadialBarChart, RadialBar, Legend } from 'recharts';
import DamageDoneDetails from './CombatDetails';
import useCombatDetailsHelper from './hooks/useCombatDetailsHelper';

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

    const combatDetailsHelperPayload = useCombatDetailsHelper(combatPlayerId);

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
                await getCombatPlayerAsync();
                await fillingGeneralDetailsList();
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

    const fillingGeneralDetailsList = async () => {
        const combatGeneralDetailsData = await combatDetailsHelperPayload.generalData(detailsType);

        if (combatGeneralDetailsData.length > 0) {
            let list = <div></div>;

            switch (detailsType) {
                case "DamageDone":
                    list = combatGeneralDetailsData.map((element) => combatDetailsHelperPayload.damageDone.generalList(element));
                    break;
                case "HealDone":
                    list = combatGeneralDetailsData.map((element) => combatDetailsHelperPayload.healDone.generalList(element));
                    break;
                case "DamageTaken":
                    list = combatGeneralDetailsData.map((element) => combatDetailsHelperPayload.damageTaken.generalList(element));
                    break;
                case "ResourceRecovery":
                    list = combatGeneralDetailsData.map((element) => combatDetailsHelperPayload.resourceRecovery.generalList(element));
                    break;
            }

            await createBarChartData(combatGeneralDetailsData);

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

    const createBarChartData = (combatGeneralDetailsData) => {
        let spellsRadialChartData = new Array(combatGeneralDetailsData.length);

        for (var i = 0; i < combatGeneralDetailsData.length; i++) {
            let color = '#' + (Math.random().toString(16) + '000000').substring(2, 8).toUpperCase();
            let spellsData = {
                name: combatGeneralDetailsData[i].spellOrItem,
                value: combatGeneralDetailsData[i].value,
                fill: color == "#fff" ? '#' + (Math.random().toString(16) + '00000  0').substring(2, 8).toUpperCase() : color
            };

            spellsRadialChartData[i] = spellsData;
        }

        setSpells(spellsRadialChartData);
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
                : <DamageDoneDetails detailsTypeName={getDetailsTypeName()} userName={userName} />
            }
        </div>;
    }

    return render();
}

export default CombatGeneralDetails;