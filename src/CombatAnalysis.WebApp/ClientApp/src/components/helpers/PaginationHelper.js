
const PaginationHelper = ({ t, setPage, page, totalPages }) => {
    const handleClickFirstPage = () => {
        if (page > 1) {
            setPage(1);
        }
    }

    const handleClickNextPage = () => {
        if (page < totalPages) {
            setPage(page + 1);
        }
    }

    const handleClickPreviousPage = () => {
        if (page > 1) {
            setPage(page - 1);
        }
    }

    const handleClickLastPage = () => {
        if (page < totalPages) {
            setPage(totalPages);
        }
    }

    return (
        <div className="pagination-controls">
            <div className="pagination-controls__container">
                <button className="btn-shadow" onClick={handleClickFirstPage} disabled={page === 1}>
                    {t("First")}
                </button>
                <button className="btn-shadow prev" onClick={handleClickPreviousPage} disabled={page === 1}>
                    {t("Previous")}
                </button>
                <span>{page} / {totalPages}</span>
                <button className="btn-shadow next" onClick={handleClickNextPage} disabled={page === totalPages}>
                    {t("Next")}
                </button>
                <button className="btn-shadow last" onClick={handleClickLastPage} disabled={page === totalPages}>
                    {t("Last")}
                </button>
            </div>
        </div>
    );
}

export default PaginationHelper;