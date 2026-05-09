import { faArrowDownWideShort, faArrowUpWideShort } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState, type ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import {
    useLazyGetDamageDoneDamageByEachTargetQuery
} from '../../api/DamageDone.api';
import { useLazyGetCombatByIdQuery } from '../../api/GameLogs.api';
import type { CombatModel } from "../../types//CombatModel";
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { CombatTargetModel } from '../../types/CombatTargetModel';
import type { CombatDetailsModel } from '../../types/dashboard/CombatDetailsModel';
import type { CombatPlayerDeathModel } from '../../types/CombatPlayerDeathModel';
import DashboardDeathItem from './DashboardDeathItem';
import DashboardGeneralItem from './DashboardGeneralItem';
import DashboardTargetItem from './DashboardTargetItem';

import './Dashboard.scss';

interface DashboardProps {
    details: CombatDetailsModel;
    combatPlayers: CombatPlayerModel[];
    playersDeath: CombatPlayerDeathModel[];
    getValueShortName(value: number): string;
}

const Dashboard: React.FC<DashboardProps> = ({ details, combatPlayers, playersDeath, getValueShortName }) => {
    const { t } = useTranslation('combatDetails/dashboard');

    const dashboardDetailsType = [t("Damage"), t("Healing"), t("DamageTaken"), t("ResourcesRecovery")];

    const [combat, setCombat] = useState<CombatModel | null>(null);
    const [duration, setDuration] = useState<number>(1);
    const [targets, setTargets] = useState<Array<CombatTargetModel[]>>([]);
    const [showGeneral, setShowGeneral] = useState<boolean>(true);
    const [showTargets, setShowTaregts] = useState<boolean>(false);
    const [showTargetsToAlliances, setShowTargetsToAlliances] = useState<boolean>(true);

    const [getDamageDoneDamageByEachTarget] = useLazyGetDamageDoneDamageByEachTargetQuery();

    const [getCombatById] = useLazyGetCombatByIdQuery();

    useEffect(() => {
        const getCombat = async () => {
            try {
                const combat = await getCombatById(details.id);
                if (combat.data !== undefined) {
                    setCombat(combat.data);
                }
            } catch (error) {
                console.error("Failed to fetch combat:", error);
            }
        }

        if (details.id > 0) {
            getCombat();
        }
    }, [details.id]);

    useEffect(() => {
        if (combat) {
            const duration = getCombatDurationSeconds();
            setDuration(duration);
        }
    }, [combat]);

    useEffect(() => {
        if (combatPlayers.length > 0) {
            const getByTarget = async () => {
                await getTopDamageByTargetAsync();
            }

            getByTarget();
        }
    }, [combatPlayers]);

    const getCombatDurationSeconds = (): number => {
        const duration = combat?.duration.split(':') || [];

        const hours = parseInt(duration[0] || '0');
        const minutes = parseInt(duration[1] || '0');
        const seconds = parseInt(duration[2] || '0');

        const totalSeconds = (hours * 3600) + (minutes * 60) + seconds;

        return totalSeconds;
    }

    const getTopDamageByTargetAsync = async () => {
        const response = await getDamageDoneDamageByEachTarget(details.id);
        if (response.data !== undefined) {
            setTargets(response.data);
        }
    }

    const damageToAlliencesHandle = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        const show: boolean = e?.target.checked || false;
        setShowTargetsToAlliances(show);
    }

    const getTargets = () => {
        if (showTargetsToAlliances) {
            return (<ul className="dashboard__items">
                {targets.map((combatTarget, index) => (
                    <li key={index} className="dashboard__statistics">
                        <DashboardTargetItem
                            combatTarget={combatTarget}
                            details={details}
                            duration={duration}
                            detailsType={index}
                            getValueShortName={getValueShortName}
                        />
                    </li>
                ))}
            </ul>);
        }

        return (<ul className="dashboard__items">
            {targets.filter(x => combatPlayers.filter(u => u.player.username === x[0].target).length === 0).map((combatTarget, index) => (
                <li key={index} className="dashboard__statistics">
                    <DashboardTargetItem
                        combatTarget={combatTarget}
                        details={details}
                        duration={duration}
                        detailsType={index}
                        getValueShortName={getValueShortName}
                    />
                </li>
            ))}
        </ul>);
    }

    const targetsHandle = () => {
        if (!showTargets) {
            return (<></>);
        }
        else if (targets.length === 0) {
            return (<ul className="dashboard__items">
                <li>Loading...</li>
            </ul>);
        }
        else {
            return (<>
                <div className="mb-3 form-check">
                    <input type="checkbox" className="form-check-input" id="exampleCheck1" defaultChecked={showTargetsToAlliances} onChange={damageToAlliencesHandle} />
                    <label className="form-check-label" htmlFor="exampleCheck1">{t("DamageToAlliances")}</label>
                </div>
                {getTargets()}
            </>);
        }
    }

    if (!combat || combatPlayers.length === 0) {
        return (<></>);
    }

    return (
        <div className="dashboard">
            <div className="dashboard__items-title" onClick={() => setShowGeneral(item => !item)}>
                <div>{t("General")}:</div>
                <FontAwesomeIcon
                    icon={showGeneral ? faArrowDownWideShort : faArrowUpWideShort}
                />
            </div>
            {showGeneral &&
                <ul className="dashboard__items">
                    {dashboardDetailsType.map((name, index) => (
                        <li key={index} className="dashboard__statistics">
                            <DashboardGeneralItem
                                name={name}
                                details={details}
                                duration={duration}
                                combatPlayers={combatPlayers}
                                detailsType={index}
                                getValueShortName={getValueShortName}
                            />
                        </li>
                    ))}
                    {playersDeath.length > 0 &&
                        <li key={dashboardDetailsType.length} className="dashboard__statistics">
                            <DashboardDeathItem
                                playersDeath={playersDeath}
                                combatPlayers={combatPlayers}
                            />
                        </li>
                    }
                </ul>
            }
            <div className="dashboard__items-title" onClick={() => setShowTaregts(item => !item)}>
                <div>{t("Targets")}:</div>
                <FontAwesomeIcon
                    icon={showTargets ? faArrowDownWideShort : faArrowUpWideShort}
                />
            </div>
            {targetsHandle()}
        </div>
    );
}

export default memo(Dashboard);