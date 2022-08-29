import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

const DamageTakenDetails = () => {
    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [combatId, setCombatId] = useState(0);
    const [damageTakenRender, setTakenTakenRender] = useState(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getDamageTakens = async () => {
                await getDamageTakensAsync();
            };

            getDamageTakens();
        }
    }, [combatPlayerId]);

    const getDamageTakensAsync = async () => {
        const response = await fetch(`damageTaken/${combatPlayerId}`);
        const damageTakens = await response.json();

        await getCombatPlayerAsync();

        fillingDamageTakenList(damageTakens);
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

    const fillingDamageTakenList = (damageTakens) => {
        if (damageTakens.length > 0) {
            const list = damageTakens.map((element) => damageTakenList(element));

            setTakenTakenRender(
                <ul className="damage-taken__container">
                    {list}
                </ul>
            );
        }
        else {
            setTakenTakenRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const damageTakenList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">Всего получено урона: {element.value}</li>
                    <li className="list-group-item">Получено урона в секунду: {element.damagePerSecond}</li>
                    <li className="list-group-item">Количество кастов: {element.castNumber}</li>
                    <li className="list-group-item">Среднее значение: {element.averageValue}</li>
                    <li className="list-group-item">Количество критов: {element.critNumber}</li>
                    <li className="list-group-item">Количество промахов: {element.missNumber}</li>
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
            <h2>Получено урона</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate(`/details-specifical-combat?id=${combatId}`)}>Выбор игрока</button>
            {damageTakenRender}
        </div>;
    }

    return render();
}

export default DamageTakenDetails;