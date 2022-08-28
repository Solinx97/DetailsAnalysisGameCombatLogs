import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { updateCombatId } from '../features/CombatReducer';
import { NavLink, useNavigate } from 'react-router-dom';

import "../styles/generalAnalysis.sass";

const GeneralAnalysis = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const combatLogId = useSelector((state) => state.combatLog.value);
    const [combatsRender, setCombatsRender] = useState(null);

    useEffect(() => {
        const getCombats = async () => {
            await getCombatsAsync();
        };

        getCombats();
    }, []);

    const fillingCombatList = (combats) => {
        if (combats.length > 0) {
            const list = combats.map((element) => combatList(element));

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

    const combatList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.name}</h5>
                    <p className="card-text">{element.dungeonName}</p>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">Затрачено энергии: {element.energyRecovery}</li>
                    <li className="list-group-item">Всего урона: {element.damageDone}</li>
                    <li className="list-group-item">Всего исцеления: {element.healDone}</li>
                    <li className="list-group-item">Всего полученно урона: {element.damageTaken}</li>
                    <li className="list-group-item">Всего смертей {element.deathNumber}</li>
                    <li className="list-group-item">Всего бафов: {element.usedBuffs}</li>
                    <li className="list-group-item">Статус боя: {getCombatStatus(element.isWin)}</li>
                    <li className="list-group-item">Дата начало боя: {element.startDate}</li>
                    <li className="list-group-item">Дата окончания боя: {element.finishDate}</li>
                    <li className="list-group-item">Продолжительность боя: {element.duration}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={"/details-specifical-combat"} onClick={() => dispatch(updateCombatId(element.id))}>Подробнее</NavLink>
                </div>
            </div>
        </li>;
    }

    const getCombatsAsync = async () => {
        const response = await fetch(`generalAnalysis/${combatLogId}`);
        const data = await response.json();

        fillingCombatList(data);
    }

    const getCombatStatus = (status) => {
        return status ? "Победа" : "Поражение";
    }

    const render = () => {
        return <div>
            <h2>Бои</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate("/")}>Главная</button>
            {combatsRender}
        </div>
    }

    return render();
}

export default GeneralAnalysis;