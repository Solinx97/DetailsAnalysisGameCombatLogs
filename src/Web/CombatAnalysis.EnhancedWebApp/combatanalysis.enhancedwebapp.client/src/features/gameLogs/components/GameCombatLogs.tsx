import { faUpload } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import type { RootState } from '../../../app/Store';
import { useLazyAuthenticationQuery } from '../../../features/user/api/User.api';
import Loading from '../../../shared/components/Loading';
import { useGetCombatLogsQuery } from '../api/GameLogs.api';
import GameCombatLogItem from './GameCombatLogItem';

import './GameCombatLogs.scss';

const GameCombatLogs: React.FC = () => {
    const { t } = useTranslation('combatDetails/mainInformation');

    const user = useSelector((state: RootState) => state.user.value);

    const [isAuth, setIsAuth] = useState(false);
    const [selectedLogType, setSelectedLogType] = useState(0);

    const { data: combatLogs, isLoading, refetch } = useGetCombatLogsQuery({ logType: selectedLogType, appUserId: user ? user.id : null });
    const [getAuthAsync] = useLazyAuthenticationQuery();

    useEffect(() => {
        const checkAuth = async () => {
            const auth = await getAuthAsync();
            setIsAuth(auth.status !== "rejected");
        }

        checkAuth();
    }, []);

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="main-information">
            <div className="main-information__title">
                <div>{t("Logs")}</div>
                <div className="log-types">
                    <div className={`log-types__item${selectedLogType === 0 ? '_selected' : ''}`} onClick={() => setSelectedLogType(0)}>{t("Public")}</div>
                    <div className={`log-types__item${selectedLogType === 1 ? '_selected' : ''} ${user === null ? 'not-allowed' : ''}`} onClick={user === null ? () => { } : () => setSelectedLogType(1)}>{t("Personal")}</div>
                </div>
                <FontAwesomeIcon
                    icon={faUpload}
                    onClick={refetch}
                />
            </div>
            <div className="main-information__container">
                <ul className="combats__container">
                    {combatLogs?.map((item) => (
                        <li key={item.id}>
                            <GameCombatLogItem
                                t={t}
                                appUserId={user ? user.id : ""}
                                log={item}
                                isAuth={isAuth}
                            />
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}

export default GameCombatLogs;