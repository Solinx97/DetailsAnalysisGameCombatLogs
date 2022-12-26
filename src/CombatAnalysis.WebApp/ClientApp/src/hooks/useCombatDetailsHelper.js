import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { fa0, faHandFist, faGauge, faStopwatch20, faUserTie, faLocationCrosshairs, faMeteor, faShare, faCircleUp, faCircleDown, faClock, faRightToBracket, faCopy, faHands, faFire, faFlask, faPooStorm } from '@fortawesome/free-solid-svg-icons';
import { useTranslation } from 'react-i18next';

const useCombatDetailsHelper = (combatPlayerId) => {
    const getUserNameWithoutRealm = (userName) => {
        let realmNameIndex = userName.indexOf('-');
        let userNameWithOutRealm = userName.substr(0, realmNameIndex);

        return userNameWithOutRealm;
    }
    const { t, i18n } = useTranslation("useCombatDetailsHelper");

    const getTimeWithoutMs = (time) => {
        let ms = time.indexOf('.');
        let timeWithoutMs = time.substring(0, ms);

        return timeWithoutMs;
    }

    const getGeneralDetailsAsync = async (detailsType) => {
        const response = await fetch(`api/v1/${detailsType}General/${combatPlayerId}`);
        const generalDetailsGeneralData = await response.json();

        return generalDetailsGeneralData;
    }

    const getDetailsAsync = async (detailsType) => {
        const response = await fetch(`api/v1/${detailsType}/${combatPlayerId}`);
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
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("TotalDamage")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title={t("AverageValue")} />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title={t("DamagePerSec")} />
                        <div>{element.damagePerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title={t("CountOfSkills")} />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title={t("CountOfCrits")} />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShare} className="list-group-item__miss-number" title={t("CountOfMiss")} />
                        <div>{element.missNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title={t("MaxValue")} />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title={t("MinValue")} />
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
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("TotalHealing")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title={t("AverageValue")} />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title={t("HealingPerSec")} />
                        <div>{element.healPerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title={t("CountOfSkills")} />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title={t("CountOfCrits")} />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title={t("MaxValue")} />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title={t("MinValue")} />
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
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("TotalDamageTaken")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title={t("AverageValue")} />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title={t("DamageTakenPerSec")} />
                        <div>{element.damageTakenPerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title={t("CountOfSkills")} />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faMeteor} className="list-group-item__crit-number" title={t("CountOfCrits")} />
                        <div>{element.critNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faShare} className="list-group-item__miss-number" title={t("CountOfMiss")} />
                        <div>{element.missNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title={t("MaxValue")} />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title={t("MinValue")} />
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
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("TotalResourcesRecovery")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faGauge} className="list-group-item__average-value" title={t("AverageValue")} />
                        <div>{element.averageValue.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faStopwatch20} className="list-group-item__damage-per-second" title={t("ResourcesRecoveryPerSec")} />
                        <div>{element.resourcePerSecond.toFixed(2)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faLocationCrosshairs} className="list-group-item__cast-number" title={t("CountOfSkills")} />
                        <div>{element.castNumber}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleUp} className="list-group-item__max-value" title={t("MaxValue")} />
                        <div>{element.maxValue}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faCircleDown} className="list-group-item__min-value" title={t("MinValue")} />
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
                            <FontAwesomeIcon icon={faFire} title={t("CritDamage")} />
                        }
                        {element.isDodge &&
                            <FontAwesomeIcon icon={faCopy} title={t("Dodge")} />
                        }
                        {element.isMiss &&
                            <FontAwesomeIcon icon={faHands} title={t("Miss")} />
                        }
                        {element.isParry &&
                            <FontAwesomeIcon icon={fa0} title={t("Parry")} />
                        }
                        {element.isImmune &&
                            <FontAwesomeIcon icon={faPooStorm} title={t("Immune")} />
                        }
                        {element.isResist &&
                            <FontAwesomeIcon icon={faFlask} title={t("Resist")} />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title={t("Time")} />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("Value")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title={t("FromWho")} />
                        <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title={t("ToTarget")} />
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
                            <FontAwesomeIcon icon={faFire} title={t("CritHealing")} />
                        }
                        {element.isFullOverheal &&
                            <FontAwesomeIcon icon={faFlask} title={t("AllToOverHeal")} />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title={t("Time")} />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("Value")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title={t("FromWho")} />
                        <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title={t("ToTarget")} />
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
                            <FontAwesomeIcon icon={faFire} title={t("Crushing")} />
                        }
                        {element.isDodge &&
                            <FontAwesomeIcon icon={faCopy} title={t("Dodge")} />
                        }
                        {element.isMiss &&
                            <FontAwesomeIcon icon={faHands} title={t("Miss")} />
                        }
                        {element.isParry &&
                            <FontAwesomeIcon icon={fa0} title={t("Parry")} />
                        }
                        {element.isImmune &&
                            <FontAwesomeIcon icon={faPooStorm} title={t("Immune")} />
                        }
                        {element.isResist &&
                            <FontAwesomeIcon icon={faFlask} title={t("Resist")} />
                        }
                    </div>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title={t("Time")} />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("Value")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title={t("FromWho")} />
                        <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title={t("ToTarget")} />
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
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faClock} className="list-group-item__value" title={t("Time")} />
                        <div>{getTimeWithoutMs(element.time)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faHandFist} className="list-group-item__value" title={t("Value")} />
                        <div>{element.value}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title={t("FromWho")} />
                        <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faRightToBracket} className="list-group-item__value" title={t("ToTarget")} />
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