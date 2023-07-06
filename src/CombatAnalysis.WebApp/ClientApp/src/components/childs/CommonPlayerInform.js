import React, { memo } from 'react';
import { faBolt, faBoxOpen, faCircleNodes, faHandFist, faPlusCircle, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const CommonPlayerInform = ({ element }) => {
    const { t, i18n } = useTranslation("detailsPlayer");
    const navigate = useNavigate();

    const render = () => {
        return (<ul className="list-group list-group-flush">
            <li className="list-group-item">
                <FontAwesomeIcon icon={faHandFist} className="list-group-item__damage-done" title={t("Damage")} />
                <div>{element.damageDone}</div>
                {element.damageDone > 0 &&
                    <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                        onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=DamageDone`)} title={t("OpenDamageAnalyzing")} />
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon icon={faPlusCircle} className="list-group-item__heal-done" title={t("Healing")} />
                <div>{element.healDone}</div>
                {element.healDone > 0 &&
                    <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                        onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=HealDone`)} title={t("OpenHealingAnalyzing")} />
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon icon={faShieldHalved} className="list-group-item__damage-taken" title={t("DamageTaken")} />
                <div>{element.damageTaken}</div>
                {element.damageTaken > 0 &&
                    <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                        onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=DamageTaken`)} title={t("OpenDamageTakenAnalyzing")} />
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon icon={faBolt} className="list-group-item__energy-recovery" title={t("ResourcesRecovery")} />
                <div>{element.energyRecovery}</div>
                {element.energyRecovery > 0 &&
                    <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                        onClick={() => navigate(`/combat-general-details?id=${element.id}&detailsType=ResourceRecovery`)} title={t("OpenResourcesRecoveryAnalyzing")} />
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon icon={faCircleNodes} className="list-group-item__used-buffs" title={t("Buffs")} />
                <div>{element.usedBuffs}</div>
                {element.usedBuffs > 0 &&
                    <FontAwesomeIcon icon={faBoxOpen} className="list-group-item__details"
                        onClick={() => navigate(`/combat-general-details?id=${element.id}`)} title={t("OpenBuffsAnalyzing")} />
                }
            </li>
        </ul>);
    }

    return render();
}

export default memo(CommonPlayerInform);