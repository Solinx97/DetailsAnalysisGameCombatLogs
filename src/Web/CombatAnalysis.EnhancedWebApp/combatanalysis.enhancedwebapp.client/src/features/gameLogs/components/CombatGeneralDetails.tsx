import { useEffect, useState, type JSX } from 'react';
import { useTranslation } from 'react-i18next';
import { Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';
import useCombatGeneralData from '../hooks/useCombatGeneralData';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { DamageDoneGeneralModel } from '../types/DamageDoneGeneralModel';
import type { DamageTakenGeneralModel } from '../types/DamageTakenGeneralModel';
import type { HealDoneGeneralModel } from '../types/HealDoneGeneralModel';
import type { ResourceRecoveryGeneralModel } from '../types/ResourceRecoveryGeneralModel';
import type { SpellsDataModel } from '../types/SpellsDataModel';
import CustomTooltip from './CustomTooltip';

interface CombatGeneralDetailsProps {
    combatPlayer: CombatPlayerModel;
    detailsType: number;
}

const CombatGeneralDetails: React.FC<CombatGeneralDetailsProps> = ({ combatPlayer, detailsType }) => {
    const { t } = useTranslation("combatDetails/combatGeneralDetails");

    const [spells, setSpells] = useState<SpellsDataModel[]>([]);
    const [showGeneralChart, setShowGeneralChart] = useState(false);
    const [playerDataDetailsRender, setPlayerDataDetailsRender] = useState<JSX.Element>();

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const [getGeneralListAsync, getPlayerGeneralDetailsAsync] = useCombatGeneralData(combatPlayer, +detailsType);

    useEffect(() => {
        const getGeneralDetails = async () => {
            await getDetailsAsync();
        }

        getGeneralDetails();
    }, []);

    const getDetailsAsync = async () => {
        const dataRender = await getGeneralListAsync();
        if (dataRender) {
            setPlayerDataDetailsRender(dataRender);
        }

        const playerGeneralDetails = await getPlayerGeneralDetailsAsync();
        createBarChartData(playerGeneralDetails);
    }

    const createBarChartData = (combatGeneralDetailsData: DamageDoneGeneralModel[] | DamageTakenGeneralModel[] | ResourceRecoveryGeneralModel[] | HealDoneGeneralModel[] | null) => {
        if (!combatGeneralDetailsData) {
            return;
        }

        const spellsRadialChartData = new Array(combatGeneralDetailsData.length);

        for (let i = 0; i < combatGeneralDetailsData.length; i++) {
            const spellsData: SpellsDataModel = {
                name: combatGeneralDetailsData[i].spell,
                value: combatGeneralDetailsData[i].value,
            };

            spellsRadialChartData[i] = spellsData;
        }

        setSpells(spellsRadialChartData);
    }

    return (
        <div className="details__container">
            {(spells.length > 0 && screenSize.width > maxWidth) &&
                <div className="form-switch">
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
                            <XAxis dataKey="name" stroke="#FFFFFF" style={{ fontSize: "0.9em", overflow: "auto" }} />
                            <YAxis dataKey="value" stroke="#FFFFFF" />
                            <Tooltip content={<CustomTooltip payload={spells} t={t} />} />
                            <Line type="monotone" dataKey="value" stroke="#58A399" fill="#6196A6" />
                        </LineChart>
                    </ResponsiveContainer>
                </div>
            }
            <ul className="player-general-data-details">
                {playerDataDetailsRender}
            </ul>
        </div>
    );
}

export default CombatGeneralDetails;