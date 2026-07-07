import { useEffect, useState, type JSX } from 'react';
import { useTranslation } from 'react-i18next';
import useCombatGeneralData from '../hooks/useCombatGeneralData';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { DamageDoneGeneralModel } from '../types/DamageDoneGeneralModel';
import type { DamageTakenGeneralModel } from '../types/DamageTakenGeneralModel';
import type { HealDoneGeneralModel } from '../types/HealDoneGeneralModel';
import type { ResourceRecoveryGeneralModel } from '../types/ResourceRecoveryGeneralModel';
import type { SpellsDataModel } from '../types/SpellsDataModel';
import CombatPlayerGenericChart from './charts/CombatPlayerGenericChart';
import { useGetCombatPlayerChartDamageDoneQuery } from '../api/DamageDone.api';
import { useGetCombatPlayerChartDamageTakenQuery } from '../api/DamageTaken.api';
import { useGetCombatPlayerChartHealDoneQuery } from '../api/HealDone.api';
import { useGetCombatPlayerChartResourceRecoveryQuery } from '../api/ResourcesRecovery.api';
import CombatPreAuraItem from './auras/CombatPreAuraItem';

interface CombatGeneralDetailsProps {
    combatPlayer: CombatPlayerModel;
    detailsType: number;
}

const CombatGeneralDetails: React.FC<CombatGeneralDetailsProps> = ({ combatPlayer, detailsType }) => {
    const { t } = useTranslation("combatDetails/combatGeneralDetails");

    const [spells, setSpells] = useState<SpellsDataModel[]>([]);
    const [showGenericChart, setShowGenericChart] = useState(false);
    const [playerDataDetailsRender, setPlayerDataDetailsRender] = useState<JSX.Element>();

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const [getGeneralListAsync, getPlayerGeneralDetailsAsync] = useCombatGeneralData(combatPlayer, detailsType);

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

    const getChart = () => {
        switch (detailsType) {
            case 0:
                return <CombatPlayerGenericChart
                    combatPlayerId={combatPlayer.id}
                    name={t("DPS")}
                    useGetChartQuery={useGetCombatPlayerChartDamageDoneQuery}
                />
            case 1:
                return <CombatPlayerGenericChart
                    combatPlayerId={combatPlayer.id}
                    name={t("HPS")}
                    useGetChartQuery={useGetCombatPlayerChartHealDoneQuery}
                />
            case 2:
                return <CombatPlayerGenericChart
                    combatPlayerId={combatPlayer.id}
                    name={t("DamageTaken")}
                    useGetChartQuery={useGetCombatPlayerChartDamageTakenQuery}
                />
            case 3:
                return <CombatPlayerGenericChart
                    combatPlayerId={combatPlayer.id}
                    name={t("ResourcesRecovery")}
                    useGetChartQuery={useGetCombatPlayerChartResourceRecoveryQuery}
                />
            default:
                return <CombatPlayerGenericChart
                    combatPlayerId={combatPlayer.id}
                    name={t("DPS")}
                    useGetChartQuery={useGetCombatPlayerChartDamageDoneQuery}
                />
        }
    }

    return (
        <div className="details__container">
            <CombatPreAuraItem
                combatPlayerId={combatPlayer.id}
                combatId={combatPlayer.combatId}
            />
            {(spells.length > 0 && screenSize.width > maxWidth) &&
                <div className="form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGenericChart((item) => !item)} defaultChecked={showGenericChart} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowDiagram")}</label>
                </div>
            }
            {showGenericChart && getChart()}
            <ul className="player-general-data-details">
                {playerDataDetailsRender}
            </ul>
        </div>
    );
}

export default CombatGeneralDetails;