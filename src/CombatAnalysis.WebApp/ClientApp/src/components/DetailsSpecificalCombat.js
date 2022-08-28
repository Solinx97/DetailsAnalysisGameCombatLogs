import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { updateCombatPlayerId } from '../features/CombatPlayerReducer';
import { NavLink, useNavigate } from 'react-router-dom';

const DetailsSpecificalCombat = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const combatId = useSelector((state) => state.combat.value);
    const [combatPlayersRender, setCombatPlayersRender] = useState(null);

    useEffect(() => {
        const getCombatPlayers = async () => {
            await getCombatPlayersAsync();
        };

        getCombatPlayers();
    }, []);

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
                        <NavLink className="card-link" to={"/damage-done-details"} onClick={() => dispatch(updateCombatPlayerId(element.id))}>Всего урона: {element.damageDone}</NavLink>
                    </li>
                    <li className="list-group-item">
                        <NavLink className="card-link" to={"/heal-done-details"} onClick={() => dispatch(updateCombatPlayerId(element.id))}>Всего исцеления: {element.healDone}</NavLink>
                    </li>
                    <li className="list-group-item">
                       <NavLink className="card-link" to={"/damage-taken-details"} onClick={() => dispatch(updateCombatPlayerId(element.id))}>Всего полученно урона: {element.damageTaken}</NavLink>
                    </li>
                    <li className="list-group-item">
                        <NavLink className="card-link" to={"/resource-recovery-details"} onClick={() => dispatch(updateCombatPlayerId(element.id))}>Всего затрачено энергии: {element.energyRecovery}</NavLink>
                    </li>
                    <li className="list-group-item">Всего смертей {element.deathNumber}</li>
                    <li className="list-group-item">Всего бафов: {element.usedBuffs}</li>
                </ul>
            </div>
        </li>;
    }

    const getCombatPlayersAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/${combatId}`);
        const data = await response.json();

        fillingCombatPlayerList(data);
    }

    const render = () => {
        return <div>
            <h2>Игроки боя</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate("/general-analysis")}>Выбор боя</button>
            {combatPlayersRender}
        </div>
    }

    return render();
}

export default DetailsSpecificalCombat;