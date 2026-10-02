import WoWGameDataContext from '@/context/WoWGameDataContext';
import { WoWAccountCollectionType } from '@/shared/helpers/EnumHelper';
import type { OptionMode } from '@/shared/types/OptionMode';
import { faArrowsSpin, faCheck, faClose, faLocationCrosshairs, faPlus, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Select from 'react-select';
import { useBattleNetDataAuthorizaitonMutation, useBattleNetDataTokenMutation, useIsAuthorizedQuery, useLazyBattleNetDisconenctQuery } from '../api/BattleNetData.api';
import { useLazyGetRealmsQuery } from '../api/WoWData.api';
import type { WoWRealmModel } from '../types/WoWRealmModel';
import AccountCharacters from './account/AccountCharacters';
import AccountCollections from './account/AccountCollections';
import AchievementsCategory from './achievements/AchievementsCategory';
import AchievementsStatistics from './achievements/AchievementsStatistics';
import CharacterReputations from './CharacterReputations';
import CharacterDungeons from './dungeons/CharacterDungeons';
import CharacterMythicKeystone from './dungeons/CharacterMythicKeystone';
import CharacterEquipments from './equipments/CharacterEquipments';
import CharacterProfessions from './professions/CharacterProfessions';
import WoWToken from './WoWToken';

import './WoWGameData.scss';

const WoWGameData: React.FC = () => {
    const regionName = "eu";

    const { t } = useTranslation('wowGameData');

    const usernameRef = useRef<HTMLInputElement | null>(null);

    const [servers, setServers] = useState<WoWRealmModel[]>([]);

    const [username, setUsername] = useState<string>("");

    const [showCharacters, setShowCharacters] = useState<boolean>(false);
    const [showDecors, setShowDecors] = useState<boolean>(false);
    const [showStatistics, setShowStatistics] = useState<boolean>(false);
    const [showEquipments, setShowEquipments] = useState<boolean>(false);
    const [showMythicKeystone, setShowMythicKeystone] = useState<boolean>(false);
    const [showRaids, setShowRaids] = useState<boolean>(false);
    const [showDungeons, setShowDungeons] = useState<boolean>(false);
    const [showAchievements, setShowAchievements] = useState<boolean>(false);
    const [showReputations, setShowReputations] = useState<boolean>(false);
    const [showMounts, setShowMounts] = useState<boolean>(false);
    const [showPets, setShowPets] = useState<boolean>(false);
    const [showToys, setShowToys] = useState<boolean>(false);
    const [showProfessions, setShowProfessions] = useState<boolean>(false);

    const [serversOptions, setServersOptions] = useState<OptionMode[]>([]);
    const [serverValue, setServerValue] = useState<OptionMode | null>(serversOptions[0]);

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
                label: item.name ? item.name : ""
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

    const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            setUsername(event.currentTarget.value);
        }
    }

    if (!isAuthorized || isLoading) {
        return (<div>Loading...</div>);
    }

    const selectionUser = () => {
        return (
            <div className="account__character">
                <div className="select-character">
                    <div>
                        <div>{t("Username")} </div>
                        <input type="text" className="form-control" placeholder="Username" aria-label="Username"
                            ref={usernameRef}
                            onKeyDown={handleKeyDown}
                            defaultValue={username} />
                    </div>
                    <div className="filter-item">
                        <div>{t("Server")}</div>
                        <Select<OptionMode>
                            className="options"
                            options={serversOptions}
                            value={serverValue}
                            onChange={(selected) => setServerValue(selected)}
                        />
                    </div>
                    <div className="battle-net">
                        <div className="status">{isAuthorized.authenticated ? 'connected' : 'not connected'}</div>
                        <div className="actions">
                            <div className={`auth btn-shadow ${isAuthorized.authenticated ? 'connected' : 'not-connected'}`}
                                onClick={isAuthorized.authenticated ? () => { } : getAuthorizationTokenAsync}>
                                <FontAwesomeIcon
                                    icon={isAuthorized.authenticated ? faUser : faPlus}
                                />
                                <div>{t("BattleNet")}</div>
                            </div>
                            {isAuthorized.authenticated &&
                                <div className="exit" onClick={battleNetDisconnect}>
                                    <FontAwesomeIcon
                                        icon={faClose}
                                    />
                                </div>
                            }
                        </div>
                    </div>
                </div>
                <div className="selected-character">
                    <div>{t("SelectedCharacter")}: </div>
                    <h5>{username}</h5>
                </div>
            </div>
        );
    }

    const accountData = () => {
        return (
            <>
                <div className="account-data">
                    <div className="btn-shadow"
                        onClick={() => setShowCharacters(prev => !prev)}>
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{t("Characters")}</div>
                    </div>
                    <div className="auth">
                        {isAuthorized.authenticated
                            ? <FontAwesomeIcon
                                icon={faCheck}
                                color="green"
                            />
                            : <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faArrowsSpin}
                                    color="orange"
                                />
                                <div>{t("MustConnectBattleNet")}</div>
                            </div>
                        }
                    </div>
                </div>
                {showCharacters &&
                    <AccountCharacters
                    />
                }
                <div className="account-data">
                    <div className="btn-shadow"
                        onClick={() => setShowMounts(prev => !prev)}>
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{t("Mounts")}</div>
                    </div>
                    <div className="auth">
                        {isAuthorized.authenticated
                            ? <FontAwesomeIcon
                                icon={faCheck}
                                color="green"
                            />
                            : <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faArrowsSpin}
                                    color="orange"
                                />
                                <div>{t("MustConnectBattleNet")}</div>
                            </div>
                        }
                    </div>
                </div>
                {showMounts &&
                    <AccountCollections
                        collectionType={WoWAccountCollectionType["MOUNT"]}
                        isAllowInfo={true}
                    />
                }
                <div className="account-data">
                    <div className="btn-shadow"
                        onClick={() => setShowPets(prev => !prev)}>
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{t("Pets")}</div>
                    </div>
                    <div className="auth">
                        {isAuthorized.authenticated
                            ? <FontAwesomeIcon
                                icon={faCheck}
                                color="green"
                            />
                            : <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faArrowsSpin}
                                    color="orange"
                                />
                                <div>{t("MustConnectBattleNet")}</div>
                            </div>
                        }
                    </div>
                </div>
                {showPets &&
                    <AccountCollections
                        collectionType={WoWAccountCollectionType["PET"]}
                        isAllowInfo={true}
                    />
                }
                <div className="account-data">
                    <div className="btn-shadow"
                        onClick={() => setShowToys(prev => !prev)}>
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{t("Toys")}</div>
                    </div>
                    <div className="auth">
                        {isAuthorized.authenticated
                            ? <FontAwesomeIcon
                                icon={faCheck}
                                color="green"
                            />
                            : <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faArrowsSpin}
                                    color="orange"
                                />
                                <div>{t("MustConnectBattleNet")}</div>
                            </div>
                        }
                    </div>
                </div>
                {showToys &&
                    <AccountCollections
                        collectionType={WoWAccountCollectionType["TOY"]}
                        isAllowInfo={true}
                    />
                }
                <div className="account-data">
                    <div className="btn-shadow_disabled">
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{t("Transmogs")}</div>
                    </div>
                    <div className="auth">
                        {isAuthorized.authenticated
                            ? <FontAwesomeIcon
                                icon={faCheck}
                                color="green"
                            />
                            : <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faArrowsSpin}
                                    color="orange"
                                />
                                <div>{t("MustConnectBattleNet")}</div>
                            </div>
                        }
                    </div>
                </div>
            </>
        );
    }

    const charactersData = () => {
        return (
            <div className="account">
                {selectionUser()}
                <WoWToken />
                {accountData()}
                <div className="btn-shadow"
                    onClick={() => setShowDecors(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Decors")}</div>
                </div>
                {showDecors &&
                    <AccountCollections
                        collectionType={WoWAccountCollectionType["DECOR"]}
                        isAllowInfo={false}
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowStatistics(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Statistics")}</div>
                </div>
                {showStatistics &&
                    <AchievementsStatistics
                    />
                }
                <div className="btn-shadow"
                    onClick={() => setShowEquipments(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Summary")}</div>
                </div>
                {showEquipments &&
                    <CharacterEquipments
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
                    <CharacterDungeons
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
                    <CharacterDungeons
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
                    onClick={() => setShowProfessions(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Professions")}</div>
                </div>
                {showProfessions &&
                    <CharacterProfessions
                    />
                }
            </div>
        );
    }

    return (
        <WoWGameDataContext.Provider value={{
            t: t,
            username: username,
            serversOptions: serversOptions,
            setUsername: setUsername,
            setServerName: setServerValue,
            serverName: serverValue ? serverValue.value : " ",
            regionName: regionName
        }}>
            {charactersData()}
        </WoWGameDataContext.Provider>
    );
}

export default WoWGameData;