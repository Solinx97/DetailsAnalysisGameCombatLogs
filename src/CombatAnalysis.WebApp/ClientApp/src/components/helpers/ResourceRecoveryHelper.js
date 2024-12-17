import React, { useEffect, useState } from 'react';
import useTime from '../../hooks/useTime';
import { useGetResourceRecoveryCountCreatorByPlayerIdQuery, useGetResourceRecoveryCreatorByPlayerIdQuery, useGetResourceRecoveryUniqueCreatorsQuery } from '../../store/api/combatParser/ResourcesRecovery.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const ResourceRecoveryHelper = ({ combatPlayerId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [selectedTarget, setSelectedTarget] = useState("All");

    const { data: count, isLoading: countIsLoading } = useGetResourceRecoveryCountCreatorByPlayerIdQuery({ combatPlayerId, creator: selectedTarget === "All" ? "-1" : selectedTarget });
    const { data, isLoading } = useGetResourceRecoveryCreatorByPlayerIdQuery({ combatPlayerId, creator: selectedTarget === "All" ? "-1" : selectedTarget, page, pageSize });

    const totalPages = Math.ceil(count / pageSize);

    useEffect(() => {
        setPage(1);
    }, [selectedTarget]);

    const tableTitle = () => {
        return (
            <li className="player-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Creator")}
                    </li>
                    <li>
                        {t("Spell")}
                    </li>
                    <li>
                        {t("Time")}
                    </li>
                    <li>
                        {t("Value")}
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
            <DetailsFilter
                combatPlayerId={combatPlayerId}
                selectedTarget={selectedTarget}
                setSelectedTarget={setSelectedTarget}
                useGetUniqueTargetsQuery={useGetResourceRecoveryUniqueCreatorsQuery}
                t={t}
            />
            <ul className="player-data-details">
                {tableTitle()}
                {data.map((item) => (
                    <li className="player-data-details__item" key={item.id}>
                        <ul>
                            <li>{getUserNameWithoutRealm(item.creator)}</li>
                            <li>{item.spell}</li>
                            <li>
                                <div>{getTimeWithoutMs(item.time)}</div>
                            </li>
                            <li>{item.value}</li>
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