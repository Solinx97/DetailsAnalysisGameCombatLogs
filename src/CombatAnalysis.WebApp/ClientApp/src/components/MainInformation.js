import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import { format } from 'date-fns';
import { useTranslation } from 'react-i18next';

import "../styles/mainInformation.scss";

const MainInformation = () => {
    const [combatsRender, setCombatsRender] = useState(null);
    const { t, i18n } = useTranslation("mainInformation");

    useEffect(() => {
        async function fetchBusinesses() {
            await getCombatLogsAsync();
        };

        fetchBusinesses();
    }, []);

    const getCombatLogsAsync = async () => {
        const response = await fetch('/api/v1/MainInformation');
        const status = response.status;
        if (status === 200) {
            const combatLogs = await response.json();

            fillingCombatLogList(combatLogs);
        }
    }

    const fillingCombatLogList = (combats) => {
        if (combats.length <= 0) {
            setCombatsRender(<div>{t("NeedToAddSomething")}</div>);
            return;
        }

        const list = combats.map((element) => combatLogList(element));

        setCombatsRender(
            <ul className="combats__container">
                {list}
            </ul>
        );
    }

    const combatLogList = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">{element.name}</li>
                    <li className="list-group-item">{format(new Date(element.date), 'MM/dd/yyyy HH:mm')}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={`/general-analysis?id=${element.id}`}>{t("Analyzing")}</NavLink>
                </div>
            </div>
        </li>);
    }

    const changeLanguage = (language) => {
        i18n.changeLanguage(language);
    }

    const render = () => {
        return (<div className="main-information__container">
            <button onClick={() => changeLanguage("ru")}>RU</button>
            <button onClick={() => changeLanguage("en")}>EN</button>
            <h2>{t("Logs")}</h2>
            {combatsRender}
        </div>);
    }

    return render();
}

export default MainInformation;