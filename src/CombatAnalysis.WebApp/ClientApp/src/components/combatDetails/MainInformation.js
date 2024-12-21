import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetCombatLogsQuery } from '../../store/api/core/CombatParser.api';
import { useLazyAuthenticationAsyncQuery } from '../../store/api/core/User.api';
import Loading from '../Loading';
import CombatLogItem from './CombatLogItem';

import "../../styles/mainInformation.scss";

const MainInformation = () => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const user = useSelector((state) => state.user.value);

    const { data: combatLogs, isLoading } = useGetCombatLogsQuery();
    const [getAuthAsync] = useLazyAuthenticationAsyncQuery();

    const [isAuth, setIsAuth] = useState(false);
    const [selectedLogType, setSelectedLogType] = useState(0);
    const [publicCombatLogs, setPublicCombatLogs] = useState([]);

    useEffect(() => {
        const checkAuth = async () => {
            const auth = await getAuthAsync();
            setIsAuth(auth.status !== "rejected");
        }

        checkAuth();
    }, []);

    useEffect(() => {
        if (combatLogs === undefined) {
            return;
        }

        let filteredCombatLogs = [];
        switch (selectedLogType) {
            case 0:
                filteredCombatLogs = combatLogs.filter(log => log.isReady && log.logType === 0);
                break;
            case 1:
                filteredCombatLogs = combatLogs.filter(log => log.appUserId === user?.id);
                break;
            default:
                filteredCombatLogs = [];
        }

        setPublicCombatLogs(filteredCombatLogs);
    }, [combatLogs, selectedLogType]);

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="main-information">
            <div className="main-information__title">
                <div>{t("Logs")}</div>
                <div className="log-types">
                    <div className={`log-types__item${selectedLogType === 0 ? '_selected' : ''}`} onClick={() => setSelectedLogType(0)}>{t("Public")}</div>
                    <div className={`log-types__item${selectedLogType === 1 ? '_selected' : ''} ${user === null ? 'not-allowed' : ''}`} onClick={user === null ? null : () => setSelectedLogType(1)}>{t("Personal")}</div>
                </div>
            </div>
            <div className="main-information__container">
                <ul className="combats__container">
                    {publicCombatLogs?.map((item) => (
                        <li key={item.id}>
                            <CombatLogItem
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

export default MainInformation;