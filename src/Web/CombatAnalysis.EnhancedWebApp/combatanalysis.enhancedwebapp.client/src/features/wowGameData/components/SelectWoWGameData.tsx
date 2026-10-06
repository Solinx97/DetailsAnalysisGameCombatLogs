import PersonalTabs from '@/features/gameLogs/components/PersonalTabs';
import { faClose, faPlus, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useBattleNetDataAuthorizaitonMutation, useIsAuthorizedQuery, useLazyBattleNetDisconenctQuery } from '../api/BattleNetData.api';
import AccountDashboard from './AccountDashboard';
import WoWGameDataDetails from './WoWGameDataDetails';

import './WoWGameDataDetails.scss';

const SelectWoWGameData: React.FC = () => {
    const regionName = "eu";

    const { t } = useTranslation('wowGameData');

    const { data: isAuthorized, refetch } = useIsAuthorizedQuery();

    const [getAuthorization] = useBattleNetDataAuthorizaitonMutation();
    const [disconnect] = useLazyBattleNetDisconenctQuery();
    
    const getAuthorizationTokenAsync = async () => {
        try {
            const authUri = await getAuthorization().unwrap();
            window.location.href = authUri.uri;
        } catch (error) {
            console.error("Failed authorization to battle net account:", error);
        }
    }

    const battleNetDisconnect = async () => {
        try {
            await disconnect().unwrap();
            await refetch().unwrap();
        } catch (error) {
            console.error("Failed authorization to battle net account:", error);
        }
    }

    return (
        <div>
            <div className="battle-net">
                <div className="status">{isAuthorized && isAuthorized.authenticated ? 'connected' : 'not connected'}</div>
                <div className="actions">
                    <div className={`auth btn-shadow ${isAuthorized && isAuthorized.authenticated ? 'connected' : 'not-connected'}`}
                        onClick={isAuthorized && isAuthorized.authenticated ? () => { } : getAuthorizationTokenAsync}>
                        <FontAwesomeIcon
                            icon={isAuthorized && isAuthorized.authenticated ? faUser : faPlus}
                        />
                        <div>{t("BattleNet")}</div>
                    </div>
                    {(isAuthorized && isAuthorized.authenticated) &&
                        <div className="exit" onClick={battleNetDisconnect}>
                            <FontAwesomeIcon
                                icon={faClose}
                            />
                        </div>
                    }
                </div>
            </div>
            <PersonalTabs
                tab={0}
                tabs={[
                    {
                        id: 0,
                        header: t("Details"),
                        content: <WoWGameDataDetails
                            regionName={regionName}
                            getAuthorizationTokenAsync={getAuthorizationTokenAsync}
                            isAuthorized={isAuthorized && isAuthorized.authenticated ? true : false}
                            t={t}
                        />
                    },
                    {
                        id: 1,
                        header: t("Dashboard"),
                        content: <AccountDashboard
                            regionName={regionName}
                            getAuthorizationTokenAsync={getAuthorizationTokenAsync}
                            isAuthorized={isAuthorized && isAuthorized.authenticated ? true : false}
                            t={t}
                        />
                    }
                ]}
                tabsClassName={"charts"}
            />
        </div>
    );
}

export default SelectWoWGameData;