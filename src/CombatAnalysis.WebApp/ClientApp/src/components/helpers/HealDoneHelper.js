import { faFire, faFlask } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import useTime from '../../hooks/useTime';
import { useGetHealDoneByPlayerIdQuery } from '../../store/api/CombatParserApi';
import PaginationHelper from './PaginationHelper';

const HealDoneHelper = ({ combatPlayerId, pageSize, getCountAsync, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [count, setCount] = useState(1);

    const { data, isLoading } = useGetHealDoneByPlayerIdQuery({ combatPlayerId, page, pageSize });

    const totalPages = Math.ceil(count / pageSize);

    useEffect(() => {
        const getCount = async () => {
            const count = await getCountAsync();
            setCount(count);
        }

        getCount();
    }, []);

    const getUserNameWithoutRealm = (username) => {
        if (!username.includes('-')) {
            return username;
        }

        const realmNameIndex = username.indexOf('-');
        const userNameWithoutRealm = username.substr(0, realmNameIndex);

        return userNameWithoutRealm;
    }

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