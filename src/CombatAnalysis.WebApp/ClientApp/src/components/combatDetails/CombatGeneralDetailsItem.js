import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Legend, RadialBar, RadialBarChart } from 'recharts';
import useCombatGeneralData from '../../hooks/useCombatGeneralData';

const CombatGeneralDetailsItem = ({ combatPlayerUsername, combatPlayerId, detailsType, detailsTypeName }) => {
    const { t, i18n } = useTranslation("combatGeneralDetails");

    const [spells, setSpells] = useState([]);
    const [showGeneralChart, setShowGeneralChart] = useState(false);
    const [playerDataDetailsRender, setPlayerDataDetailsRender] = useState(<></>);

    const [getGeneralListAsync, getPlayerGeneralDetailsAsync] = useCombatGeneralData(combatPlayerId, detailsType);

    const style = {
        top: '50%',
        right: 0,
        transform: 'translate(0, -50%)',
        lineHeight: '24px',
    };

    useEffect(() => {
        const getGeneralDetails = async () => {
            await getDetailsAsync(combatPlayerId);
        }

        getGeneralDetails();
    }, [])

    const getDetailsAsync = async (combatPlayerId) => {
        const dataRender = await getGeneralListAsync(combatPlayerId, detailsType);
        if (dataRender !== undefined) {
            setPlayerDataDetailsRender(dataRender);
        }

        const data = await getPlayerGeneralDetailsAsync(combatPlayerId, detailsType);
        if (data !== undefined) {
            await createBarChartData(data);
        }
    }

    const createBarChartData = (combatGeneralDetailsData) => {
        const spellsRadialChartData = new Array(combatGeneralDetailsData.length);

        for (let i = 0; i < combatGeneralDetailsData.length; i++) {
            const color = '#' + (Math.random().toString(16) + '000000').substring(2, 8).toUpperCase();
            const spellsData = {
                name: combatGeneralDetailsData[i].spellOrItem,
                value: combatGeneralDetailsData[i].value,
                fill: color === "#fff" ? '#' + (Math.random().toString(16) + '00000  0').substring(2, 8).toUpperCase() : color
            };

            spellsRadialChartData[i] = spellsData;
        }

        setSpells(spellsRadialChartData);
    }

    return (
        <div>
            <div>
                <h3>{t("CommonInform")} [{detailsTypeName}]</h3>
                <h4>{t("Player")}: {combatPlayerUsername}</h4>
            </div>
            {spells.length > 0 &&
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralChart((item) => !item)} defaultChecked={showGeneralChart} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowDiagram")}</label>
                </div>
            }
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
                    <div className="title">{t("Skills")}</div>
                </div>
            }
            <ul>
                {playerDataDetailsRender}
            </ul>
        </div>
    );
}

export default CombatGeneralDetailsItem;