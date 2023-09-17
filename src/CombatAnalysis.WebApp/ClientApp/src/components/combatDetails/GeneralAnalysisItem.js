import { faBolt, faBookSkull, faCheck, faCircleNodes, faCirclePlay, faClock, faFlagCheckered, faGraduationCap, faHandFist, faHourglassStart, faDatabase, faPlusCircle, faShieldHalved, faSkull } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

const GeneralAnalysisItem = ({ combat, combatLogId }) => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const navigate = useNavigate();

    return (
        <div className="card">
            <div className="combat-title">
                <div className="status">
                    <div className="card-body">
                        <h5 className="card-title">{combat.name}</h5>
                        <p className="card-text">{combat.dungeonName}</p>
                    </div>
                    {combat.isWin
                        ? <FontAwesomeIcon
                            icon={faGraduationCap}
                            className="win"
                            title={t("Win")}
                        />
                        : <FontAwesomeIcon
                            icon={faSkull}
                            className="lose"
                            title={t("Lose")}
                        />
                    }
                </div>
                {combat.isReady
                    ? <FontAwesomeIcon
                        icon={faCheck}
                        className="list-group-item__ready"
                        title={t("Ready")}
                    />
                    : <FontAwesomeIcon
                        icon={faClock}
                        className="list-group-item__not-ready"
                        title={t("NotReady")}
                    />
                }
            </div>
            <ul className="list-group list-group-flush information">
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faHandFist}
                        className="list-group-item__damage-done"
                        title={t("Damage")}
                    />
                    <div>{combat.damageDone}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faPlusCircle}
                        className="list-group-item__heal-done"
                        title={t("Healing")}
                    />
                    <div>{combat.healDone}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faShieldHalved}
                        className="list-group-item__damage-taken"
                        title={t("DamageTaken")}
                    />
                    <div>{combat.damageTaken}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faBolt}
                        className="list-group-item__energy-recovery"
                        title={t("ResourcesRecovery")}
                    />
                    <div>{combat.energyRecovery}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faBookSkull}
                        className="list-group-item__death-number"
                        title={t("Deaths")}
                    />
                    <div>{combat.deathNumber}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faCircleNodes}
                        title={t("Buffs")}
                    />
                    <div>{combat.usedBuffs}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faCirclePlay}
                        title={t("StartDate")}
                    />
                    <div className="list-group-item__date">
                        <div>{format(new Date(combat.startDate), 'HH:mm')}</div>
                    </div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faFlagCheckered}
                        title={t("FinishDate")}
                    />
                    <div className="list-group-item__date">
                        <div>{format(new Date(combat.finishDate), 'HH:mm')}</div>
                    </div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faHourglassStart}
                        className="list-group-item__duration"
                        title={t("Duration")}
                    />
                    <div>{combat.duration}</div>
                </li>
            </ul>
            <div className="card-body details">
                {combat.isReady
                    ? <div className="btn-shadow" onClick={() => navigate(`/details-specifical-combat?id=${combat.id}&combatLogId=${combatLogId}`)}>
                        <FontAwesomeIcon
                            icon={faDatabase}
                        />
                        <div>{t("MoreDetails")}</div>
                    </div>
                    : <div>{t("NeedWait")}</div>
                }
            </div>
        </div>
    );
}

export default GeneralAnalysisItem;