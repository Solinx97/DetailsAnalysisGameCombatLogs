import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

const DetailsSpecificalCombat = () => {
    const [combatId, setCombatId] = useState(0);
    const [combatPlayersRender, setCombatPlayersRender] = useState(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatId(+queryParams.get("id"));
        console.log(combatId);
    }, []);

    useEffect(() => {
        if (combatId > 0) {
            const getCombatPlayers = async () => {
                await getCombatPlayersAsync();
            };

            getCombatPlayers();
        }
    }, [combatId]);

    const fillingCombatPlayerList = (combatPlayers) => {
        if (combatPlayers.length > 0) {
            const list = combatPlayers.map((element) => combatPlayerList(element));

            setCombatPlayersRender(
                <ul className="combat-players__container">
                    {list}
                </ul>
            );
        }
        else {
            setCombatPlayersRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const combatPlayerList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.userName}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">Затрачено энергии: {element.energyRecovery}</li>
                    <li className="list-group-item">Всего урона: {element.damageDone}</li>
                    <li className="list-group-item">Всего исцеления: {element.healDone}</li>
                    <li className="list-group-item">Всего полученно урона: {element.damageTaken}</li>
                    <li className="list-group-item">Всего смертей {element.deathNumber}</li>
                    <li className="list-group-item">Всего бафов: {element.usedBuffs}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={"/target-course?id=" + element.id}>Подробнее</NavLink>
                </div>
            </div>
        </li>;
    }

    const getCombatPlayersAsync = async () => {
        const response = await fetch('detailsSpecificalCombat/' + combatId);
        const data = await response.json();

        fillingCombatPlayerList(data);
    }

    const render = () => {
        return <div>
            <h2>Игроки боя</h2>
            {combatPlayersRender}
        </div>
    }

    return render();
}

export default DetailsSpecificalCombat;