import { faPen, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { CartesianGrid, Legend, Line, LineChart, Tooltip, XAxis, YAxis } from 'recharts';
import useCombatDetailsData from '../hooks/useCombatDetailsData';

import "../styles/combatDetails.scss";

const CombatDetails = ({ detailsTypeName, userName }) => {
    const { t, i18n } = useTranslation("combatDetails");

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [detailsType, setDetailsType] = useState("");
    const [combatDataRender, setCombatDataRender] = useState(<></>);
    const [detailsChartData, setDetailsChartData] = useState([]);
    const [detailsData, setDetailsData] = useState(null);
    const [showGeneralDetails, setShowGeneralDetails] = useState(false);
    const [selectedTime, setSelectedTime] = useState("");
    const [startTime, setStartTime] = useState("");
    const [finishTime, setFinishTime] = useState("");
    const [usedSingleFilter, setUsedSingleFilter] = useState(false);
    const [usedMultiplyFilter, setUsedMultiplyFilter] = useState(false);

    const [combatDataList, getCombatData] = useCombatDetailsData(combatPlayerId, detailsType);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
        setDetailsType(queryParams.get("detailsType"));
    }, []);

    useEffect(() => {
        if (combatPlayerId <= 0) {
            return;
        }

        const getDetails = async () => {
            await fillingDetailsListAsync();
        };

        getDetails();
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

    const fillingDetailsListAsync = async (spellsByTime) => {
        let combatDetailsData = [];
        if (spellsByTime == undefined) {
            combatDetailsData = await getCombatData();
        }
        else {
            combatDetailsData = spellsByTime;
        }

        setDetailsData(combatDetailsData);

        if (combatDetailsData.length === 0) {
            setCombatDataRender(<div>{t("NeedToAddSomething")}</div>);
            return;
        }

        setCombatDataRender(await combatDataList());
        createChartData(combatDetailsData);
    }

    const compare = (a, b) => {
        if (a.time < b.time) {
            return -1;
        }
        if (a.time > b.time) {
            return 1;
        }

        return 0;
    }

    const createChartData = (combatDetailsData) => {
        combatDetailsData.sort(compare);

        let chartData = new Array(combatDetailsData.length);

        for (let i = 0; i < combatDetailsData.length; i++) {
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

        for (let i = 0; i < detailsData.length; i++) {
            if (detailsData[i].time.includes(activePayloadTime)) {
                spellsByTime.push(detailsData[i]);
            }
        }

        fillingDetailsListAsync(spellsByTime);
    }

    const getStartTimeInterval = (e) => {
        if (e === null) {
            return;
        }

        if (usedMultiplyFilter) {
            setStartTime(e.activeLabel);
        }
        else {
            getSpellsByTime(e);
        }
    }

    const getFinishTimeInterval = (e) => {
        if (!usedMultiplyFilter || e === null) {
            return;
        }

        let spellsByTime = [];
        let damageDonesByFilter = [];
        setFinishTime(e.activeLabel);

        for (let i = 0; i < detailsData.length; i++) {
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
        fillingDetailsListAsync(spellsByTime);
    }

    const cancelSingleFilter = () => {
        fillingDetailsListAsync();
        setSelectedTime("");
        setUsedSingleFilter(false);
    }

    const cancelSelectInterval = () => {
        fillingDetailsListAsync();
        setStartTime("");
        setFinishTime("");
        setUsedMultiplyFilter(false);
    }

    const switchToInterval = () => {
        setUsedMultiplyFilter(!usedMultiplyFilter);

        if (usedMultiplyFilter) {
            cancelSelectInterval();
        }
    }

    const render = () => {
        return (<div className="details__container">
            <div>
                <h3>{t("DetailsInform")} [{detailsTypeName}]</h3>
                <h4>{t("Player")}: {userName}</h4>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetails(!showGeneralDetails)} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowDiagram")}</label>
            </div>
            {showGeneralDetails &&
                <div>
                    <FontAwesomeIcon icon={faPen} className={usedMultiplyFilter ? "chart-editor active" : "chart-editor"} title={t("SelectInterval")} onClick={switchToInterval} />
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
                <div className="select-filter">
                    <FontAwesomeIcon icon={faXmark} className="list-group-item__value" onClick={cancelSingleFilter} title={t("Cancel")} />
                    <div>{t("Time")}: {selectedTime}</div>
                </div>
            }
            {(usedMultiplyFilter && finishTime != "") &&
                <div className="select-filter">
                    <FontAwesomeIcon icon={faXmark} className="list-group-item__value" onClick={cancelSelectInterval} title={t("Cancel")} />
                    <div>{t("StartOfInterval")}: {startTime}, {t("FinishOfInterval")}: {finishTime}</div>
                </div>
            }
            {combatDataRender}
        </div>);
    }

    return render();
}

export default CombatDetails;