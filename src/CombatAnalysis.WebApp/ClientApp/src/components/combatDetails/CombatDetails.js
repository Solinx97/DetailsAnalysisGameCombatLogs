import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import useCombatDetailsData from '../../hooks/useCombatDetailsData';
import CombatDetailsChart from './CombatDetailsChart';

import "../../styles/combatDetails.scss";

const CombatDetails = ({ combatPlayerId, detailsType, detailsTypeName }) => {
    const { t } = useTranslation("combatDetails/combatDetails");

    const [combatDetailsData, setCombatDetailsData] = useState([]);
    const [detailsDataRender, setDetailsDataRender] = useState(null);
    const [showGeneralDetailsChart, setShowGeneralDetailsChart] = useState(false);
    const [selectedTime, setSelectedTime] = useState("");
    const [startTime, setStartTime] = useState("");
    const [finishTime, setFinishTime] = useState("");
    const [usedSingleFilter, setUsedSingleFilter] = useState(false);
    const [usedMultiplyFilter, setUsedMultiplyFilter] = useState(false);

    const [getCombatDataListAsync, getFilteredCombatDataList, getCombatData] = useCombatDetailsData(combatPlayerId, detailsType);

    useState(() => {
        const getCombatDataComponent = async () => {
            const combatDataComponent = await getCombatDataListAsync();
            setDetailsDataRender(combatDataComponent);

            fillingDetailsListAsync();
        }

        getCombatDataComponent();
    }, [])

    const fillingDetailsListAsync = async () => {
        const combatDetailsData = await getCombatData();
        setCombatDetailsData(combatDetailsData);
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

    return (
        <div className="details__container">
            {detailsDataRender &&
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetailsChart((item) => !item)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowDiagram")}</label>
                </div>
            }
            {showGeneralDetailsChart &&
                <CombatDetailsChart
                    detailsTypeName={detailsTypeName}
                    detailsData={Object.assign([], combatDetailsData)}
                    setDetailsDataRender={setDetailsDataRender}
                    getFilteredCombatDataList={getFilteredCombatDataList}
                />
            }
            {usedSingleFilter &&
                <div className="select-filter">
                    <FontAwesomeIcon
                        icon={faXmark}
                        className="list-group-item__value"
                        onClick={cancelSingleFilter}
                        title={t("Cancel")}
                    />
                    <div>{t("Time")}: {selectedTime}</div>
                </div>
            }
            {(usedMultiplyFilter && finishTime !== "") &&
                <div className="select-filter">
                    <FontAwesomeIcon
                        icon={faXmark}
                        className="list-group-item__value"
                        onClick={cancelSelectInterval}
                        title={t("Cancel")}
                    />
                    <div>{t("StartOfInterval")}: {startTime}, {t("FinishOfInterval")}: {finishTime}</div>
                </div>
            }
            <ul>
                {detailsDataRender}
            </ul>
        </div>
    );
}

export default CombatDetails;