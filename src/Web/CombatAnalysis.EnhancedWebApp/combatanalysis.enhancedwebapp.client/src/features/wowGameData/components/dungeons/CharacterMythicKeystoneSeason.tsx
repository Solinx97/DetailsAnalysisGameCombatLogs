import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faKey, faDashboard } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterMythicKeystoneSeasonQuery } from '../../api/WoWCharacter.api';

const CharacterMythicKeystoneSeason: React.FC<{ seasonId: number }> = ({ seasonId }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);

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
                                <div>{run.mythicRating.rating.toFixed(2)}</div>
                            </div>
                            <div>{run.dungeon.name}</div>
                        </li>
                    ))
                    }
                </ul>
            }
        </div>
    );
}

export default CharacterMythicKeystoneSeason;