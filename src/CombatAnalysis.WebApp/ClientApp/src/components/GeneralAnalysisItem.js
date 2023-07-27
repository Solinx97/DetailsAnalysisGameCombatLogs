import { faBolt, faBookSkull, faCalendarPlus, faCalendarXmark, faCheck, faCircleNodes, faClock, faClockRotateLeft, faHandFist, faPlusCircle, faShieldHalved, faSignal } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { NavLink } from 'react-router-dom';

const GeneralAnalysisItem = ({ combat, combatLogId }) => {
    const { t, i18n } = useTranslation("generalAnalysis");

    const getCombatStatus = (status) => {
        return status ? t("Win") : t("Defeat");
    }

    return (
        <>
            <div className="card">
                <div className="combat-title">
                    <div className="card-body">
                        <h5 className="card-title">{combat.name}</h5>
                        <p className="card-text">{combat.dungeonName}</p>
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
                <ul className="list-group list-group-flush">
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
                            icon={faSignal}
                            className="list-group-item__status"
                            title={t("CombatStatus")}
                        />
                        <div>{getCombatStatus(combat.isWin)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faCalendarPlus}
                            className="list-group-item__date-icon"
                            title={t("StartDate")}
                        />
                        <div className="list-group-item__date">
                            <div>{format(new Date(combat.startDate), 'MM/dd/yyyy')}</div>
                            <div>{format(new Date(combat.startDate), 'HH:mm')}</div>
                        </div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faCalendarXmark}
                            className="list-group-item__date-icon"
                            title={t("FinishDate")}
                        />
                        <div className="list-group-item__date">
                            <div>{format(new Date(combat.finishDate), 'MM/dd/yyyy')}</div>
                            <div>{format(new Date(combat.finishDate), 'HH:mm')}</div>
                        </div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faClockRotateLeft}
                            className="list-group-item__duration"
                            title={t("Duration")}
                        />
                        <div>{combat.duration}</div>
                    </li>
                </ul>
                <div className="card-body">
                    {combat.isReady
                        ? <NavLink
                            className="card-link"
                            to={`/details-specifical-combat?id=${combat.id}&combatLogId=${combatLogId}`}
                        >{t("MoreDetails")}</NavLink>
                        : <div>{t("NeedWait")}</div>
                    }
                </div>
            </div>
        </>
    );
}

export default GeneralAnalysisItem;