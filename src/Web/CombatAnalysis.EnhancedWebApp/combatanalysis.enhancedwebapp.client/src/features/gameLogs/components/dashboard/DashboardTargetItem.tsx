import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatTargetModel } from '../../types/CombatTargetModel';
import type { CombatDetailsModel } from '../../types/dashboard/CombatDetailsModel';
import TopPlayersByTarget from './TopPlayersByTarget';

interface DashboardTargetItemProps {
    combatTarget: CombatTargetModel[];
    details: CombatDetailsModel;
    duration: number;
    detailsType: number;
    getValueShortName(value: number): string;
}

const DashboardTargetItem: React.FC<DashboardTargetItemProps> = ({ combatTarget, details, duration, detailsType, getValueShortName }) => {
    const minTopPlayersCount = 3;

    const { t } = useTranslation("combatDetails/dashboard");

    const navigate = useNavigate();

    const [sum, setSum] = useState(0);
    const [topPlayersCount, setTopPlayersCount] = useState(3);

    useEffect(() => {
        const sum = calculateSum(combatTarget);
        setSum(sum);
    }, []);

    const calculation = (combatTargetPlayerSum: number): string => {
        const playerContribution: number = combatTargetPlayerSum / sum;
        const playerContributionFixed: string = (playerContribution * 100).toFixed(2);

        return playerContributionFixed;
    }

    const calculationDamagePerTimeByTarget = (damage: number): string => {
        const valuePerTime = damage / duration;
        const shortValue = getValueShortName(parseInt(valuePerTime.toFixed(2)));

        return shortValue;
    }

    const calculateSum = (targetPlayers: CombatTargetModel[]): number => {
        const sum = targetPlayers.reduce((acc, player: CombatTargetModel) => acc + player.sum, 0);

        return sum;
    }

    const goToCombatGeneralDetails = (playerId: number): void => {
        navigate(`/combat-details?id=${details.id}&playerId=${playerId}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&tab=${0}&number=${details.number}&isWin=${details.isWin}`);
    }

    const updatePlayersList = () => {
        setTopPlayersCount(topPlayersCount === minTopPlayersCount ? combatTarget.length : minTopPlayersCount);
    }

    return (
        <>
            <div className="title">
                <div>{combatTarget[0].target}</div>
            </div>
            <TopPlayersByTarget
                calculation={calculation}
                calculationDamagePerTimeByTarget={calculationDamagePerTimeByTarget}
                goToCombatGeneralDetails={goToCombatGeneralDetails}
                getValueShortName={getValueShortName}
                targetTopPlayers={combatTarget.slice(0, topPlayersCount)}
            />
            {topPlayersCount === minTopPlayersCount 
                ? <div className="extend" onClick={updatePlayersList}>{t("More")}</div>
                : <div className="extend" onClick={updatePlayersList}>{t("Less")}</div>
            }
        </>
    );
}

export default memo(DashboardTargetItem);