import React, { memo } from 'react';
import { faBolt, faBookOpenReader, faCircleNodes, faHandFist, faPlusCircle, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const CommonPlayerInform = ({ combatPlayer, combatId, combatLogId }) => {
    const { t } = useTranslation("childs/playerInformation");

    const navigate = useNavigate();

    return (
        <ul className="list-group list-group-flush player-information">
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faHandFist}
                    className="list-group-item__damage-done"
                    title={t("Damage")}
                />
                <div>{combatPlayer.damageDone}</div>
                {combatPlayer.damageDone > 0 &&
                    <div className="btn-shadow" onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=DamageDone&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenDamageAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("Damage")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faPlusCircle}
                    className="list-group-item__heal-done"
                    title={t("Healing")}
                />
                <div>{combatPlayer.healDone}</div>
                {combatPlayer.healDone > 0 &&
                    <div className="btn-shadow" onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=HealDone&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenHealingAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("Healing")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faShieldHalved}
                    className="list-group-item__damage-taken"
                    title={t("DamageTaken")}
                />
                <div>{combatPlayer.damageTaken}</div>
                {combatPlayer.damageTaken > 0 &&
                    <div className="btn-shadow" onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=DamageTaken&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenDamageTakenAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("DamageTaken")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faBolt}
                    className="list-group-item__energy-recovery"
                    title={t("ResourcesRecovery")}
                />
                <div>{combatPlayer.energyRecovery}</div>
                {combatPlayer.energyRecovery > 0 &&
                    <div className="btn-shadow" onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=ResourceRecovery&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenResourcesRecoveryAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("Resources")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faCircleNodes}
                    className="list-group-item__used-buffs"
                    title={t("Buffs")}
                />
                <div>{combatPlayer.usedBuffs}</div>
                {combatPlayer.usedBuffs > 0 &&
                    <div className="btn-shadow" onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}`)}
                        title={t("OpenBuffsAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("Buffs")}</div>
                    </div>
                }
            </li>
        </ul>
    );
}

export default memo(CommonPlayerInform);