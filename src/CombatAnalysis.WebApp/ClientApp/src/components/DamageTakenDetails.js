import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { NavLink, useNavigate } from 'react-router-dom';

const DamageTakenDetails = () => {
    const navigate = useNavigate();

    const combatPlayerId = useSelector((state) => state.combatPlayer.value);
    const [damageTakenRender, setTakenTakenRender] = useState(null);

    useEffect(() => {
        const getDamageTaken = async () => {
            await getDamageTakenAsync();
        };

        getDamageTaken();
    }, []);

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

    const getDamageTakenAsync = async () => {
        const response = await fetch(`damageTaken/${combatPlayerId}`);
        const data = await response.json();

        fillingDamageTakenList(data);
    }

    const render = () => {
        return <div>
            <h2>Получено урона</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate("/details-specifical-combat")}>Выбор игрока</button>
            {damageTakenRender}
        </div>
    }

    return render();
}

export default DamageTakenDetails;