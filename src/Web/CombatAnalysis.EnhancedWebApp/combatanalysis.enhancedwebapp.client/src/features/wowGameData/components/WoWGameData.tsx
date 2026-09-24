import { faClose, faLocationCrosshairs, faPlus, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useBattleNetDataAuthorizaitonMutation, useBattleNetDataTokenMutation, useIsAuthorizedQuery, useLazyBattleNetDisconenctQuery } from '../api/BattleNetData.api';
import CharacterMounts from './CharacterMounts';
import CharacterMythicKeystone from './CharacterMythicKeystone';
import CharacterReputations from './CharacterReputations';
import CharacterSummary from './CharacterSummary';
import CharacterRaids from './CharacterRaids';

import './WoWGameData.scss';

const WoWGameData: React.FC = () => {
    const { t } = useTranslation('wowGameData');

    const usernameRef = useRef<HTMLInputElement | null>(null);

    const [username, setUsername] = useState<string | undefined>();
    const [showSummary, setShowSummary] = useState<boolean>(false);
    const [showMythicKeystone, setShowMythicKeystone] = useState<boolean>(false);
    const [showRaids, setShowRaids] = useState<boolean>(false);
    const [showDungeons, setShowDungeons] = useState<boolean>(false);
    const [showReputations, setShowReputations] = useState<boolean>(false);
    const [showMounts, setShowMounts] = useState<boolean>(false);

    const { data: isAuthorized, isLoading, refetch } = useIsAuthorizedQuery();

    const [getToken] = useBattleNetDataTokenMutation();
    const [getAuthorization] = useBattleNetDataAuthorizaitonMutation();
    const [disconnect] = useLazyBattleNetDisconenctQuery();

    useEffect(() => {
        const getTokenAsync = async () => {
            try {
                await getToken().unwrap();
            } catch (error) {
                console.error("Failed auth to battle net game api data:", error);
            }
        }

        getTokenAsync();
    }, []);

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
                    <div>{t("Username")} </div>
                    <input type="text" className="form-control" placeholder="Username" aria-label="Username"
                        ref={usernameRef}
                        onChange={() => setUsername(usernameRef.current?.value)} />
                    <div className="battle-net">
                        <div className="status">{isAuthorized.authenticated ? 'connected' : 'not connected'}</div>
                        <div className={`auth btn-shadow ${isAuthorized.authenticated ? 'connected' : 'not-connected'}`}
                            onClick={getAuthorizationTokenAsync}>
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
                    username={username}
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
                    username={username}
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
                    username={username}
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
                    username={username}
                    isRaids={false}
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
                    username={username}
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
                    username={username}
                />
            }
        </div>
    );
}

export default WoWGameData;