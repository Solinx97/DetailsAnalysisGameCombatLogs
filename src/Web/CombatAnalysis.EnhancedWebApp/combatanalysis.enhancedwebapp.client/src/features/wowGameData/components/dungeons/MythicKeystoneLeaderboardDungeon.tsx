import WoWGameDataContext from '@/context/WoWGameDataContext';
import useFormatting from '@/shared/hooks/useFormatting';
import { faKey } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import type { MythicKeystoneDungeonLeaderboardGroupModel } from '../../types/mythicKeystone/MythicKeystoneDungeonLeaderboardGroupModel';
import Character from '../Character';

const MythicKeystoneLeaderboardDungeon: React.FC<{ groups: MythicKeystoneDungeonLeaderboardGroupModel[] }> = ({ groups }) => {
    const defaultPageSize = 25;

    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;

    const { getDate } = useFormatting();

    const [loadMore, setLoadMore] = useState<boolean>(false);
    const [size, setSize] = useState<number>(defaultPageSize);

    useEffect(() => {
        if (!loadMore) {
            return;
        }

        setSize(prev => prev + defaultPageSize);
        setLoadMore(false);
    }, [loadMore]);

    return (
        <ul className="mythic-keystone-leaderboard__dungeon">
            {groups.slice(0, size).map((group, index) => (
                <li key={index} className="details">
                    <div className="level">
                        <div className="key-level special">{t("Ranking")}</div>
                        <div className="special">{group.ranking}</div>
                        <div className="key-level special">{t("Key")}</div>
                        <div className="btn-shadow">
                            <FontAwesomeIcon
                                icon={faKey}
                            />
                            <div>{group.keystoneLevel}</div>
                        </div>
                        <div className="special">{group.mythicRating.rating.toFixed(2)}</div>
                        <div className="key-level special">{t("CompletedAt")}</div>
                        <div className="special">{getDate(group.completedTime)}</div>
                        <div className="key-level special">{t("Duration")}</div>
                        <div className="special">{group.duration}</div>
                    </div>
                    <ul className="members">
                        {group.members.map((member, index1) => (
                            <li key={`${index}-${index1}`}>
                                <Character
                                    character={member.character}
                                />
                            </li>
                        ))
                        }
                    </ul>
                </li>
            ))}
            {size < groups.length &&
                <li className="load-more" onClick={() => setLoadMore(true)}>Load more</li>
            }
        </ul>
    );
}

export default MythicKeystoneLeaderboardDungeon;