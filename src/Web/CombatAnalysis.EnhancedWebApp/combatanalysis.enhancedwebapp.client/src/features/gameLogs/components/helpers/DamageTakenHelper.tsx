import { faCopy, faFire, faFlask, faHands, faPooStorm, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState, type JSX } from 'react';
import useTime from '../../../../shared/hooks/useTime';
import {
    useCountDamageTakenQuery,
    useGetAllDamageTakenQuery,
    useGetDamageTakenUniqueFilterValuesQuery
} from '../../api/DamageTaken.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const damageTakenType = {
    Normal: 0,
    Crushing: 1,
    Dodge: 2,
    Parry: 3,
    Miss: 4,
    Resist: 5,
    Immune: 6,
    Absorb: 7
};

interface DamageTakenHelperProps {
    combatPlayerId: number;
    pageSize: number;
    getUserNameWithoutRealm?: (username: string) => string;
    t: (key: string) => string;
}

const DamageTakenHelper: React.FC<DamageTakenHelperProps> = ({ combatPlayerId, pageSize, t }) => {
    const NONE_VALUE = "NONE";
    const ZERO_TIME_VALUE = "00:00:00";

    const { getTimeWithoutMs } = useTime();

    const [totalPages, setTotalPages] = useState(1);
    const [page, setPage] = useState(1);
    const [selectedFilter, setSelectedFilter] = useState({ target: NONE_VALUE, creator: NONE_VALUE, spell: NONE_VALUE, from: ZERO_TIME_VALUE, to: ZERO_TIME_VALUE });

    const { data: count, isLoading: countIsLoading } = useCountDamageTakenQuery(
        { combatPlayerId, target: selectedFilter.target, creator: selectedFilter.creator, spell: selectedFilter.spell, from: selectedFilter.from, to: selectedFilter.to }
    );
    const { data, isLoading: dataIsLoading } = useGetAllDamageTakenQuery(
        { combatPlayerId, target: selectedFilter.target, creator: selectedFilter.creator, spell: selectedFilter.spell, from: selectedFilter.from, to: selectedFilter.to, page, pageSize }
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
            case damageTakenType.Crushing:
                return <FontAwesomeIcon
                    icon={faFire}
                    title={t("Crushing")}
                    className="overvalue"
                />;
            case damageTakenType.Dodge:
                return <FontAwesomeIcon
                    icon={faCopy}
                    title={t("Dodge")}
                    className="overvalue"
                />;
            case damageTakenType.Parry:
                return <FontAwesomeIcon
                    icon={faXmark}
                    title={t("Parry")}
                    className="overvalue"
                />;
            case damageTakenType.Miss:
                return <FontAwesomeIcon
                    icon={faHands}
                    title={t("Miss")}
                    className="overvalue"
                />;
            case damageTakenType.Resist:
                return <FontAwesomeIcon
                    icon={faFlask}
                    title={t("Resist")}
                    className="overvalue"
                />;
            case damageTakenType.Immune:
                return <FontAwesomeIcon
                    icon={faPooStorm}
                    title={t("Immune")}
                    className="overvalue"
                />;
            case damageTakenType.Absorb:
                return <FontAwesomeIcon
                    icon={faPooStorm}
                    title={t("Absorb")}
                    className="overvalue"
                />;
            default:
                return <></>;
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
                    filters={[ "Creator", "Spell" ]}
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    useGetUniqueFilterValuesQuery={useGetDamageTakenUniqueFilterValuesQuery}
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
                                <div className="extra-details">{getIcon(item.damageTakenType)}</div>
                            </li>
                            <li>
                                {getTimeWithoutMs(item.time)}
                            </li>
                            <li>
                                {item.value}
                            </li>
                            <li>
                                {item.creator}
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

export default DamageTakenHelper;