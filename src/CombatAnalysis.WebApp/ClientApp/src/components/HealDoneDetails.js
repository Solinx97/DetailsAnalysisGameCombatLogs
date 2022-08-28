import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { NavLink, useNavigate } from 'react-router-dom';

const HealDoneDetails = () => {
    const navigate = useNavigate();

    const combatPlayerId = useSelector((state) => state.combatPlayer.value);
    const [healDoneRender, setHealDoneRender] = useState(null);

    useEffect(() => {
        const getHealDone = async () => {
            await getHealDoneAsync();
        };

        getHealDone();
    }, []);

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
                    <li className="list-group-item">Исцеление в секунду: {element.damagePerSecond}</li>
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

    const getHealDoneAsync = async () => {
        const response = await fetch(`healDone/${combatPlayerId}`);
        const data = await response.json();

        fillingHealDoneList(data);
    }

    const render = () => {
        return <div>
            <h2>Исцеление</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate("/details-specifical-combat")}>Выбор игрока</button>
            {healDoneRender}
        </div>
    }

    return render();
}

export default HealDoneDetails;