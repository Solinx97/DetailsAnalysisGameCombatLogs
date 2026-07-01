import { faFire, faFlask } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type JSX } from 'react';
import useTime from '../../../../shared/hooks/useTime';
import {
    useGetHealDoneByFilterQuery,
    useGetHealDoneCountByFilterQuery,
    useGetHealDoneUniqueFilterValuesQuery
} from '../../api/HealDone.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const filterTypes = {
    0: "None",
    1: "Target",
    2: "Creator",
    3: "Spell",
    4: "All"
}

interface HealDoneHelperProps {
    combatPlayerId: number;
    pageSize: number;
    getUserNameWithoutRealm: (username: string) => string;
    t: (key: string) => string;
}

const HealDoneHelper: React.FC<HealDoneHelperProps> = ({ combatPlayerId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [totalPages, setTotalPages] = useState(1);
    const [page, setPage] = useState(1);
    const [selectedFilter, setSelectedFilter] = useState({ filter: "None", target: "All", spell: "All" });

    const { data: count, isLoading: countIsLoading } = useGetHealDoneCountByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, target: selectedFilter.target, spell: selectedFilter.spell }
    );
    const { data, isLoading: dataIsLoading } = useGetHealDoneByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, target: selectedFilter.target, spell: selectedFilter.spell, page, pageSize }
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
                        {t("Target")}
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
                    filters={[ filterTypes[1], filterTypes[3] ]}
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    useGetUniqueFilterValuesQuery={useGetHealDoneUniqueFilterValuesQuery}
                    t={t}
                />
            </div>
            <ul className="player-data-details">
                {tableTitle()}
                {data?.map((item) => (
                    <li className="player-data-details__item" key={item.id}>
                        <ul>
                            <li>
                                <div>{item.spell}</div>
                                <div className="extra-details">
                                    {item.isCrit &&
                                        <FontAwesomeIcon
                                            icon={faFire}
                                            title={t("CritHealing")}
                                            className="crit"
                                        />
                                    }
                                    {(item.value === item.overheal) &&
                                        <FontAwesomeIcon
                                            icon={faFlask}
                                            title={t("AllToOverHeal")}
                                            className="overvalue"
                                        />
                                    }
                                </div>
                            </li>
                            <li>
                                {getTimeWithoutMs(item.time)}
                            </li>
                            <li className="extra-details">
                                {(item.value === item.overheal)
                                    ? <div className="value-equal-zero">
                                        <div>0</div>
                                        <div className="overvalue">({item.value})</div>
                                    </div>
                                    : <div className={item.isCrit ? 'crit' : ''}>{item.value}</div>
                                }
                            </li>
                            <li>
                                {getUserNameWithoutRealm(item.target)}
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
        </>
    );
}

export default HealDoneHelper;