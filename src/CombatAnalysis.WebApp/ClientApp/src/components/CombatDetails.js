import React, { useState, useEffect } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen } from '@fortawesome/free-solid-svg-icons';
import useCombatDetailsHelper from './hooks/useCombatDetailsHelper';

const CombatDetails = ({ detailsTypeName, userName }) => {
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

    const combatDetailsHelperPayload = useCombatDetailsHelper(combatPlayerId);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
        setDetailsType(queryParams.get("detailsType"));
    }, []);

    useEffect(() => {
        if (combatPlayerId > 0) {
            const getDetails = async () => {
                await fillingDetailsDataList();
            };

            getDetails();
        }
    }, [combatPlayerId]);

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

    const fillingDetailsDataList = async (spellsByTime) => {
        let combatDetailsData = [];
        if (spellsByTime == undefined) {
            combatDetailsData = await combatDetailsHelperPayload.data(detailsType);
        }
        else {
            combatDetailsData = spellsByTime;
        }

        setDetailsData(combatDetailsData);

        if (combatDetailsData.length > 0) {
            let list = <div></div>;

            switch (detailsType) {
                case "DamageDone":
                    list = combatDetailsData.map((element) => combatDetailsHelperPayload.damageDone.list(element));
                    break;
                case "HealDone":
                    list = combatDetailsData.map((element) => combatDetailsHelperPayload.healDone.list(element));
                    break;
                case "DamageTaken":
                    list = combatDetailsData.map((element) => combatDetailsHelperPayload.damageTaken.list(element));
                    break;
                case "ResourceRecovery":
                    list = combatDetailsData.map((element) => combatDetailsHelperPayload.resourceRecovery.list(element));
                    break;
            }

            createChartData(combatDetailsData);

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

    const createChartData = (combatDetailsData) => {
        let chartData = new Array(combatDetailsData.length);

        for (var i = 0; i < combatDetailsData.length; i++) {
            let spellsData = {
                time: getTimeWithoutMs(combatDetailsData[i].time),
                value: combatDetailsData[i].value,
                duration: getDuration(getTimeWithoutMs(combatDetailsData[i].time), getTimeWithoutMs(combatDetailsData[0].time))
            };

            chartData[i] = spellsData;
        }

        setDetailsChartData(chartData);
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
        fillingDetailsDataList();
        setSelectedTime("");
        setUsedSingleFilter(false);
    }

    const cancelMultiplyFilter = () => {
        fillingDetailsDataList();
        setStartTime("");
        setFinishTime("");
        setUsedMultiplyFilter(false);
    }

    const switchSelectInterval = () => {
        setUsedMultiplyFilter(!usedMultiplyFilter);
        if (usedMultiplyFilter) {
            cancelMultiplyFilter();
        }
    }

    const render = () => {
        return <div className="details__container">
            <div>
                <h3>Подробная информация [{detailsTypeName}]</h3>
                <h4>Игрок: {userName}</h4>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetails(!showGeneralDetails)} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">Показать диаграмму</label>
            </div>
            {showGeneralDetails &&
                <div>
                    <FontAwesomeIcon icon={faPen} className={usedMultiplyFilter ? "chart-editor active" : "chart-editor"} title="Выделить интервал" onClick={switchSelectInterval} />
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
                        <Line type="monotone" dataKey="value" name={detailsTypeName} stroke="#8884d8" activeDot={{ r: 8 }} />
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