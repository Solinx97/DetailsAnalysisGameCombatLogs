import React, { useState, useEffect } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHandFist, faClock, faUserTie, faRightToBracket, faPen, faFire, faFlask, faCopy, faHands, faCompas, faPooStorm } from '@fortawesome/free-solid-svg-icons';

const DamageDoneDetails = () => {
    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [damageDoneRender, setDamageDoneRender] = useState(null);
    const [damageDoneChartData, setDamageDoneChartData] = useState([]);
    const [damageDones, setDamageDones] = useState(null);
    const [showGeneralDetails, setShowGeneralDetails] = useState(false);
    const [selectedTime, setSelectedTime] = useState("");
    const [startTime, setStartTime] = useState("");
    const [finishTime, setFinishTime] = useState("");
    const [usedSingleFilter, setUsedSingleFilter] = useState(false);
    const [usedMultiplyFilter, setUsedMultiplyFilter] = useState(false);

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

        setDamageDones(damageDones);
        createChartData(damageDones);

        fillingDamageDoneList(damageDones);
    }

    const createChartData = (damageDones) => {
        let chartData = new Array(damageDones.length);

        for (var i = 0; i < damageDones.length; i++) {
            let spellsData = {
                time: getTimeWithoutMs(damageDones[i].time),
                value: damageDones[i].value,
                duration: getDuration(getTimeWithoutMs(damageDones[i].time), getTimeWithoutMs(damageDones[0].time))
            };

            chartData[i] = spellsData;
        }

        setDamageDoneChartData(chartData);
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
                    <li className="list-group-item">
                        <FontAwesomeIcon icon={faUserTie} className="list-group-item__value" title="От кого" />
                        <div>{getUserNameWithoutRealm(element.fromPlayer)}</div>
                    </li>
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

        for (var i = 0; i < damageDones.length; i++) {
            if (damageDones[i].time.includes(activePayloadTime)) {
                spellsByTime.push(damageDones[i]);
            }
        }

        fillingDamageDoneList(spellsByTime);
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

            for (var i = 0; i < damageDones.length; i++) {
                let getDurationAsSeconds = getSeconds(damageDoneChartData[i].duration);
                let startTimeAsSeconds = getSeconds(startTime);
                let finishTimeAsSeconds = getSeconds(e.activeLabel);

                if (getDurationAsSeconds >= startTimeAsSeconds
                    && getDurationAsSeconds <= finishTimeAsSeconds) {
                    if (damageDones[i].time.includes(damageDoneChartData[i].time)) {
                        spellsByTime.push(damageDones[i]);
                        damageDonesByFilter.push(damageDones[i]);
                    }
                }
            }

            createChartData(damageDonesByFilter);
            fillingDamageDoneList(spellsByTime);
        }
    }

    const cancelSingleFilter = () => {
        fillingDamageDoneList(damageDones);
        setSelectedTime("");
        setUsedSingleFilter(false);
    }

    const cancelMultiplyFilter = () => {
        fillingDamageDoneList(damageDones);
        createChartData(damageDones);
        setStartTime("");
        setFinishTime("");
        setUsedMultiplyFilter(false);
    }

    const render = () => {
        return <div className="damage-done-details__container">
            <div>
                <h3>Подробная информаця об уроне</h3>
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
                        data={damageDoneChartData}
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

export default DamageDoneDetails;