import { faPlus, faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';

const CombatAuraTimes = ({ t, setSelectedCreatorAuras, defaultSelectedCreatorAuras }) => {
    const defaultStartTime = "00:00:00";
    const defaultFinishTime = "00:00:01";

    const [timeApplied, setTimeApplied] = useState(false);
    const [timeSpanUsed, setTimeSpanUsed] = useState(false);
    const [showTime, setShowTime] = useState(false);
    const [startTime, setStartTime] = useState("00:00:00");
    const [finishTime, setFinishTime] = useState("00:00:01");

    useEffect(() => {
        if (!timeApplied) {
            return;
        }

        applyTime();
    }, [timeApplied]);

    const handleStartTimeChange = (e) => {
        setStartTime(e.target.value);
    }

    const handleFinishTimeChange = (e) => {
        setFinishTime(e.target.value);
    }

    const applyTime = () => {
        const auras = [];

        defaultSelectedCreatorAuras.forEach(aura => {
            if (aura.startTime >= startTime && aura.finishTime <= finishTime) {
                auras.push(aura);
            }
        });

        setSelectedCreatorAuras(auras);
        setTimeSpanUsed(true);
        setTimeApplied(false);
    }

    const restoreFiltersToDefault = () => {
        setSelectedCreatorAuras(defaultSelectedCreatorAuras);
        setStartTime(defaultStartTime);
        setFinishTime(defaultFinishTime);
        setTimeSpanUsed(false);
    }

    return (
        <div className="times">
            <div className="times__controll-panel">
                <div className={`btn-shadow ${timeSpanUsed ? 'filter-applied' : ''}`} onClick={() => setShowTime(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faPlus}
                    />
                    <div>{t("Times")}</div>
                </div>
                <div className="times__clear">
                    <FontAwesomeIcon
                        icon={faRotate}
                        onClick={restoreFiltersToDefault}
                    />
                </div>
            </div>
            <div className={`times__aura-times${showTime ? '_show' : ''}`}>
                <div>When:</div>
                <input type="text" value={startTime} placeholder="Start time" onChange={handleStartTimeChange} />
                <input type="text" value={finishTime} placeholder="Finish time" onChange={handleFinishTimeChange} />
                <div className="btn-shadow" onClick={() => setTimeApplied(true)}>
                    <FontAwesomeIcon
                        icon={faPlus}
                    />
                    <div>{t("Apply")}</div>
                </div>
            </div>
        </div>
    );
}

export default CombatAuraTimes;