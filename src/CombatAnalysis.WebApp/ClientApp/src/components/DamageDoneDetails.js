import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

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
            const getDamageDones = async () => {
                await getDamageDonesAsync();
            };

            getDamageDones();
        }
    }, [combatPlayerId]);

    const getDamageDonesAsync = async () => {
        const response = await fetch(`damageDone/${combatPlayerId}`);
        const damageDones = await response.json();

        fillingDamageDoneList(damageDones);
    }

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
                    <li className="list-group-item">Время: {element.time}</li>
                    <li className="list-group-item">Урон: {element.value}</li>
                    <li className="list-group-item">От игрока: {element.fromPlayer}</li>
                    <li className="list-group-item">Цель: {element.toEnemy}</li>
                    <li className="list-group-item">Заклинание: {element.spellOrItem}</li>
                </ul>
            </div>
        </li>;
    }

    const render = () => {
        return <div>
            <h2>Подробная информация об уроне</h2>
            <button type="button" className="btn btn-success" onClick={() => navigate(`/damage-done-general-details?id=${combatPlayerId}`)}>Общая информация</button>
            {damageDoneRender}
        </div>;
    }

    return render();
}

export default DamageDoneDetails;