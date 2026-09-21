import CombatPlayerBuild from '@/shared/components/combatLogBuild/CombatPlayerBuild';
import Loading from '@/shared/components/Loading';
import { UnitHealthStatus } from '@/shared/helpers/EnumHelper';
import useTime from '@/shared/hooks/useTime';
import { faSkull, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
    useLazyGetCombatPlayerByIdQuery,
    useLazyGetCombatPlayerDeathCountQuery,
    useLazyGetCombatPlayerDeathQuery,
    useLazyGetUnitsHealthByIntervalQuery,
    useLazyGetWhenCombatPlayerDeathQuery,
} from '../../api/GameLogs.api';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { CombatPlayerDeathModel } from '../../types/CombatPlayerDeathModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { UnitHealthModel } from '../../types/UnitHealthModel';
import CombatDetailsHeader from '../details/CombatDetailsHeader';

import './PlayerDiethDetails.scss';

const PlayerDiethDetails: React.FC = () => {
    const { t } = useTranslation('combatDetails/combatGeneralDetails');

    const [details, setDetails] = useState<CombatDetailsModel>({
        id: 0,
        detailsType: 0,
        combatLogId: 0,
        name: '',
        number: 0,
        isWin: false,
        duration: 0,
        gameVersion: -1
    });
    const [playerId, setPlayerId] = useState<number>(0);
    const [combatPlayer, setCombatPlayer] = useState<CombatPlayerModel>();
    const [unitId, setUnitId] = useState<string>("0");
    const [selectedPlayerDeathCount, setSelectedPlayerDeathCount] = useState<number>(0);
    const [playerDeathCount, setPlayerDeathCount] = useState<number>(0);
    const [whenPlayerDeath, setWhenPlayerDeath] = useState<string>("");
    const [playerDeath, setPlayerDeath] = useState<CombatPlayerDeathModel[]>([]);
    const [playerHealth, setPlayerHealth] = useState<UnitHealthModel[]>([]);

    const { getTimeWithoutMs } = useTime();

    const [getCombatPlayerDeathCount] = useLazyGetCombatPlayerDeathCountQuery();
    const [getWhenCombatPlayerDeath] = useLazyGetWhenCombatPlayerDeathQuery();
    const [getCombatPlayerDeath] = useLazyGetCombatPlayerDeathQuery();
    const [getUnitHealthBeforeInterval] = useLazyGetUnitsHealthByIntervalQuery();
    const [getCombatPlayerById] = useLazyGetCombatPlayerByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const detailsType: number = +(queryParams.get("detailsType") ?? 0);
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';
        const duration: number = parseInt(queryParams.get("duration") || '1');
        const gameVersion: number = parseInt(queryParams.get("gameVersion") || '-1');

        const playerId: number = parseInt(queryParams.get("playerId") || '0');
        setPlayerId(playerId);

        const uniId: string = queryParams.get("unitId") || '0';
        setUnitId(uniId);

        setDetails({
            id,
            detailsType,
            combatLogId,
            name,
            number,
            isWin,
            duration,
            gameVersion
        });
    }, []);

    useEffect(() => {
        if (unitId === "0") {
            return;
        }

        const loadData = async () => {
            try {
                const [deathCount] = await Promise.all([
                    getCombatPlayerDeathCount(unitId).unwrap(),
                ]);

                setPlayerDeathCount(deathCount);
            } catch (e) {
                console.error(e);
            }
        }

        loadData();
    }, [unitId]);

    useEffect(() => {
        if (unitId === "0") {
            return;
        }

        const loadData = async () => {
            try {
                const [whenDeath] = await Promise.all([
                    getWhenCombatPlayerDeath({ unitId, skipCount: selectedPlayerDeathCount }).unwrap(),
                ]);

                setWhenPlayerDeath(whenDeath);
            } catch (e) {
                console.error(e);
            }
        }

        loadData();
    }, [unitId, selectedPlayerDeathCount]);

    useEffect(() => {
        if (playerId <= 0) {
            return;
        }

        const loadData = async () => {
            try {
                const [combatPlayer] = await Promise.all([
                    getCombatPlayerById(playerId).unwrap(),
                ]);

                setCombatPlayer(combatPlayer);
            } catch (e) {
                console.error(e);
            }
        }

        loadData();
    }, [playerId]);

    useEffect(() => {
        if (!combatPlayer || whenPlayerDeath === "") {
            return;
        }

        const loadData = async () => {
            try {
                const [death, playerHealth] = await Promise.all([
                    getCombatPlayerDeath({ unitId, whenDied: whenPlayerDeath }).unwrap(),
                    getUnitHealthBeforeInterval({ unitId: combatPlayer.unitId, whenDied: whenPlayerDeath }).unwrap(),
                ]);

                setPlayerDeath(death);
                setPlayerHealth(playerHealth);
            } catch (e) {
                console.error(e);
            }
        }

        loadData();
    }, [combatPlayer, whenPlayerDeath]);

    if (!combatPlayer) {
        return (<Loading />);
    }

    if (!combatPlayer || playerDeath.length === 0 || playerHealth.length === 0) {
        return (
            <div className="general-details__container">
                <div className="general-details__navigate">
                    <CombatDetailsHeader
                        details={details}
                        combatPlayer={combatPlayer}
                        t={t}
                    />
                </div>
                <CombatPlayerBuild />
            </div>
        );
    }

    return (
        <div className="general-details__container">
            <div className="general-details__navigate">
                <CombatDetailsHeader
                    details={details}
                    combatPlayer={combatPlayer}
                    t={t}
                />
            </div>
            <div className="death-count">
                <div>{t("Death")}</div>
                <ul className="death-count__numbers">
                    {[...(Array(playerDeathCount))].map((_, index) => (
                        <li key={index} className={`item ${selectedPlayerDeathCount === index ? 'selected' : ''}`}
                            onClick={() => setSelectedPlayerDeathCount(index)}>{index + 1}</li>
                    ))
                    }
                </ul>
            </div>
            <div className="death-history">
                <ul className="death-history__information">
                    {playerDeath.map((item, index) => (
                        <li key={index} className="history">
                            {index === 0 &&
                                <FontAwesomeIcon
                                    icon={faSkull}
                                    color="red"
                                />
                            }
                            <span>{getTimeWithoutMs(item.time)}</span>
                            <div className="value">
                                <span>{item.spell}</span>
                                <span>{item.value}</span>
                            </div>
                        </li>
                    ))
                    }
                </ul>
                <ul className="death-history__health">
                    {playerHealth.slice(0, playerDeath.length).map((item, index) => (
                        <li key={index} className="history">
                            <FontAwesomeIcon
                                icon={faArrowRight}
                                color={`${item.status === UnitHealthStatus["Increase"] ? 'green' : 'orange'}`}
                            />
                            <div className="health-bar">
                                <div className={`health-bar__value ${item.status === UnitHealthStatus["Increase"] ? 'increase' : 'decrease'}`}>{item.currentHealth} / {item.maxHealth}</div>
                                <div className="health-bar__procentage">
                                    {item.maxHealth > 0
                                        ? `${((item.currentHealth / item.maxHealth) * 100).toFixed(2)}%`
                                        : '0%'}
                                </div>
                            </div>
                        </li>
                    ))
                    }
                </ul>
            </div>
        </div>
    );
}

export default PlayerDiethDetails;