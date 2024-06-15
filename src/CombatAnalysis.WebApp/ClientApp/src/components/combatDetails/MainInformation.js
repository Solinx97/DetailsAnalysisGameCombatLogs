import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCombatLogsQuery } from '../../store/api/CombatParserApi';
import { useLazyAuthenticationAsyncQuery } from '../../store/api/UserApi';
import CombatLogItem from './CombatLogItem';
import MainInformationPreview from './MainInformationPreview';

import "../../styles/mainInformation.scss";

const MainInformation = () => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const { data: combatLogs, isLoading } = useGetCombatLogsQuery();
    const [getAuthAsync] = useLazyAuthenticationAsyncQuery();

    const [isAuth, setIsAuth] = useState(false);

    useEffect(() => {
        const checkAuth = async () => {
            const auth = await getAuthAsync();
            setIsAuth(auth.status !== "rejected");
        }

        checkAuth();
    }, [])

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="main-information">
            <h2>{t("Logs")}</h2>
            <div className="main-information__container">
                <ul className="combats__container">
                    {combatLogs?.filter(log => log.isReady).map((item) => (
                        <li key={item.id}>
                            <CombatLogItem
                                log={item}
                                isAuth={isAuth}
                            />
                        </li>
                    ))}
                </ul>
                <MainInformationPreview />
            </div>
        </div>
    );
}

export default MainInformation;