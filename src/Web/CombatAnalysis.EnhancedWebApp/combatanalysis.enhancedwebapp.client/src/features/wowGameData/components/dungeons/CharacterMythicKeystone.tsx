import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faDashboard, faKey, faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterMythicKeystoneQuery } from '../../api/WoWCharacter.api';
import CharacterMythicKeystoneSeason from './CharacterMythicKeystoneSeason';
import MythicKeystoneLeaderboard from './MythicKeystoneLeaderboard';
import SelectedMythicKeystoneDungeon from './SelectedMythicKeystoneDungeon';

import './Dungeons.scss';

const CharacterMythicKeystone: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);
    const [showLeaderboard, setShowLeaderborad] = useState<boolean>(false);
    const [selectedSeasonId, setSelectedSeasonId] = useState<number>(0);
    const [selectedDungeon, setSelectedDungeon] = useState<string | undefined>("");

    const { data: mythicKeystone, isLoading, error } = useGetCharacterMythicKeystoneQuery({ username, serverName, regionName },
        {
            skip: isSkipRequest
        });

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!mythicKeystone || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!mythicKeystone || isLoading}
        />);
    }

    const seasonsHandle = (seasonId: number) => {
        setSelectedSeasonId(prev => prev === seasonId ? 0 : seasonId);
    }

    return (
        <div className="mythic-keystone">
            <ul className="seasons">
                {mythicKeystone.seasons.map(season => (
                    <li key={season.id} className="season">
                        <div className="btn-shadow"
                            onClick={() => seasonsHandle(season.id)}>
                            <FontAwesomeIcon
                                icon={selectedSeasonId === season.id ? faLocationCrosshairs : faPlus}
                            />
                            <div>{t("Season")} {season.id}</div>
                        </div>
                    </li>
                ))
                }
            </ul>
            {selectedSeasonId > 0 &&
                <>
                    <div>{t("Season")} {selectedSeasonId}</div>
                    <CharacterMythicKeystoneSeason
                        seasonId={selectedSeasonId}
                    />
                </>
            }
            <div className="leaderboard">
                <div className="btn-shadow"
                    onClick={() => setShowLeaderborad(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={showLeaderboard ? faLocationCrosshairs : faPlus}
                    />
                    <div>{t("Leaderboard")}</div>
                </div>
            </div>
            {showLeaderboard &&
                <MythicKeystoneLeaderboard
                    connectedRealmId={mythicKeystone.character.realm.id}
                    periodId={mythicKeystone.currentPeriod.period.id}
                />
            }
            <div className="mythic-keystone__title">
                <h6>{t("MythicKeystoneRaiting")}</h6>
                <div className="btn-shadow">
                    <FontAwesomeIcon
                        icon={faDashboard}
                    />
                    <div>{mythicKeystone?.currentMythicRating.rating.toFixed(2)}</div>
                </div>
            </div>
            {mythicKeystone.currentPeriod.bestRuns.length === 0
                ? <div>{t("NoAnyKeyFinishedYet")}</div>
                : <ul className="current-period">
                    {mythicKeystone.currentPeriod.bestRuns.map((run, index) => (
                        <li key={index} className="run">
                            <div className="level">
                                <div className="btn-shadow">
                                    <FontAwesomeIcon
                                        icon={faKey}
                                    />
                                    <div>{run.level}</div>
                                </div>
                                <div className="special">{run.mythicRating.rating.toFixed(2)}</div>
                            </div>
                            <div className="dungeon-name">
                                <div className="btn-shadow"
                                    onClick={() => setSelectedDungeon(prev => prev === run.dungeon.name ? "" : run.dungeon.name)}>
                                    <FontAwesomeIcon
                                        icon={faLocationCrosshairs}
                                    />
                                    <div>{run.dungeon.name}</div>
                                </div>
                            </div>
                            {selectedDungeon === run.dungeon.name &&
                                <SelectedMythicKeystoneDungeon
                                    run={run}
                                />
                            }
                        </li>
                    ))
                    }
                </ul>
            }
        </div>
    );
}

export default CharacterMythicKeystone;