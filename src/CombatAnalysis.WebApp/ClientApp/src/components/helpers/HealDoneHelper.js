import { faFire, faFlask } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import useTime from '../../hooks/useTime';
import { useGetHealDoneTargetByPlayerIdQuery, useGetHealDoneCountTargetsByPlayerIdQuery, useGetHealDoneUniqueTargetsQuery } from '../../store/api/combatParser/HealDone.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const HealDoneHelper = ({ combatPlayerId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [selectedTarget, setSelectedTarget] = useState("All");

    const { data: count, isLoading: countIsLoading } = useGetHealDoneCountTargetsByPlayerIdQuery({ combatPlayerId, target: selectedTarget === "All" ? "-1" : selectedTarget });
    const { data, isLoading } = useGetHealDoneTargetByPlayerIdQuery({ combatPlayerId, target: selectedTarget === "All" ? "-1" : selectedTarget, page, pageSize });

    const totalPages = Math.ceil(count / pageSize);

    useEffect(() => {
        setPage(1);
    }, [selectedTarget]);

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
                        {t("Target")}
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
                useGetUniqueTargetsQuery={useGetHealDoneUniqueTargetsQuery}
                t={t}
            />
            <ul className="player-data-details">
                {tableTitle()}
                {data.map((item) => (
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
        </div>
    );
}

export default HealDoneHelper;