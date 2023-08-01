import { faPen, faStopwatch, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { CartesianGrid, Legend, Line, LineChart, Tooltip, XAxis, YAxis } from 'recharts';
import useTime from '../../hooks/useTime';

const CombatDetailsChart = ({ detailsTypeName, detailsData, setDetailsDataRender, getFilteredCombatDataList }) => {
    const { t, i18n } = useTranslation("combatDetails/combatDetailsChart");

    const [detailsChartData, setDetailsChartData] = useState([]);
    const [useFilter, setUseFilter] = useState(false);
    const [startOfInterval, setStartOfInterval] = useState("");
    const [finishOfInterval, setFinishOfInterval] = useState("");
    const [startDurationOfInterval, setStartDurationOfInterval] = useState("");
    const [finishDurationOfInterval, setFinishDurationOfInterval] = useState("");

    const [getTimeWithoutMs, , getDuration] = useTime();

    useEffect(() => {
        createChartData(detailsData, detailsData[0].time);
    }, [])

    const compare = (a, b) => {
        if (a.time < b.time) {
            return -1;
        }
        if (a.time > b.time) {
            return 1;
        }

        return 0;
    }

    const createChartData = (selectedCombateDetailsData, startTime) => {
        selectedCombateDetailsData.sort(compare);

        const chartData = new Array(selectedCombateDetailsData.length);
        for (let i = 0; i < selectedCombateDetailsData.length; i++) {
            const spellsData = {
                time: getTimeWithoutMs(selectedCombateDetailsData[i].time),
                value: selectedCombateDetailsData[i].value,
                duration: getDuration(getTimeWithoutMs(selectedCombateDetailsData[i].time), getTimeWithoutMs(startTime))
            };

            chartData[i] = spellsData;
        }

        setDetailsChartData(chartData);
    }

    const getSelectedTimespan = (e) => {
        if (e === null) {
            return;
        }

        setUseFilter(true);
        const timeWithoutMs = getTimeWithoutMs(detailsData[0].time);
        const duration = getDuration(e.activePayload[0].payload.time, timeWithoutMs);

        startOfInterval === ""
            ? setStartDurationOfInterval(duration)
            : setFinishDurationOfInterval(duration);

        startOfInterval === ""
            ? setStartOfInterval(e.activePayload[0].payload.time)
            : setFinishOfInterval(e.activePayload[0].payload.time);
    }

    const calculateSpellsByTimespan = () => {
        const spellsByTime = [];
        for (let i = 0; i < detailsData.length; i++) {
            if (detailsData[i].time >= startOfInterval && detailsData[i].time <= finishOfInterval) {
                spellsByTime.push(detailsData[i]);
            }
        }

        const dataForRender = getFilteredCombatDataList(spellsByTime);
        setDetailsDataRender(dataForRender);
        createChartData(spellsByTime, detailsData[0].time);
    }

    const switchToMultplyInterval = () => {
        setStartOfInterval("");
        setFinishOfInterval("");
        setStartDurationOfInterval("");
        setFinishDurationOfInterval("");

        setUseFilter((item) => !item);
    }

    const cancelSelectInterval = () => {
        setUseFilter(false);
        setStartOfInterval("");
        setFinishOfInterval("");
        setStartDurationOfInterval("");
        setFinishDurationOfInterval("");

        const dataForRender = getFilteredCombatDataList(detailsData);
        setDetailsDataRender(dataForRender);
        createChartData(detailsData, detailsData[0].time);
    }

    return (
        <>
            <div>
                {useFilter &&
                    <div>
                        <FontAwesomeIcon
                            icon={faXmark}
                            className="list-group-item__value"
                            onClick={cancelSelectInterval}
                            title={t("Cancel")}
                        />
                        <div>
                            {t("StartOfInterval")}: {startDurationOfInterval}
                        </div>
                        <div>
                            {t("FinishOfInterval")}: {finishDurationOfInterval}
                        </div>
                    </div>
                }
            </div>
            <div>
                <div className="chart-editor-menu">
                    <FontAwesomeIcon
                        icon={faPen}
                        className={useFilter ? "chart-editor active" : "chart-editor"}
                        title={t("SelectInterval")}
                        onClick={switchToMultplyInterval}
                    />
                    {finishOfInterval !== "" &&
                        <FontAwesomeIcon
                            icon={faStopwatch}
                            className="chart-editor"
                            title={t("Calculate")}
                            onClick={calculateSpellsByTimespan}
                        />
                    }
                </div>
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
                    onMouseDown={getSelectedTimespan}
                >
                    <CartesianGrid
                        strokeDasharray="3 3"
                    />
                    <XAxis
                        dataKey="duration"
                    />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    <Line
                        type="monotone"
                        dataKey="value"
                        name={detailsTypeName}
                        stroke="#8884d8"
                        activeDot={{ r: 8 }}
                    />
                </LineChart>
            </div>
        </>
    );
}

export default CombatDetailsChart;