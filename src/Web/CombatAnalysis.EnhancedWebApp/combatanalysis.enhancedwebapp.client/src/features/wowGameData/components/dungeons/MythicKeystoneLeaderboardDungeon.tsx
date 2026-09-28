import { faKey } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { MythicKeystoneDungeonLeaderboardGroupModel } from '../../types/mythicKeystone/MythicKeystoneDungeonLeaderboardGroupModel';

const MythicKeystoneLeaderboardDungeon: React.FC<{ groups: MythicKeystoneDungeonLeaderboardGroupModel[] }> = ({ groups }) => {
    return (
        <div className="mythic-keystone-leaderboar">
            <ul>
                {groups.map((group) => (
                    <li>
                        <div className="level">
                            <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faKey}
                                />
                                <div>{group.keystoneLevel}</div>
                            </div>
                            <div>{group.mythicRating.rating.toFixed(2)}</div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MythicKeystoneLeaderboardDungeon;