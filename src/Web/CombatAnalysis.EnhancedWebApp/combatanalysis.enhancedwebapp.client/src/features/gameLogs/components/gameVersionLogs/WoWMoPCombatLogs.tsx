import type { RootState } from '@/app/Store';
import Loading from '@/shared/components/Loading';
import React from 'react';
import { useSelector } from 'react-redux';
import { useGetCombatLogsQuery } from '../../api/GameLogs.api';
import GameCombatLogItem from '../GameCombatLogItem';

interface CombatLogsProps {
    selectedLogType: number;
    gameVersion: number;
    t: (key: string) => string;
}

const WoWMoPCombatLogs: React.FC<CombatLogsProps> = ({ selectedLogType, gameVersion, t }) => {
    const user = useSelector((state: RootState) => state.user.value);

    const { data: combatLogs, isLoading } = useGetCombatLogsQuery({ logType: selectedLogType, gameVersion, appUserId: user ? user.id : null });

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="main-information__container">
            <ul className="combats__container">
                {combatLogs?.map((item) => (
                    <li key={item.id}>
                        <GameCombatLogItem
                            t={t}
                            appUserId={user ? user.id : "0"}
                            combatLog={item}
                            gameVersion={gameVersion}
                            isAuth={user !== null}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default WoWMoPCombatLogs;
