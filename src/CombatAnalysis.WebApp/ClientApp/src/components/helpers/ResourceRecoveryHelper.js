import React, { useEffect, useState } from 'react';
import useTime from '../../hooks/useTime';
import { useGetResourceRecoveryByPlayerIdQuery } from '../../store/api/CombatParserApi';
import PaginationHelper from './PaginationHelper';

const ResourceRecoveryHelper = ({ combatPlayerId, pageSize, getCountAsync, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [count, setCount] = useState(1);

    const { data, isLoading } = useGetResourceRecoveryByPlayerIdQuery({ combatPlayerId, page, pageSize });

    const totalPages = Math.ceil(count / pageSize);

    useEffect(() => {
        const getCount = async () => {
            const count = await getCountAsync();
            setCount(count);
        }

        getCount();
    }, []);

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
                </ul>
            </li>
        );
    }

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div>
            <ul className="player-data-details">
                {tableTitle()}
                {data.map((item) => (
                    <li className="player-data-details__item" key={item.id}>
                        <ul>
                            <li>
                                {item.spell}
                            </li>
                            <li>
                                <div>{getTimeWithoutMs(item.time)}</div>
                            </li>
                            <li>
                                {item.value}
                            </li>
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