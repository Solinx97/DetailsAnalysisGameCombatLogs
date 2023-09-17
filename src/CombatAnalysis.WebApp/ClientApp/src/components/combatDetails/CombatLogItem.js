import { faCirclePlus, faMagnifyingGlassChart, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

const CombatLogItem = ({ log, isAuth }) => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const navigate = useNavigate();

    return (
        <div className="card">
            <ul className="list-group list-group-flush">
                <li className="list-group-item title">
                    <div>{log.name}</div>
                    <div className="actions">
                        {!isAuth &&
                            <FontAwesomeIcon
                                icon={faTriangleExclamation}
                                className="authorization"
                                title={t("ShouldAuthorize")}
                            />
                        }
                        <div className={`communication ${isAuth ? "" : "disabled"}`}>
                            <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faCirclePlus}
                                />
                                <div>{t("Chat")}</div>
                            </div>
                            <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faCirclePlus}
                                />
                                <div>{t("Discussion")}</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li className="list-group-item">{format(new Date(log.date), 'MM/dd/yyyy HH:mm')}</li>
            </ul>
            <div className="card-body">
                <div className="btn-shadow" onClick={() => navigate(`/general-analysis?id=${log.id}`)}>
                    <FontAwesomeIcon
                        icon={faMagnifyingGlassChart}
                    />
                    <div>{t("Analyzing")}</div>
                </div>
            </div>
        </div>
    );
}

export default CombatLogItem;