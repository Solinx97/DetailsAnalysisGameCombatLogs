import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

const DamageDoneDetails = () => {
    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [damageDoneRender, setDamageDoneRender] = useState(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getDamageDone = async () => {
                await getDamageDoneAsync();
            };

            getDamageDone();
        }
    }, [combatPlayerId]);

    const fillingDamageDoneList = (damageDones) => {
        if (damageDones.length > 0) {
            const list = damageDones.map((element) => damageDoneList(element));

            setDamageDoneRender(
                <ul className="damage-done__container">
                    {list}
                </ul>
            );
        }
        else {
            setDamageDoneRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const damageDoneList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">Всего урона: {element.value}</li>
                    <li className="list-group-item">Урон в секунду: {element.damagePerSecond}</li>
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

    const getDamageDoneAsync = async () => {
        const response = await fetch('damageDone/' + combatPlayerId);
        const data = await response.json();

        fillingDamageDoneList(data);
    }

    const render = () => {
        return <div>
            <h2>Урон</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate("/details-specifical-combat?id=" + combatPlayerId)}>Назад</button>
            {damageDoneRender}
        </div>
    }

    return render();
}

export default DamageDoneDetails;