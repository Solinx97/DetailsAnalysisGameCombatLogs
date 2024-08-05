import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import useCombatDetailsData from '../../hooks/useCombatDetailsData';
import CombatDetailsChart from './CombatDetailsChart';

import "../../styles/combatDetails.scss";

const CombatDetails = ({ combatPlayerId, detailsType, combatStartDate }) => {
    const { t } = useTranslation("combatDetails/combatDetails");

    const [count, setCount] = useState(1);
    const [combatDetailsData, setCombatDetailsData] = useState([]);
    const [detailsDataRender, setDetailsDataRender] = useState(null);
    const [showGeneralDetailsChart, setShowGeneralDetailsChart] = useState(false);
    const [selectedTime, setSelectedTime] = useState("");
    const [startTime, setStartTime] = useState("");
    const [finishTime, setFinishTime] = useState("");
    const [usedSingleFilter, setUsedSingleFilter] = useState(false);
    const [usedMultiplyFilter, setUsedMultiplyFilter] = useState(false);

    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 20;

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const [getCombatDataListAsync, getPlayerDetailsAsync, getCountAsync] = useCombatDetailsData(combatPlayerId, detailsType, combatStartDate);

    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = combatDetailsData.slice(indexOfFirstItem, indexOfLastItem);
    const totalPages = Math.ceil(count / itemsPerPage);

    useState(() => {
        const getCombatDataComponent = async () => {
            const count = await getCountAsync();
            setCount(count);

            await getNewDataAsync(currentPage);

            //fillingDetailsListAsync();
        }

        getCombatDataComponent();
    }, []);

    //const fillingDetailsListAsync = async () => {
    //    const combatDetailsData = await getPlayerDetailsAsync();
    //    setCombatDetailsData(combatDetailsData);
    //}

    //const cancelSingleFilter = () => {
    //    fillingDetailsListAsync();
    //    setSelectedTime("");
    //    setUsedSingleFilter(false);
    //}

    //const cancelSelectInterval = () => {
    //    fillingDetailsListAsync();
    //    setStartTime("");
    //    setFinishTime("");
    //    setUsedMultiplyFilter(false);
    //}

    const getNewDataAsync = async (currentPage) => {
        const combatDataComponent = await getCombatDataListAsync(currentPage, itemsPerPage);
        setDetailsDataRender(combatDataComponent);
    }

    const handleFirstPageAsync = async () => {
        if (currentPage > 1) {
            setCurrentPage(1);

            await getNewDataAsync(1);
        }
    }

    const handleNextPageAsync = async () => {
        if (currentPage < totalPages) {
            setCurrentPage(currentPage + 1);

            await getNewDataAsync(currentPage + 1);
        }
    }

    const handlePreviousPageAsymc = async () => {
        if (currentPage > 1) {
            setCurrentPage(currentPage - 1);

            await getNewDataAsync(currentPage - 1);
        }
    }

    const handleLastPageAsync = async () => {
        if (currentPage < totalPages) {
            setCurrentPage(totalPages);

            await getNewDataAsync(totalPages);
        }
    }

    return (
        <div className="details__container">
            {/*{(detailsDataRender && screenSize.width > maxWidth) &&*/}
            {/*    <div className="form-check form-switch">*/}
            {/*        <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetailsChart((item) => !item)} />*/}
            {/*        <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowDiagram")}</label>*/}
            {/*    </div>*/}
            {/*}*/}
            {/*{showGeneralDetailsChart &&*/}
            {/*    <CombatDetailsChart*/}
            {/*        detailsTypeName={detailsTypeName}*/}
            {/*        detailsData={Object.assign([], combatDetailsData)}*/}
            {/*        setDetailsDataRender={setDetailsDataRender}*/}
            {/*        getFilteredCombatDataList={getCombatDataListAsync}*/}
            {/*    />*/}
            {/*}*/}
            {/*{usedSingleFilter &&*/}
            {/*    <div className="select-filter">*/}
            {/*        <FontAwesomeIcon*/}
            {/*            icon={faXmark}*/}
            {/*            className="list-group-item__value"*/}
            {/*            onClick={cancelSingleFilter}*/}
            {/*            title={t("Cancel")}*/}
            {/*        />*/}
            {/*        <div>{t("Time")}: {selectedTime}</div>*/}
            {/*    </div>*/}
            {/*}*/}
            {/*{(usedMultiplyFilter && finishTime !== "") &&*/}
            {/*    <div className="select-filter">*/}
            {/*        <FontAwesomeIcon*/}
            {/*            icon={faXmark}*/}
            {/*            className="list-group-item__value"*/}
            {/*            onClick={cancelSelectInterval}*/}
            {/*            title={t("Cancel")}*/}
            {/*        />*/}
            {/*        <div>{t("StartOfInterval")}: {startTime}, {t("FinishOfInterval")}: {finishTime}</div>*/}
            {/*    </div>*/}
            {/*}*/}
            <ul>
                {detailsDataRender}
            </ul>
            <div className="pagination-controls">
                <div className="pagination-controls__container">
                    <button className="btn-shadow" onClick={async () => await handleFirstPageAsync()} disabled={currentPage === 1}>
                        {t("First")}
                    </button>
                    <button className="btn-shadow prev" onClick={async () => await handlePreviousPageAsymc()} disabled={currentPage === 1}>
                        {t("Previous")}
                    </button>
                    <span>{currentPage} / {totalPages}</span>
                    <button className="btn-shadow next" onClick={async () => await handleNextPageAsync()} disabled={currentPage === totalPages}>
                        {t("Next")}
                    </button>
                    <button className="btn-shadow last" onClick={async () => await handleLastPageAsync()} disabled={currentPage === totalPages}>
                        {t("Last")}
                    </button>
                </div>
            </div>
        </div>
    );
}

export default CombatDetails;