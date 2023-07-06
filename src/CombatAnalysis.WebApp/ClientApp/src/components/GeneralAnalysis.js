import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faClock, faPlusCircle, faShieldHalved, faHandFist, faBolt, faCalendarPlus, faCalendarXmark, faClockRotateLeft, faSignal, faBookSkull, faCircleNodes } from '@fortawesome/free-solid-svg-icons';
import { format } from 'date-fns';
import { useTranslation } from 'react-i18next';

import "../styles/generalAnalysis.scss";

const GeneralAnalysis = () => {
    const navigate = useNavigate();
    const { t, i18n } = useTranslation("generalAnalysis");

    const [combatLogId, setCombatLogId] = useState(0);
    const [combatsRender, setCombatsRender] = useState(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatLogId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        const getCombats = async () => {
            await getCombatsAsync();
        };

        getCombats();
    }, [combatLogId]);

    const getCombatsAsync = async () => {
        const response = await fetch(`api/v1/GeneralAnalysis/${combatLogId}`);
        const combats = await response.json();

        fillingCombatList(combats);
    }

    const fillingCombatList = (combats) => {
        if (combats.length <= 0) {
            setCombatsRender(<div>{t("NeedToAddSomething")}</div>);
            return;
        }

        const list = combats.map((element) => combatList(element));

        setCombatsRender(
            <ul className="combats__container">
                {list}
            </ul>
        );
    }

    const getCombatStatus = (status) => {
        return status ? t("Win") : t("Defeat");
    }

    const combatList = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <div className="combat-title">
                    <div className="card-body">
                        <h5 className="card-title">{element.name}</h5>
                        <p className="card-text">{element.dungeonName}</p>
                    </div>
                    {!element.isReady ? (
                        <FontAwesomeIcon icon={faClock} className="list-group-item__not-ready" title={t("NotReady")} />
                    ) : (
                        <FontAwesomeIcon icon={faCheck} className="list-group-item__ready" title={t("Ready")} />
                    )}
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__damage-done" title={t("Damage")} />
                        <div>{element.damageDone}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faPlusCircle} className="list-group-item__heal-done" title={t("Healing")} />
                        <div>{element.healDone}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShieldHalved} className="list-group-item__damage-taken" title={t("DamageTaken")} />
                        <div>{element.damageTaken}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faBolt} className="list-group-item__energy-recovery" title={t("ResourcesRecovery")} />
                        <div>{element.energyRecovery}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faBookSkull} className="list-group-item__death-number" title={t("Deaths")} />
                        <div>{element.deathNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleNodes} title={t("Buffs")} />
                        <div>{element.usedBuffs}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faSignal} className="list-group-item__status" title={t("CombatStatus")} />
                        <div>{getCombatStatus(element.isWin)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCalendarPlus} className="list-group-item__date-icon" title={t("StartDate")} />
                        <div className="list-group-item__date">
                            <div>{format(new Date(element.startDate), 'MM/dd/yyyy')}</div>
                            <div>{format(new Date(element.startDate), 'HH:mm')}</div>
                        </div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCalendarXmark} className="list-group-item__date-icon" title={t("FinishDate")} />
                        <div className="list-group-item__date">
                            <div>{format(new Date(element.finishDate), 'MM/dd/yyyy')}</div>
                            <div>{format(new Date(element.finishDate), 'HH:mm')}</div>
                        </div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClockRotateLeft} className="list-group-item__duration" title={t("Duration")} />
                        <div>{element.duration}</div>
                    </li>
                </ul>
                <div className="card-body">
                    {!element.isReady ? (
                        <div>{t("NeedWait")}</div>
                    ) : (
                        <NavLink className="card-link" to={`/details-specifical-combat?id=${element.id}`}>{t("MoreDetails")}</NavLink>
                    )}
                </div>
            </div>
        </li>);
    }

    const render = () => {
        return (<div className="general-analysis__container">
            <div className="general-analysis__container_navigate">
                <h3>{t("Combats")}</h3>
                <button type="button" className="btn btn-primary" onClick={() => navigate("/")}>{t("MainPage")}</button>
            </div>
            {combatsRender}
        </div>);
    }

    return render();
}

export default GeneralAnalysis;