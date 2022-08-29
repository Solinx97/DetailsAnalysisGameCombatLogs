import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

const DetailsSpecificalCombat = () => {
    const navigate = useNavigate();

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combatPlayersRender, setCombatPlayersRender] = useState(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatId > 0) {
            const getCombatPlayers = async () => {
                await getCombatPlayersAsync();
            };

            getCombatPlayers();
        }
    }, [combatId]);

    const getCombatPlayersAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/combatPlayersByCombatId/${combatId}`);
        const combatPlayers = await response.json();

        await getCombatsAsync();

        fillingCombatPlayerList(combatPlayers);
    }

    const getCombatsAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/combatById/${combatId}`);
        const combat = await response.json();

        setCombatLogId(combat.combatLogId);
    }

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
                    <li className="list-group-item">
                        <NavLink className="card-link" to={"/damage-done-details?id=" + element.id}>Всего урона: {element.damageDone}</NavLink>
                    </li>
                    <li className="list-group-item">
                        <NavLink className="card-link" to={"/heal-done-details?id=" + element.id}>Всего исцеления: {element.healDone}</NavLink>
                    </li>
                    <li className="list-group-item">
                        <NavLink className="card-link" to={"/damage-taken-details?id=" + element.id}>Всего полученно урона: {element.damageTaken}</NavLink>
                    </li>
                    <li className="list-group-item">
                        <NavLink className="card-link" to={`/heal-done-details?id=${element.id}`}>Всего исцеления: {element.healDone}</NavLink>
                    </li>
                    <li className="list-group-item">
                       <NavLink className="card-link" to={`/damage-taken-details?id=${element.id}`}>Всего полученно урона: {element.damageTaken}</NavLink>
                    </li>
                    <li className="list-group-item">
                        <NavLink className="card-link" to={`/resource-recovery-details?id=${element.id}`}>Всего затрачено энергии: {element.energyRecovery}</NavLink>
                    </li>
                    <li className="list-group-item">Всего бафов: {element.usedBuffs}</li>
                </ul>
            </div>
        </li>;
    }

    const render = () => {
        return <div>
            <h2>Игроки боя</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>Выбор боя</button>
            {combatPlayersRender}
        </div>;
    }

    return render();
}

export default DetailsSpecificalCombat;