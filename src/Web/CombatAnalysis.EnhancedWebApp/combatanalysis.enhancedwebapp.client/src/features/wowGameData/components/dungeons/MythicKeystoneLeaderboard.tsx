import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faKey, faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import { useGetMythicKeystoneLeaderboardQuery } from '../../api/WoWData.api';
import type { MythicKeystoneDungeonLeaderboardModel } from '../../types/mythicKeystone/MythicKeystoneDungeonLeaderboardModel';
import MythicKeystoneLeaderboardDungeon from './MythicKeystoneLeaderboardDungeon';

import useFormatting from '@/shared/hooks/useFormatting';
import './Dungeons.scss';

const MythicKeystoneLeaderboard: React.FC<{ connectedRealmId: number, periodId: number }> = ({ connectedRealmId, periodId }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const { getDate } = useFormatting();

    const [selectedDungeon, setSelectedDungeon] = useState<string>("");

    const { data: mythicKeystoneLeaderboard, isLoading, error } = useGetMythicKeystoneLeaderboardQuery({ connectedRealmId, periodId, regionName });

    if (!mythicKeystoneLeaderboard || isLoading || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!mythicKeystoneLeaderboard || isLoading}
        />);
    }

    return (
        <div className="mythic-keystone-leaderboard">
            <div className="mythic-keystone-leaderboar__title">
                <h6>{t("Leaderboard")}</h6>
            </div>
            <ul className="current-period">
                {Object.entries(mythicKeystoneLeaderboard.currentLeaderboards).map(([key, leaderboard]: [string, MythicKeystoneDungeonLeaderboardModel]) => (
                    <li key={key} className="dungeon">
                        <div className="level">
                            <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faKey}
                                />
                                <div>{leaderboard.leadingGroups[0].keystoneLevel}</div>
                            </div>
                            <div className="special">{leaderboard.leadingGroups[0].mythicRating.rating.toFixed(2)}</div>
                            <div className="key-level special">{t("PeriodStartAt")}</div>
                            <div className="special">{getDate(leaderboard.periodStartTime)}</div>
                            <div className="key-level special">{t("PeriodEndAt")}</div>
                            <div className="special">{getDate(leaderboard.periodEndTime)}</div>
                        </div>
                        <div className="dungeon-name">
                            <div className="btn-shadow"
                                onClick={() => setSelectedDungeon(prev => prev === leaderboard.name ? "" : leaderboard.name)}>
                                <FontAwesomeIcon
                                    icon={faLocationCrosshairs}
                                />
                                <div>{leaderboard.name}</div>
                            </div>
                        </div>
                        {selectedDungeon === leaderboard.name &&
                            <MythicKeystoneLeaderboardDungeon
                                groups={leaderboard.leadingGroups}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default MythicKeystoneLeaderboard;