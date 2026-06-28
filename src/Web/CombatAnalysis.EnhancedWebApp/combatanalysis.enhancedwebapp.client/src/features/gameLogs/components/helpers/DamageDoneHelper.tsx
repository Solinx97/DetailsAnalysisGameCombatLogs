import { faCopy, faFire, faFlask, faHands, faPooStorm, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type JSX } from 'react';
import useTime from '../../../../shared/hooks/useTime';
import {
    useGetDamageDoneByFilterQuery,
    useGetDamageDoneCountByFilterQuery,
    useGetDamageDoneUniqueFilterValuesQuery
} from '../../api/DamageDone.api';
import type { DamageDoneModel } from '../../types/DamageDoneModel';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const damageType = {
    Normal: 0,
    Crit: 1,
    Dodge: 2,
    Parry: 3,
    Miss: 4,
    Resist: 5,
    Immune: 6,
}

interface DamageDoneHelperProps {
    combatPlayerId: number;
    pageSize: number;
    getUserNameWithoutRealm: (username: string) => string;
    t: (key: string) => string;
}

const DamageDoneHelper: React.FC<DamageDoneHelperProps> = ({ combatPlayerId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [totalPages, setTotalPages] = useState(1);
    const [page, setPage] = useState(1);
    const [selectedFilter, setSelectedFilter] = useState({ filter: "None", value: "All" });

    const { data: count, isLoading: countIsLoading } = useGetDamageDoneCountByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, filterValue: selectedFilter.value }
    );
    const { data, isLoading: dataIsLoading } = useGetDamageDoneByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, filterValue: selectedFilter.value, page, pageSize }
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

    const getIcon = (type: number): JSX.Element => {
        switch (type) {
            case damageType.Crit:
                return <FontAwesomeIcon
                    icon={faFire}
                    title={t("CritDamage")}
                    className="crit"
                />;
            case damageType.Dodge:
                return <FontAwesomeIcon
                    icon={faCopy}
                    title={t("Dodge")}
                    className="overvalue"
                />;
            case damageType.Parry:
                return <FontAwesomeIcon
                    icon={faXmark}
                    title={t("Parry")}
                    className="overvalue"
                />;
            case damageType.Miss:
                return <FontAwesomeIcon
                    icon={faHands}
                    title={t("Miss")}
                    className="overvalue"
                />;
            case damageType.Resist:
                return <FontAwesomeIcon
                    icon={faFlask}
                    title={t("Resist")}
                    className="overvalue"
                />;
            case damageType.Immune:
                return <FontAwesomeIcon
                    icon={faPooStorm}
                    title={t("Immune")}
                    className="overvalue"
                />;
            default:
                return <></>;
        }
    }

    const getClassNameByDamageType = (item: DamageDoneModel): string => {
        if (item.damageType === 1) {
            return "crit";
        }
        else if (item.damageType > 1) {
            return "overvalue";
        }
        else {
            return "";
        }
    }

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
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    filter="Target"
                    filterName={t("Target")}
                    useGetUniqueFilterValuesQuery={useGetDamageDoneUniqueFilterValuesQuery}
                    t={t}
                />
                <DetailsFilter
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    filter="Spell"
                    filterName={t("Spell")}
                    useGetUniqueFilterValuesQuery={useGetDamageDoneUniqueFilterValuesQuery}
                    t={t}
                />
            </div>
            <ul className="player-data-details">
                {tableTitle()}
                {data?.map((item: DamageDoneModel) => (
                    <li className="player-data-details__item" key={item.id}>
                        <ul>
                            <li>
                                <div>{item.spell}</div>
                                <div className="extra-details">{getIcon(item.damageType)}</div>
                            </li>
                            <li>{getTimeWithoutMs(item.time)}</li>
                            <li className="extra-details">
                                <div className={getClassNameByDamageType(item)}>{item.value}</div>
                            </li>
                            <li>{getUserNameWithoutRealm(item.target)}</li>
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

export default DamageDoneHelper;