import { faCopy, faFire, faFlask, faHands, faPooStorm, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import useTime from '../../hooks/useTime';
import { useGetDamageTakenByPlayerIdQuery } from '../../store/api/combatParser/DamageTaken.api';
import PaginationHelper from './PaginationHelper';

const damageTakenType =
{
    Normal: 0,
    Crushing: 1,
    Dodge: 2,
    Parry: 3,
    Miss: 4,
    Resist: 5,
    Immune: 6,
    Absorb: 7
}

const DamageTakenHelper = ({ combatPlayerId, pageSize, getCountAsync, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [count, setCount] = useState(1);

    const { data, isLoading } = useGetDamageTakenByPlayerIdQuery({ combatPlayerId, page, pageSize });

    const totalPages = Math.ceil(count / pageSize);

    useEffect(() => {
        const getCount = async () => {
            const count = await getCountAsync();
            setCount(count);
        }

        getCount();
    }, []);

    const getIcon = (type) => {
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
                        {t("WhoDamage")}
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
        </div>
    );
}

export default DamageTakenHelper;