import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

const HealDoneDetails = () => {
    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [combatId, setCombatId] = useState(0);
    const [healDoneRender, setHealDoneRender] = useState(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getHealDones = async () => {
                await getHealDonesAsync();
            };

            getHealDones();
        }
    }, [combatPlayerId]);

    const getHealDonesAsync = async () => {
        const response = await fetch(`healDone/${combatPlayerId}`);
        const healDones = await response.json();

        await getCombatPlayerAsync();

        fillingHealDoneList(healDones);
    }

    const getCombatPlayerAsync = async () => {
        const response = await fetch(`detailsSpecificalCombat/combatPlayerById/${combatPlayerId}`);
        const combatPlayer = await response.json();

        await getCombatsAsync(combatPlayer.combatId);
    }

    const getCombatsAsync = async (id) => {
        const response = await fetch(`detailsSpecificalCombat/combatById/${id}`);
        const combat = await response.json();

        setCombatId(combat.id);
    }

    const fillingHealDoneList = (healDones) => {
        if (healDones.length > 0) {
            const list = healDones.map((element) => healDoneList(element));

            setHealDoneRender(
                <ul className="damage-done__container">
                    {list}
                </ul>
            );
        }
        else {
            setHealDoneRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const healDoneList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">Всего исцеления: {element.value}</li>
                    <li className="list-group-item">Исцеление в секунду: {element.healPerSecond}</li>
                    <li className="list-group-item">Количество кастов: {element.castNumber}</li>
                    <li className="list-group-item">Среднее значение: {element.averageValue}</li>
                    <li className="list-group-item">Количество критов: {element.critNumber}</li>
                    <li className="list-group-item">Максимальное значение: {element.maxValue}</li>
                    <li className="list-group-item">Минимальное значение: {element.minValue}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={"/target-course?id=" + element.id}>Подробнее</NavLink>
                </div>
            </div>
        </li>;
    }

    const render = () => {
        return <div>
            <h2>Исцеление</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate(`/details-specifical-combat?id=${combatId}`)}>Выбор игрока</button>
            {healDoneRender}
        </div>;
    }

    return render();
}

export default HealDoneDetails;