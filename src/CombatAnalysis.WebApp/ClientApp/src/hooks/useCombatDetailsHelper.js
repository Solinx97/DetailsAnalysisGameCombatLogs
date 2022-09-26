import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHandFist, faGauge, faStopwatch20, faLocationCrosshairs, faMeteor, faShare, faCircleUp, faCircleDown, faClock, faRightToBracket, faCopy, faHands, faFire, faFlask } from '@fortawesome/free-solid-svg-icons';

const useCombatDetailsHelper = (combatPlayerId) => {
    const getUserNameWithoutRealm = (userName) => {
        let realmNameIndex = userName.indexOf('-');
        let userNameWithOutRealm = userName.substr(0, realmNameIndex);

        return userNameWithOutRealm;
    }

    const getTimeWithoutMs = (time) => {
        let ms = time.indexOf('.');
        let timeWithoutMs = time.substring(0, ms);

        return timeWithoutMs;
    }

    const getGeneralDetailsAsync = async (detailsType) => {
        const response = await fetch(`${detailsType}General/${combatPlayerId}`);
        const generalDetailsGeneralData = await response.json();

        return generalDetailsGeneralData;
    }

    const getDetailsAsync = async (detailsType) => {
        const response = await fetch(`${detailsType}/${combatPlayerId}`);
        const generalDetailsGeneralData = await response.json();

        return generalDetailsGeneralData;
    }

    const damageDoneGeneralList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего урона" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.damagePerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title="Кол-во критов" />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShare} className="list-group-item__miss-number" title="Кол-во промахов" />
                        <div>{element.missNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const healDoneGeneralList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего исцеления" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.healPerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title="Кол-во критов" />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const damageTakenGeneralList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего урона" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.damageTakenPerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title="Кол-во критов" />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShare} className="list-group-item__miss-number" title="Кол-во промахов" />
                        <div>{element.missNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const resourceRecoveryGeneralList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Всего урона" />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title="Среднее значение" />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title="Урон в секунду" />
                        <div>{element.resourcePerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title="Кол-во кастов" />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title="Макс. значение" />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title="Мин. значение" />
                        <div>{element.minValue}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const damageDoneList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {element.isCrit &&
                            <FontAwesomeIcon icon={faFire} title="Критический урон" />
                        }
                        {element.isDodge &&
                            <FontAwesomeIcon icon={faCopy} title="Уклонение" />
                        }
                        {element.isMiss &&
                            <FontAwesomeIcon icon={faHands} title="Промах" />
                        }
                        {element.isParry &&
                            <FontAwesomeIcon icon={faCompas} title="Паррирование" />
                        }
                        {element.isImmune &&
                            <FontAwesomeIcon icon={faPooStorm} title="Иммунитет к урону" />
                        }
                        {element.isResist &&
                            <FontAwesomeIcon icon={faFlask} title="Сопротивление" />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title="Время" />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Значение" />
                        <div>{element.value}</div>
                    </li>
                    {/*<li className="list-group-item">*/}
                    {/*    <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title="От кого" />*/}
                    {/*    <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>*/}
                    {/*</li>*/}
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title="К цели" />
                        <div>{element.toEnemy}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const healDoneList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {element.isCrit &&
                            <FontAwesomeIcon icon={faFire} title="Критическое исцеление" />
                        }
                        {element.isFullOverheal &&
                            <FontAwesomeIcon icon={faFlask} title="Все исцеление в оверхил" />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title="Время" />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Значение" />
                        <div>{element.value}</div>
                    </li>
                    {/*<li className="list-group-item">*/}
                    {/*    <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title="От кого" />*/}
                    {/*    <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>*/}
                    {/*</li>*/}
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title="К цели" />
                        <div>{element.toPlayer}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const damageTakenList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {element.isCrushing &&
                            <FontAwesomeIcon icon={faFire} title="Сокрушающий удар" />
                        }
                        {element.isDodge &&
                            <FontAwesomeIcon icon={faCopy} title="Уклонение" />
                        }
                        {element.isMiss &&
                            <FontAwesomeIcon icon={faHands} title="Промах" />
                        }
                        {element.isParry &&
                            <FontAwesomeIcon icon={faCompas} title="Паррирование" />
                        }
                        {element.isImmune &&
                            <FontAwesomeIcon icon={faPooStorm} title="Иммунитет к урону" />
                        }
                        {element.isResist &&
                            <FontAwesomeIcon icon={faFlask} title="Сопротивление" />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title="Время" />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Значение" />
                        <div>{element.value}</div>
                    </li>
                    {/*<li className="list-group-item">*/}
                    {/*    <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title="От кого" />*/}
                    {/*    <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>*/}
                    {/*</li>*/}
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title="К цели" />
                        <div>{element.toEnemy}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    const resourceRecoveryList = (element) => {
        return <li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.spellOrItem}</h5>
                    <div className="card-body__extra-damage">
                        {element.isCrit &&
                            <FontAwesomeIcon icon={faFire} title="Критический урон" />
                        }
                        {element.isDodge &&
                            <FontAwesomeIcon icon={faCopy} title="Уклонение" />
                        }
                        {element.isMiss &&
                            <FontAwesomeIcon icon={faHands} title="Промах" />
                        }
                        {element.isParry &&
                            <FontAwesomeIcon icon={faCompas} title="Паррирование" />
                        }
                        {element.isImmune &&
                            <FontAwesomeIcon icon={faPooStorm} title="Иммунитет к урону" />
                        }
                        {element.isResist &&
                            <FontAwesomeIcon icon={faFlask} title="Сопротивление" />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title="Время" />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title="Значение" />
                        <div>{element.value}</div>
                    </li>
                    {/*<li className="list-group-item">*/}
                    {/*    <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title="От кого" />*/}
                    {/*    <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>*/}
                    {/*</li>*/}
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title="К цели" />
                        <div>{element.toEnemy}</div>
                    </li>
                </ul>
            </div>
        </li>;
    }

    return {
        damageDone: {
            generalList: damageDoneGeneralList,
            list: damageDoneList,
        },
        healDone: {
            generalList: healDoneGeneralList,
            list: healDoneList,
        },
        damageTaken: {
            generalList: damageTakenGeneralList,
            list: damageTakenList,
        },
        resourceRecovery: {
            generalList: resourceRecoveryGeneralList,
            list: resourceRecoveryList,
        },
        generalData: getGeneralDetailsAsync,
        data: getDetailsAsync
    };
}

export default useCombatDetailsHelper;