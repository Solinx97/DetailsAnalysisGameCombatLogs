import React, { memo } from 'react';
import { faBolt, faBoxOpen, faCircleNodes, faHandFist, faPlusCircle, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const CommonPlayerInform = ({ combatPlayer, combatId, combatLogId }) => {
    const { t } = useTranslation("detailsPlayer");

    const navigate = useNavigate();

    return (
        <ul className="list-group list-group-flush">
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faHandFist}
                    className="list-group-item__damage-done"
                    title={t("Damage")}
                />
                <div>{combatPlayer.damageDone}</div>
                {combatPlayer.damageDone > 0 &&
                    <FontAwesomeIcon
                        icon={faBoxOpen}
                        className="list-group-item__details"
                        onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=DamageDone&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenDamageAnalyzing")}
                    />
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
                    <FontAwesomeIcon
                        icon={faBoxOpen}
                        className="list-group-item__details"
                    onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=HealDone&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenHealingAnalyzing")}
                    />
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
                    <FontAwesomeIcon
                        icon={faBoxOpen}
                        className="list-group-item__details"
                    onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=DamageTaken&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenDamageTakenAnalyzing")}
                    />
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
                    <FontAwesomeIcon
                        icon={faBoxOpen}
                        className="list-group-item__details"
                    onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}&detailsType=ResourceRecovery&combatId=${combatId}&combatLogId=${combatLogId}`)}
                        title={t("OpenResourcesRecoveryAnalyzing")}
                    />
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
                    <FontAwesomeIcon
                        icon={faBoxOpen}
                        className="list-group-item__details"
                        onClick={() => navigate(`/combat-general-details?id=${combatPlayer.id}`)}
                        title={t("OpenBuffsAnalyzing")}
                    />
                }
            </li>
        </ul>
    );
}

export default memo(CommonPlayerInform);