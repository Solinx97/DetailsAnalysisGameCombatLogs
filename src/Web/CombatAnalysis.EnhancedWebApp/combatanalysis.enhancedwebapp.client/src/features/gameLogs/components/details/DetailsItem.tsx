import { faUser, faBookDead, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { UnitInfoModel } from '../../types/UnitInfoModel';
import DetailsItemParam from './DetailsItemParam';

interface DetailsItemProps {
    avgilvl: number;
    playerId: number;
    unitInfo: UnitInfoModel;
    details: CombatDetailsModel;
    getValueShortName(value: number): string;
    deathCount: number;
}

const DetailsItem: React.FC<DetailsItemProps> = ({ avgilvl, playerId, unitInfo, details, getValueShortName, deathCount }) => {
    const { t } = useTranslation('childs/playerInformation');

    const navigate = useNavigate();

    const navigateToDetails = (detailsType: number) => {
        navigate(`/combat-details?id=${details.id}&playerId=${playerId}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&number=${details.number}&isWin=${details.isWin}&duration=${details.duration}&gameVersion=${details.gameVersion}`);
    }

    const navigateToDeathetails = () => {
        navigate(`/player-dieth-details?id=${details.id}&playerId=${playerId}&unitId=${unitInfo.unitId}&combatLogId=${details.combatLogId}&name=${details.name}&number=${details.number}&isWin=${details.isWin}&duration=${details.duration}&gameVersion=${details.gameVersion}`);
    }

    return (
        <ul className="details__item">
            <li className="list-group-item">
                <DetailsItemParam
                    topName={t("Damage")}
                    topValue={getValueShortName(Math.round(unitInfo.damageDone))}
                    bottomName={t("DPS")}
                    bottomValue={getValueShortName(Math.round(unitInfo.damageDone / details.duration))}
                    title={t("OpenDamageAnalyzing") || ""}
                    isActive={unitInfo.damageDone > 0}
                    navigate={() => navigateToDetails(0)}
                />
            </li>
            <li className="list-group-item">
                <DetailsItemParam
                    topName={t("Healing")}
                    topValue={getValueShortName(Math.round(unitInfo.healDone))}
                    bottomName={t("HPS")}
                    bottomValue={getValueShortName(Math.round(unitInfo.healDone / details.duration))}
                    title={t("OpenHealingAnalyzing") || ""}
                    isActive={unitInfo.healDone > 0}
                    navigate={() => navigateToDetails(1)}
                />
            </li>
            <li className="list-group-item">
                <DetailsItemParam
                    topName={t("DamageTaken")}
                    topValue={getValueShortName(Math.round(unitInfo.damageTaken))}
                    bottomName={t("DTPS")}
                    bottomValue={getValueShortName(Math.round(unitInfo.damageTaken / details.duration))}
                    title={t("OpenDamageTakenAnalyzing") || ""}
                    isActive={unitInfo.damageTaken > 0}
                    navigate={() => navigateToDetails(2)}
                />
            </li>
            <li className="list-group-item">
                <DetailsItemParam
                    topName={t("ResourcesRecovery")}
                    topValue={getValueShortName(Math.round(unitInfo.resourcesRecovery))}
                    bottomName={t("RPS")}
                    bottomValue={getValueShortName(Math.round(unitInfo.resourcesRecovery / details.duration))}
                    title={t("OpenResourcesRecoveryAnalyzing") || ""}
                    isActive={unitInfo.resourcesRecovery > 0}
                    navigate={() => navigateToDetails(3)}
                />
            </li>
            <li className="list-group-item">
                {deathCount > 0
                    ? <div className="btn-shadow death-count" onClick={navigateToDeathetails}>
                        <FontAwesomeIcon
                            icon={faPlus}
                        />
                        <div>{t("HowDeath")}</div>
                    </div>
                    : <>
                        <div>{t("Death")}</div>
                        <FontAwesomeIcon
                            icon={faBookDead}
                            className="list-group-item__player-statistic-item"
                        />
                    </>
                }
                <div>{deathCount}</div>
            </li>
            <li className="list-group-item">
                <div>{t("AverageItemLevel")}</div>
                <FontAwesomeIcon
                    icon={faUser}
                    className="list-group-item__player-statistic-item"
                />
                <div>{avgilvl}</div>
            </li>
        </ul>
    );
}

export default memo(DetailsItem);