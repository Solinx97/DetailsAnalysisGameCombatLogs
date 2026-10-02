import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faDashboard, faKey, faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterMythicKeystoneSeasonQuery } from '../../api/WoWCharacter.api';
import SelectedMythicKeystoneDungeon from './SelectedMythicKeystoneDungeon';

const CharacterMythicKeystoneSeason: React.FC<{ seasonId: number }> = ({ seasonId }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);
    const [selectedDungeon, setSelectedDungeon] = useState<string | undefined>("");

    const { data: mythicKeystoneSeason, isLoading, error } = useGetCharacterMythicKeystoneSeasonQuery({ username, seasonId, serverName, regionName },
        {
            skip: isSkipRequest
        });

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!mythicKeystoneSeason || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!mythicKeystoneSeason || isLoading}
        />);
    }

    return (
        <div className="mythic-keystone">
            <div className="rating">
                <div className="btn-shadow">
                    <FontAwesomeIcon
                        icon={faDashboard}
                    />
                    <div>{mythicKeystoneSeason.mythicRating.rating.toFixed(2)}</div>
                </div>
            </div>
            {mythicKeystoneSeason.bestRuns.length === 0
                ? <div>{t("NoAnyKeyFinishedYet")}</div>
                : <ul className="current-period">
                    {mythicKeystoneSeason.bestRuns.map((run, index) => (
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

export default CharacterMythicKeystoneSeason;