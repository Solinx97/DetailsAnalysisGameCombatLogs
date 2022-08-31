import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import { format } from 'date-fns';

import "../styles/mainInformation.scss";

const MainInformation = () => {
    const [combatsRender, setCombatsRender] = useState(null);

    useEffect(() => {
        const getCombatLogs = async () => {
            await getCombatLogsAsync();
        };

        getCombatLogs();
    }, []);

    const getCombatLogsAsync = async () => {
        const response = await fetch('mainInformation');
        const combatLogs = await response.json();

        fillingCombatLogList(combatLogs);
    }

    const fillingCombatLogList = (combats) => {
        if (combats.length > 0) {
            const list = combats.map((element) => combatLogList(element));

            setCombatsRender(
                <ul className="combats__container">
                    {list}
                </ul>
            );
        }
        else {
            setCombatsRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const combatLogList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">{element.name}</li>
                    <li className="list-group-item">{format(new Date(element.date), 'MM/dd/yyyy HH:mm')}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={`/general-analysis?id=${element.id}`}>Разбор</NavLink>
                </div>
            </div>
        </li>;
    }

    const render = () => {
        return <div className="main-information__container">
            <h2>Логи боев</h2>
            {combatsRender}
        </div>;
    }

    return render();
}

export default MainInformation;