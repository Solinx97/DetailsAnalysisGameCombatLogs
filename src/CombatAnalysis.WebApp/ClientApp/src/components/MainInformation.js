import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux'
import { updateCombatLogId } from '../features/CombatLogReducer'
import { NavLink } from 'react-router-dom';

import "../styles/generalAnalysis.sass";

const MainInformation = () => {
    const [combatsRender, setCombatsRender] = useState(null);
    const dispatch = useDispatch();

    useEffect(() => {
        const getCombats = async () => {
            await getCombatsAsync();
        };

        getCombats();
    }, []);

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
                    <li className="list-group-item">{element.date}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={"/general-analysis"} onClick={() => dispatch(updateCombatLogId(element.id))}>Разбор</NavLink>
                </div>
            </div>
        </li>;
    }

    const getCombatsAsync = async () => {
        const response = await fetch('mainInformation');
        const data = await response.json();

        fillingCombatLogList(data);
    }

    const render = () => {
        return <div>
            <h2>Логи боев</h2>
            {combatsRender}
        </div>
    }

    return render();
}

export default MainInformation;