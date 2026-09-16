import { faBolt, faBookOpenReader, faKhanda, faPlusCircle, faShieldHalved, faUser, faBookDead } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { UnitInfoModel } from '../../types/UnitInfoModel';
import type { SpecializationScoreModel } from '../../types/SpecializationScoreModel';

interface DetailsItemProps {
    avgilvl: number;
    playerId: number;
    unitInfo: UnitInfoModel;
    details: CombatDetailsModel;
    getValueShortName(value: number): string;
    score?: SpecializationScoreModel;
    deathCount: number;
}

const DetailsItem: React.FC<DetailsItemProps> = ({ avgilvl, playerId, unitInfo, details, getValueShortName, score, deathCount }) => {
    const { t } = useTranslation('childs/playerInformation');

    const navigate = useNavigate();

    const navigateToDetails = (detailsType: number) => {
        navigate(`/combat-details?id=${details.id}&playerId=${playerId}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&number=${details.number}&isWin=${details.isWin}&duration=${details.duration}`);
    }

    return (
        <ul className="details__item">
            <li className="list-group-item">
                <div>{t("DPS")}</div>
                <FontAwesomeIcon
                    icon={faKhanda}
                    className="list-group-item__player-statistic-item"
                />
                <div>{getValueShortName(Math.round(unitInfo.damageDone / details.duration))}</div>
                {unitInfo.damageDone > 0 &&
                    <div>
                        {score !== undefined &&
                            <div className="player-score">{score.damageScore.toFixed(2)}%</div>
                        }
                        <div className="btn-shadow"
                            onClick={() => navigateToDetails(0)}
                            title={t("OpenDamageAnalyzing") || ""}>
                            <FontAwesomeIcon
                                icon={faBookOpenReader}
                            />
                        </div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <div>{t("HPS")}</div>
                <FontAwesomeIcon
                    icon={faPlusCircle}
                    className="list-group-item__player-statistic-item"
                />
                <div>{getValueShortName(Math.round(unitInfo.healDone / details.duration))}</div>
                {unitInfo.healDone > 0 &&
                    <div>
                        {score !== undefined &&
                            <div className="player-score">{score.healScore.toFixed(2)}%</div>
                        }
                        <div className="btn-shadow"
                            onClick={() => navigateToDetails(1)}
                            title={t("OpenHealingAnalyzing") || ""}>
                            <FontAwesomeIcon
                                icon={faBookOpenReader}
                            />
                        </div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <div>{t("DamageTaken")}</div>
                <FontAwesomeIcon
                    icon={faShieldHalved}
                    className="list-group-item__player-statistic-item"
                />
                <div>{getValueShortName(Math.round(unitInfo.damageTaken))}</div>
                {unitInfo.damageTaken > 0 &&
                    <div className="btn-shadow"
                        onClick={() => navigateToDetails(2)}
                        title={t("OpenDamageTakenAnalyzing") || ""}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                    </div>
                }
            </li>
            <li className="list-group-item">
                <div>{t("ResourcesRecovery")}</div>
                <FontAwesomeIcon
                    icon={faBolt}
                    className="list-group-item__player-statistic-item"
                />
                <div>{getValueShortName(Math.round(unitInfo.resourcesRecovery))}</div>
                {unitInfo.resourcesRecovery > 0 &&
                    <div className="btn-shadow"
                        onClick={() => navigateToDetails(3)}
                        title={t("OpenResourcesRecoveryAnalyzing") || ""}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                    </div>
                }
            </li>
            <li className="list-group-item">
                <div>{t("Death")}</div>
                <FontAwesomeIcon
                    icon={faBookDead}
                    className="list-group-item__player-statistic-item"
                />
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