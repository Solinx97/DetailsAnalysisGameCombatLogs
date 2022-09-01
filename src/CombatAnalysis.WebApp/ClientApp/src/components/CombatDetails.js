import React, { useState, useEffect } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHandFist, faClock, faUserTie, faRightToBracket, faPen, faFire, faFlask, faCopy, faHands, faCompas, faPooStorm } from '@fortawesome/free-solid-svg-icons';

const CombatDetails = ({ detailsTypeName }) => {
    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [detailsType, setDetailsType] = useState("");
    const [damageDoneRender, setDamageDoneRender] = useState(null);
    const [detailsChartData, setDetailsChartData] = useState([]);
    const [detailsData, setDetailsData] = useState(null);
    const [showGeneralDetails, setShowGeneralDetails] = useState(false);
    const [selectedTime, setSelectedTime] = useState("");
    const [startTime, setStartTime] = useState("");
    const [finishTime, setFinishTime] = useState("");
    const [usedSingleFilter, setUsedSingleFilter] = useState(false);
    const [usedMultiplyFilter, setUsedMultiplyFilter] = useState(false);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
        setDetailsType(queryParams.get("detailsType"));
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getDetails = async () => {
                await getDetailsAsync();
            };

            getDetails();
        }
    }, [combatPlayerId]);

    const getDetailsAsync = async () => {
        const response = await fetch(`${detailsType}/${combatPlayerId}`);
        const detailsData = await response.json();

        setDetailsData(detailsData);
        createChartData(detailsData);

        fillingDetailsDataList(detailsData);
    }

    const createChartData = (detailsData) => {
        let chartData = new Array(detailsData.length);

        for (var i = 0; i < detailsData.length; i++) {
            let spellsData = {
                time: getTimeWithoutMs(detailsData[i].time),
                value: detailsData[i].value,
                duration: getDuration(getTimeWithoutMs(detailsData[i].time), getTimeWithoutMs(detailsData[0].time))
            };

            chartData[i] = spellsData;
        }

        setDetailsChartData(chartData);
    }

    const getTimeWithoutMs = (time) => {
        let ms = time.indexOf('.');
        let timeWithoutMs = time.substring(0, ms);

        return timeWithoutMs;
    }

    const getSeconds = (time) => {
        let timeElementsByTime = time.split(':');
        let hoursByTime = +timeElementsByTime[0];
        let minutesByTime = (hoursByTime * 60) + +timeElementsByTime[1];
        let secondsByTime = (minutesByTime * 60) + +timeElementsByTime[2];

        return secondsByTime;
    }

    const getDuration = (time1, time2) => {
        let secondsByTime1 = getSeconds(time1);
        let secondsByTime2 = getSeconds(time2);

        let durationToMinutes = "00";
        let durationToHours = "00";
        let durationToSeconds = secondsByTime1 - secondsByTime2;

        if (durationToSeconds > 60) {
            durationToMinutes = Math.trunc(durationToSeconds / 60);
            durationToSeconds -= durationToMinutes * 60;
        }

        if (durationToMinutes > 60) {
            durationToHours = Math.trunc(durationToMinutes / 60);
            durationToMinutes -= durationToHours * 60;
        }

        let duration = `${durationToHours}:${durationToMinutes}:${durationToSeconds > 9 ? durationToSeconds : `0${durationToSeconds}`}`;

        return duration;
    }

    const fillingDetailsDataList = (detailsData) => {
        if (detailsData.length > 0) {
            let list = <div></div>;

            switch (detailsType) {
                case "DamageDone":
                    list = detailsData.map((element) => damageDoneList(element));
                    break;
                case "HealDone":
                    list = detailsData.map((element) => healDoneList(element));
                    break;
                case "DamageTaken":
                    list = detailsData.map((element) => damageTakenList(element));
                    break;
                case "ResourceRecovery":
                    list = detailsData.map((element) => resourceRecoveryList(element));
                    break;
            }

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

    const getUserNameWithoutRealm = (userName) => {
        let realmNameIndex = userName.indexOf('-');
        let userNameWithOutRealm = userName.substr(0, realmNameIndex);

        return userNameWithOutRealm;
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

    const getSpellsByTime = (e) => {
        let spellsByTime = [];
        let activePayloadTime = e.activePayload[0].payload.time;
        setSelectedTime(e.activeLabel);
        setUsedSingleFilter(true);

        for (var i = 0; i < detailsData.length; i++) {
            if (detailsData[i].time.includes(activePayloadTime)) {
                spellsByTime.push(detailsData[i]);
            }
        }

        fillingDetailsDataList(spellsByTime);
    }

    const getStartTimeInterval = (e) => {
        if (e != null) {
            if (usedMultiplyFilter) {
                setStartTime(e.activeLabel);
            }
            else {
                getSpellsByTime(e);
            }
        }
    }

    const getFinishTimeInterval = (e) => {
        if (usedMultiplyFilter && e != null) {
            let spellsByTime = [];
            let damageDonesByFilter = [];
            setFinishTime(e.activeLabel);

            for (var i = 0; i < detailsData.length; i++) {
                let getDurationAsSeconds = getSeconds(detailsChartData[i].duration);
                let startTimeAsSeconds = getSeconds(startTime);
                let finishTimeAsSeconds = getSeconds(e.activeLabel);

                if (getDurationAsSeconds >= startTimeAsSeconds
                    && getDurationAsSeconds <= finishTimeAsSeconds) {
                    if (detailsData[i].time.includes(detailsChartData[i].time)) {
                        spellsByTime.push(detailsData[i]);
                        damageDonesByFilter.push(detailsData[i]);
                    }
                }
            }

            createChartData(damageDonesByFilter);
            fillingDetailsDataList(spellsByTime);
        }
    }

    const cancelSingleFilter = () => {
        fillingDetailsDataList(detailsData);
        setSelectedTime("");
        setUsedSingleFilter(false);
    }

    const cancelMultiplyFilter = () => {
        fillingDetailsDataList(detailsData);
        createChartData(detailsData);
        setStartTime("");
        setFinishTime("");
        setUsedMultiplyFilter(false);
    }

    const render = () => {
        return <div className="details__container">
            <div>
                <h3>Подробная информация [{detailsTypeName}]</h3>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetails(!showGeneralDetails)} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать диаграмму</label>
            </div>
            {showGeneralDetails &&
                <div>
                    <FontAwesomeIcon icon={faPen} className={usedMultiplyFilter ? "chart-editor active" : "chart-editor"} title="Выделить интервал" onClick={() => setUsedMultiplyFilter(!usedMultiplyFilter)} />
                    <LineChart
                        width={1250}
                        height={300}
                        data={detailsChartData}
                        margin={{
                            top: 5,
                            right: 30,
                            left: 20,
                            bottom: 5,
                        }}
                        onMouseDown={getStartTimeInterval}
                        onMouseUp={getFinishTimeInterval}
                    >
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="duration" />
                        <YAxis />
                        <Tooltip />
                        <Legend />
                        <Line type="monotone" dataKey="value" name="Урон" stroke="#8884d8" activeDot={{ r: 8 }} />
                    </LineChart>
                </div>
            }
            {usedSingleFilter &&
                <div>
                    <div onClick={cancelSingleFilter}>Время: {selectedTime}</div>
                </div>
            }
            {(usedMultiplyFilter && finishTime != "") &&
                <div>
                    <div onClick={cancelMultiplyFilter}>Начало интервала: {startTime}, Конец интервала: {finishTime}</div>
                </div>
            }
            {damageDoneRender}
        </div>;
    }

    return render();
}

export default CombatDetails;