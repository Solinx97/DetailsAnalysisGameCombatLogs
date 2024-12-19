import React, { useEffect, useState } from 'react';
import useTime from '../../hooks/useTime';
import {
    useGetResourceRecoveryCountByFilterQuery, useGetResourceRecoveryByFilterQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery
} from '../../store/api/combatParser/ResourcesRecovery.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const ResourceRecoveryHelper = ({ combatPlayerId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [selectedFilter, setSelectedFilter] = useState({ filter: "None", value: -1 });

    const { data: count, isLoading: countIsLoading } = useGetResourceRecoveryCountByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, filterValue: selectedFilter.value }
    );
    const { data, isLoading } = useGetResourceRecoveryByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, filterValue: selectedFilter.value, page, pageSize }
    );

    const totalPages = Math.ceil(count / pageSize);

    useEffect(() => {
        setPage(1);
    }, [selectedFilter]);

    const tableTitle = () => {
        return (
            <li className="player-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Spell")}
                    </li>
                    <li>
                        {t("Time")}
                    </li>
                    <li>
                        {t("Value")}
                    </li>
                    <li>
                        {t("Creator")}
                    </li>
                </ul>
            </li>
        );
    }

    if (isLoading || countIsLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div>
            <div className="player-filter-details">
                <DetailsFilter
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    filter="Creator"
                    filterName={t("Creator")}
                    useGetUniqueFilterValuesQuery={useGetResourceRecoveryUniqueFilterValuesQuery}
                    t={t}
                />
                <DetailsFilter
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    filter="Spell"
                    filterName={t("Spell")}
                    useGetUniqueFilterValuesQuery={useGetResourceRecoveryUniqueFilterValuesQuery}
                    t={t}
                />
            </div>
            <ul className="player-data-details">
                {tableTitle()}
                {data.map((item) => (
                    <li className="player-data-details__item" key={item.id}>
                        <ul>
                            <li>{item.spell}</li>
                            <li>
                                <div>{getTimeWithoutMs(item.time)}</div>
                            </li>
                            <li>{item.value}</li>
                            <li>{getUserNameWithoutRealm(item.creator)}</li>
                        </ul>
                    </li>
                ))}
            </ul>
            <PaginationHelper
                setPage={setPage}
                page={page}
                totalPages={totalPages}
                t={t}
            />
        </div>
    );
}

export default ResourceRecoveryHelper;