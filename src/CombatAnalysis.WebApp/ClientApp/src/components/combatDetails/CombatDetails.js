import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import useCombatDetailsData from '../../hooks/useCombatDetailsData';

import "../../styles/combatDetails.scss";

const CombatDetails = ({ combatPlayerId, detailsType, combatStartDate }) => {
    const { t } = useTranslation("combatDetails/combatDetails");

    const [count, setCount] = useState(1);
    const [detailsDataRender, setDetailsDataRender] = useState(<></>);

    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 20;

    const { getCombatDataListAsync, getCountAsync } = useCombatDetailsData(combatPlayerId, detailsType, combatStartDate);

    const totalPages = Math.ceil(count / itemsPerPage);

    useState(() => {
        const getCombatDataComponent = async () => {
            const count = await getCountAsync();
            setCount(count);

            await getNewDataAsync(currentPage);
        }

        getCombatDataComponent();
    }, []);

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
            <ul className="player-data-details">
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