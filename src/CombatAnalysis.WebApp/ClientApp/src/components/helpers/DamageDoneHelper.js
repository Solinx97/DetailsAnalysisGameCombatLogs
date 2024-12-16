import { faCopy, faFire, faFlask, faHands, faPooStorm, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import useTime from '../../hooks/useTime';
import { useGetDamageDoneByPlayerIdQuery } from '../../store/api/combatParser/DamageDone.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

const damageType =
{
    Normal: 0,
    Crit: 1,
    Dodge: 2,
    Parry: 3,
    Miss: 4,
    Resist: 5,
    Immune: 6,
};

const DamageDoneHelper = ({ combatPlayerId, pageSize, getCountAsync, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [count, setCount] = useState(1);
    const [damageDone, setDamageDone] = useState([]);
    const [selectedTarget, setSelectedTarget] = useState("All");

    const { data, isLoading } = useGetDamageDoneByPlayerIdQuery({ combatPlayerId, page, pageSize });

    const totalPages = Math.ceil(count / pageSize);

    useEffect(() => {
        const getCount = async () => {
            const count = await getCountAsync();
            setCount(count);
        }

        getCount();
    }, []);

    useEffect(() => {
        if (data === undefined) {
            return;
        }

        setDamageDone(data);
    }, [data]);

    const getIcon = (type) => {
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

    const getClassNameByDamageType = (item) => {
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
            <DetailsFilter
                combatPlayerId={combatPlayerId}
                selectedTarget={selectedTarget}
                setSelectedTarget={setSelectedTarget}
                setCount={setCount}
                setDamageDone={setDamageDone}
                page={page}
                pageSize={pageSize}
            />
            <ul className="player-data-details">
                {tableTitle()}
                {damageDone?.map((item) => (
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
                            <li>{item.target}</li>
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

export default DamageDoneHelper;