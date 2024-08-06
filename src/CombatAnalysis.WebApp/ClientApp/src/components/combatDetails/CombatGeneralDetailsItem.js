import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Brush, CartesianGrid, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';
import useCombatGeneralData from '../../hooks/useCombatGeneralData';

const CombatGeneralDetailsItem = ({ combatPlayerId, detailsType }) => {
    const { t } = useTranslation("combatDetails/combatGeneralDetails");

    const [spells, setSpells] = useState([]);
    const [showGeneralChart, setShowGeneralChart] = useState(false);
    const [playerDataDetailsRender, setPlayerDataDetailsRender] = useState(<></>);

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const [getGeneralListAsync, getPlayerGeneralDetailsAsync] = useCombatGeneralData(combatPlayerId, detailsType);

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
            createBarChartData(data);
        }
    }

    const createBarChartData = (combatGeneralDetailsData) => {
        const spellsRadialChartData = new Array(combatGeneralDetailsData.length);

        for (let i = 0; i < combatGeneralDetailsData.length; i++) {
            const color = '#' + (Math.random().toString(16) + '000000').substring(2, 8).toUpperCase();
            const spellsData = {
                name: combatGeneralDetailsData[i].spellOrItem,
                uv: combatGeneralDetailsData[i].value,
                fill: color === "#fff" ? '#' + (Math.random().toString(16) + '00000  0').substring(2, 8).toUpperCase() : color
            };

            spellsRadialChartData[i] = spellsData;
        }

        setSpells(spellsRadialChartData);
    }

    return (
        <div>
            {(spells.length > 0 && screenSize.width > maxWidth) &&
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralChart((item) => !item)} defaultChecked={showGeneralChart} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowDiagram")}</label>
                </div>
            }
            {showGeneralChart &&
                <div className="general-details__radial-chart">
                    <ResponsiveContainer width="100%" height={200}>
                        <LineChart
                            width={500}
                            height={200}
                            data={spells}
                            syncId="anyId"
                            margin={{
                                top: 10,
                                right: 30,
                                left: 0,
                                bottom: 0,
                            }}
                        >
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="name" />
                            <YAxis />
                            <Tooltip />
                            <Line type="monotone" dataKey="uv" stroke="#58A399" fill="#6196A6" />
                            <Brush />
                        </LineChart>
                    </ResponsiveContainer>
                    <div className="title">{t("Skills")}</div>
                </div>
            }
            <ul className="player-general-data-details">
                {playerDataDetailsRender}
            </ul>
        </div>
    );
}

export default CombatGeneralDetailsItem;