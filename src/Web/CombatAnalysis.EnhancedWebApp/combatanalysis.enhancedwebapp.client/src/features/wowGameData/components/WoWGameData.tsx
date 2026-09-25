import WoWGameDataContext from '@/context/WoWGameDataContext';
import { NoneValue } from '@/shared/helpers/ConstHelpers';
import { faClose, faLocationCrosshairs, faPlus, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Select from 'react-select';
import { useBattleNetDataAuthorizaitonMutation, useBattleNetDataTokenMutation, useIsAuthorizedQuery, useLazyBattleNetDisconenctQuery } from '../api/BattleNetData.api';
import { useLazyGetRealmsQuery } from '../api/WoWData.api';
import type { RealmModel } from '../types/RealmModel';
import AchievementsCategory from './AchievementsCategory';
import CharacterMounts from './CharacterMounts';
import CharacterMythicKeystone from './CharacterMythicKeystone';
import CharacterRaids from './CharacterRaids';
import CharacterReputations from './CharacterReputations';
import CharacterSummary from './CharacterSummary';

import './WoWGameData.scss';

type Option = {
    value: string;
    label: string;
}

const WoWGameData: React.FC = () => {
    const regionName = "eu";

    const { t } = useTranslation('wowGameData');

    const usernameRef = useRef<HTMLInputElement | null>(null);

    const [servers, setServers] = useState<RealmModel[]>([]);

    const [username, setUsername] = useState<string | undefined>();
    const [showSummary, setShowSummary] = useState<boolean>(false);
    const [showMythicKeystone, setShowMythicKeystone] = useState<boolean>(false);
    const [showRaids, setShowRaids] = useState<boolean>(false);
    const [showDungeons, setShowDungeons] = useState<boolean>(false);
    const [showAchievements, setShowAchievements] = useState<boolean>(false);
    const [showReputations, setShowReputations] = useState<boolean>(false);
    const [showMounts, setShowMounts] = useState<boolean>(false);

    const [serversOptions, setServersOptions] = useState<Option[]>([]);
    const [serverValue, setServerValue] = useState<Option | null>(serversOptions[0]);

    const { data: isAuthorized, isLoading, refetch } = useIsAuthorizedQuery();

    const [getToken] = useBattleNetDataTokenMutation();
    const [getAuthorization] = useBattleNetDataAuthorizaitonMutation();
    const [disconnect] = useLazyBattleNetDisconenctQuery();
    const [getRealms] = useLazyGetRealmsQuery();

    useEffect(() => {
        const getTokenAsync = async () => {
            try {
                await getToken().unwrap();
                const realms = await getRealms({ regionName }).unwrap();
                setServers(realms);
            } catch (error) {
                console.error("Failed auth to battle net game api data:", error);
            }
        }

        getTokenAsync();
    }, []);

    useEffect(() => {
        if (!servers || servers.length === 0) {
            return;
        }

        const options = servers.map(
            (item) => ({
                value: item.slug,
                label: item.name
            })
        )

        setServersOptions(options);
    }, [servers]);

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

    if (!isAuthorized || isLoading) {
        return (<div>Loading...</div>);
    }

    const selectionUser = () => {
        return (
            <div className="character">
                <div className="select-user">
                    <div>
                        <div>{t("Username")} </div>
                        <input type="text" className="form-control" placeholder="Username" aria-label="Username"
                            ref={usernameRef}
                            onChange={() => setUsername(usernameRef.current?.value)} />
                    </div>
                    <div className="filter-item">
                        <div>{t("Server")}</div>
                        <Select<Option>
                            className="options"
                            options={serversOptions}
                            value={serverValue}
                            onChange={(selected) => setServerValue(selected)}
                        />
                    </div>
                    <div className="battle-net">
                        <div className="status">{isAuthorized.authenticated ? 'connected' : 'not connected'}</div>
                        <div className={`auth btn-shadow ${isAuthorized.authenticated ? 'connected' : 'not-connected'}`}
                            onClick={isAuthorized.authenticated ? () => {} : getAuthorizationTokenAsync}>
                            <FontAwesomeIcon
                                icon={isAuthorized.authenticated ? faUser : faPlus}
                            />
                            <div>{t("BattleNet")}</div>
                        </div>
                    </div>
                    {isAuthorized.authenticated &&
                        <div className="exit" onClick={battleNetDisconnect}>
                            <FontAwesomeIcon
                                icon={faClose}
                            />
                        </div>
                    }
                </div>
                <div className="selected-user">
                    <div>{t("SelectedUser")}: </div>
                    <h5>{username}</h5>
                </div>
            </div>
        );
    }

    const charactersData = () => {
        return (
            <div className="character">
                {selectionUser()}
                <div className="btn-shadow"
                    onClick={() => setShowSummary(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Summary")}</div>
                </div>
                {showSummary &&
                    <CharacterSummary
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowMythicKeystone(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("MythicKeystone")}</div>
                </div>
                {showMythicKeystone &&
                    <CharacterMythicKeystone
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowRaids(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Raids")}</div>
                </div>
                {showRaids &&
                    <CharacterRaids
                        isRaids={true}
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowDungeons(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Dungeons")}</div>
                </div>
                {showDungeons &&
                    <CharacterRaids
                        isRaids={false}
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowAchievements(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Achievements")}</div>
                </div>
                {showAchievements &&
                    <AchievementsCategory
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowReputations(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Reputations")}</div>
                </div>
                {showReputations &&
                    <CharacterReputations
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowMounts(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Mounts")}</div>
                </div>
                {showMounts &&
                    <CharacterMounts
                    />
                }
            </div>
        );
    }

    return (
        <WoWGameDataContext.Provider value={{
            t: t,
            username: username ? username : NoneValue.NONE_VALUE,
            serverName: serverValue ? serverValue.value : NoneValue.NONE_VALUE,
            regionName: regionName
        }}>
            {charactersData()}
        </WoWGameDataContext.Provider>
    );
}

export default WoWGameData;