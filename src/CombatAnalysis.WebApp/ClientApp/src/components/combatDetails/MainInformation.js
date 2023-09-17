import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCombatLogsQuery } from '../../store/api/CombatParserApi';
import CombatLogItem from './CombatLogItem';
import { useLazyAuthenticationAsyncQuery } from '../../store/api/UserApi';

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
        return <></>;
    }

    return (
        <div className="main-information__container">
            <h2>{t("Logs")}</h2>
            <ul className="combats__container">
                {combatLogs?.map((item) => (
                    <li key={item.id}>
                        <CombatLogItem
                            log={item}
                            isAuth={isAuth}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MainInformation;