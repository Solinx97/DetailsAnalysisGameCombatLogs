import Loading from '@/shared/components/Loading';
import { UnitHealthStatus } from '@/shared/helpers/EnumHelper';
import useTime from '@/shared/hooks/useTime';
import { faSkull } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
    useLazyGetCombatPlayerByIdQuery,
    useLazyGetCombatPlayerDeathCountQuery,
    useLazyGetCombatPlayerDeathQuery,
} from '../../api/GameLogs.api';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { CombatPlayerDeathModel } from '../../types/CombatPlayerDeathModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
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
    const [combatPlayer, setCombatPlayer] = useState<CombatPlayerModel | null>(null);
    const [unitId, setUnitId] = useState<string>("0");
    const [selectedPlayerDeathCount, setSelectedPlayerDeathCount] = useState<number>(0);
    const [playerDeathCount, setPlayerDeathCount] = useState<number>(0);
    const [playerDeath, setPlayerDeath] = useState<CombatPlayerDeathModel[]>([]);

    const { getTimeWithoutMs } = useTime();

    const [getCombatPlayerDeathCount] = useLazyGetCombatPlayerDeathCountQuery();
    const [getCombatPlayerDeath] = useLazyGetCombatPlayerDeathQuery();
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
                const [death] = await Promise.all([
                    getCombatPlayerDeath({ unitId, skipCount: selectedPlayerDeathCount }).unwrap(),
                ]);

                setPlayerDeath(death);
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

    if (details.id <= 0 || !combatPlayer) {
        return (<Loading />);
    }

    console.log(playerDeathCount);
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

            <ul className="death-history">
                {playerDeath.map((health, index) => (
                    <li key={index} className="death-history__history">
                        {index === 0 &&
                            <FontAwesomeIcon
                                icon={faSkull}
                                color="red"
                            />
                        }
                        <span>{getTimeWithoutMs(health.time)}</span>
                        <div>{health.status === UnitHealthStatus["Increase"] ? '+' : '-'}</div>
                        <div className={`value ${health.status === UnitHealthStatus["Increase"] ? 'increase' : 'decrease'}`}>
                            <span>{health.spell}</span>
                            <span>{health.value}</span>
                        </div>
                        <div className="health-bar">{health.currentHealth}/{health.maxHealth}</div>
                    </li>
                ))
                }
            </ul>
        </div>

    );
}

export default PlayerDiethDetails;