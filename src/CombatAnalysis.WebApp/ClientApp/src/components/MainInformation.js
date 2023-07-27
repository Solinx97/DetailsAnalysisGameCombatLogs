import { format } from 'date-fns';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { NavLink } from 'react-router-dom';
import { useGetCombatLogsQuery } from '../store/api/CombatParserApi';

import "../styles/mainInformation.scss";

const MainInformation = () => {
    const { t, i18n } = useTranslation("mainInformation");

    const { data: combatLogs, isLoading } = useGetCombatLogsQuery();

    if (isLoading) {
        return <h2>{t("Logs")}</h2>;
    }

    return (
        <div className="main-information__container">
            <h2>{t("Logs")}</h2>
            <ul className="combats__container">
                {
                    combatLogs?.map((item) => (
                        <li key={item.id}>
                            <div className="card">
                                <ul className="list-group list-group-flush">
                                    <li className="list-group-item">{item.name}</li>
                                    <li className="list-group-item">{format(new Date(item.date), 'MM/dd/yyyy HH:mm')}</li>
                                </ul>
                                <div className="card-body">
                                    <NavLink
                                        className="card-link"
                                        to={`/general-analysis?id=${item.id}`}
                                    >{t("Analyzing")}</NavLink>
                                </div>
                            </div>
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default MainInformation;