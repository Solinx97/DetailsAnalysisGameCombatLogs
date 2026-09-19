import { NoneValue } from '@/shared/helpers/ConstHelpers';
import { useEffect, useState, type JSX } from 'react';
import useTime from '../../../../shared/hooks/useTime';
import {
    useCountResourceRecoveryQuery,
    useGetAllResourceRecoveryQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery
} from '../../api/ResourcesRecovery.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

interface ResourceRecoveryHelperProps {
    unitId: string;
    pageSize: number;
    getUserNameWithoutRealm: (username: string) => string;
    t: (key: string) => string;
}

const ResourceRecoveryHelper: React.FC<ResourceRecoveryHelperProps> = ({ unitId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [totalPages, setTotalPages] = useState(1);
    const [page, setPage] = useState(1);
    const [selectedFilter, setSelectedFilter] = useState({ target: NoneValue.NONE_VALUE.toString(), creator: NoneValue.NONE_VALUE.toString(), spell: NoneValue.NONE_VALUE.toString(), from: NoneValue.ZERO_TIME_VALUE.toString(), to: NoneValue.ZERO_TIME_VALUE.toString() });

    const { data: count, isLoading: countIsLoading } = useCountResourceRecoveryQuery(
        { unitId, target: selectedFilter.target, creator: selectedFilter.creator, spell: selectedFilter.spell, from: selectedFilter.from, to: selectedFilter.to }
    );
    const { data, isLoading: dataIsLoading } = useGetAllResourceRecoveryQuery(
        { unitId, target: selectedFilter.target, creator: selectedFilter.creator, spell: selectedFilter.spell, from: selectedFilter.from, to: selectedFilter.to, page, pageSize }
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
                    filters={["Creator", "Spell"]}
                    unitId={unitId}
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
                            <li>{getUserNameWithoutRealm(item.unit.name)}</li>
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