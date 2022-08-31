import React, { useState, useEffect } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHandFist, faClock, faUserTie, faRightToBracket, faFire } from '@fortawesome/free-solid-svg-icons';

const DamageDoneDetails = () => {
    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [damageDoneRender, setDamageDoneRender] = useState(null);
    const [damageDoneChartData, setDamageDoneChartData] = useState([]);
    const [damageDones, setDamageDones] = useState(null);
    const [showGeneralDetails, setShowGeneralDetails] = useState(false);
    const [selectedTime, setSelectedTime] = useState("");
    const [usedFilter, setUsedFilter] = useState(false);

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

    const getDuration = (time1, time2) => {
        let timeElementsByTime1 = time1.split(':');
        let hoursByTime1 = +timeElementsByTime1[0];
        let minutesByTime1 = (hoursByTime1 * 60) + +timeElementsByTime1[1];
        let secondsByTime1 = (minutesByTime1 * 60) + +timeElementsByTime1[2];

        let timeElementsByTime2 = time2.split(':');
        let hoursByTime2 = +timeElementsByTime2[0];
        let minutesByTime2 = (hoursByTime2 * 60) + +timeElementsByTime2[1];
        let secondsByTime2 = (minutesByTime2 * 60) + +timeElementsByTime2[2];

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
        let activePayloadTime = e == null ? "" : e.activePayload[0].payload.time;

        if (activePayloadTime != "") {
            setSelectedTime(e.activeLabel);
            setUsedFilter(true);

            for (var i = 0; i < damageDones.length; i++) {
                if (damageDones[i].time.includes(activePayloadTime)) {
                    spellsByTime.push(damageDones[i]);
                }
            }

            fillingDamageDoneList(spellsByTime);
        }
    }

    const cancelSelectedSpellsByTime = () => {
        fillingDamageDoneList(damageDones);
        setSelectedTime("");
        setUsedFilter(false);
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
                    onClick={getSpellsByTime}
                >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="duration" />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    <Line type="monotone" dataKey="value" name="Урон" stroke="#8884d8" activeDot={{ r: 8 }} />
                </LineChart>
            }
            {usedFilter &&
                <div>
                    <div onClick={cancelSelectedSpellsByTime}>Время: {selectedTime}</div>
                </div>
            }
            {damageDoneRender}
        </div>;
    }

    return render();
}

export default DamageDoneDetails;