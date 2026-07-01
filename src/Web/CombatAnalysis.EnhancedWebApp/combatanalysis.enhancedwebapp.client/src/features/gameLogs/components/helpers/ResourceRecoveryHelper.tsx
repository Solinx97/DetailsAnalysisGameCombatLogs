import { useEffect, useState, type JSX } from 'react';
import useTime from '../../../../shared/hooks/useTime';
import {
    useGetResourceRecoveryByFilterQuery,
    useGetResourceRecoveryCountByFilterQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery
} from '../../api/ResourcesRecovery.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const filterTypes = {
    0: "None",
    1: "Target",
    2: "Creator",
    3: "Spell",
    4: "All"
}

interface ResourceRecoveryHelperProps {
    combatPlayerId: number;
    pageSize: number;
    getUserNameWithoutRealm: (username: string) => string;
    t: (key: string) => string;
}

const ResourceRecoveryHelper: React.FC<ResourceRecoveryHelperProps> = ({ combatPlayerId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [totalPages, setTotalPages] = useState(1);
    const [page, setPage] = useState(1);
    const [selectedFilter, setSelectedFilter] = useState({ filter: "None", target: "All", spell: "All" });

    const { data: count, isLoading: countIsLoading } = useGetResourceRecoveryCountByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, creator: selectedFilter.target, spell: selectedFilter.spell }
    );
    const { data, isLoading: dataIsLoading } = useGetResourceRecoveryByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, creator: selectedFilter.target, spell: selectedFilter.spell, page, pageSize }
    );

    useEffect(() => {
        setPage(1);
    }, [selectedFilter]);

    useEffect(() => {
        if (!count || count === 0) {
            return;
        }

        setTotalPages(Math.ceil(count / pageSize));
    }, [count]);

    const tableTitle = (): JSX.Element => {
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

    if (countIsLoading || dataIsLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <div className="player-filter-details">
                <DetailsFilter
                    filters={[ filterTypes[2], filterTypes[3] ]}
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    useGetUniqueFilterValuesQuery={useGetResourceRecoveryUniqueFilterValuesQuery}
                    t={t}
                />
            </div>
            <ul className="player-data-details">
                {tableTitle()}
                {data?.map((item) => (
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
        </>
    );
}

export default ResourceRecoveryHelper;