import { faBolt, faBookOpenReader, faKhanda, faPlusCircle, faShieldHalved, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { CombatDetailsModel } from '../../types/dashboard/CombatDetailsModel';

interface DetailsItemProps {
    player: CombatPlayerModel;
    details: CombatDetailsModel;
    getValueShortName(value: number): string;
}

const DetailsItem: React.FC<DetailsItemProps> = ({ player, details, getValueShortName }) => {
    const { t } = useTranslation("childs/playerInformation");

    const navigate = useNavigate();

    const navigateToDetails = (detailsType: number) => {
        navigate(`/combat-details?id=${details.id}&playerId=${player.id}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&number=${details.number}&isWin=${details.isWin}`);
    }

    return (
        <ul className="details__item">
            <li className="list-group-item">
                <div>{t("Damage")}</div>
                <FontAwesomeIcon
                    icon={faKhanda}
                    className="list-group-item__player-statistic-item"
                />
                <div>{getValueShortName(player.damageDone)}</div>
                {player.damageDone > 0 &&
                    <div>
                        {player.score !== null &&
                            <div className="player-score">{player.score.damageScore.toFixed(2)}%</div>
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
                <div>{t("Healing")}</div>
                <FontAwesomeIcon
                    icon={faPlusCircle}
                    className="list-group-item__player-statistic-item"
                />
                <div>{getValueShortName(player.healDone)}</div>
                {player.healDone > 0 &&
                    <div>
                        {player.score !== null &&
                            <div className="player-score">{player.score.healScore.toFixed(2)}%</div>
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
                <div>{getValueShortName(player.damageTaken)}</div>
                {player.damageTaken > 0 &&
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
                <div>{getValueShortName(player.resourcesRecovery)}</div>
                {player.resourcesRecovery > 0 &&
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
                <div>{t("AverageItemLevel")}</div>
                <FontAwesomeIcon
                    icon={faUser}
                    className="list-group-item__player-statistic-item"
                />
                <div>{player.averageItemLevel}</div>
            </li>
        </ul>
    );
}

export default memo(DetailsItem);