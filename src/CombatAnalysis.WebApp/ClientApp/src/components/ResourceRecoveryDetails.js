import React, { useState, useEffect } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

const ResourceRecoveryDetails = () => {
    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [resourceRecoveryRender, setResourceRecoveryRender] = useState(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getResourceRecovery = async () => {
                await getResourceRecoveryAsync();
            };

            getResourceRecovery();
        }
    }, [combatPlayerId]);

    const fillingResourceRecoveryList = (resourceRecoveries) => {
        if (resourceRecoveries.length > 0) {
            const list = resourceRecoveries.map((element) => resourceRecoveryList(element));

            setResourceRecoveryRender(
                <ul className="resource-recovery__container">
                    {list}
                </ul>
            );
        }
        else {
            setResourceRecoveryRender(<div>Необходимо добавить хотя бы 1 элемент</div>);
        }
    }

    const resourceRecoveryList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">Всего восполнено ресурсов: {element.value}</li>
                    <li className="list-group-item">Восполнение ресурсво в секунду: {element.damagePerSecond}</li>
                    <li className="list-group-item">Количество кастов: {element.castNumber}</li>
                    <li className="list-group-item">Среднее значение: {element.averageValue}</li>
                    <li className="list-group-item">Максимальное значение: {element.maxValue}</li>
                    <li className="list-group-item">Минимальное значение: {element.minValue}</li>
                </ul>
                <div className="card-body">
                    <NavLink className="card-link" to={"/target-course?id=" + element.id}>Подробнее</NavLink>
                </div>
            </div>
        </li>;
    }

    const getResourceRecoveryAsync = async () => {
        const response = await fetch('resourceRecovery/' + combatPlayerId);
        const data = await response.json();

        fillingResourceRecoveryList(data);
    }

    const render = () => {
        return <div>
            <h2>Ресурсы игркоа</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate("/details-specifical-combat?id=" + combatPlayerId)}>Назад</button>
            {resourceRecoveryRender}
        </div>
    }

    return render();
}

export default ResourceRecoveryDetails;