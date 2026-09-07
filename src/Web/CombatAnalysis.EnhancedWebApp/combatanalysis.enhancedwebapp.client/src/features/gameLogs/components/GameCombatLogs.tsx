import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import type { RootState } from '../../../app/Store';
import PersonalTabs from './PersonalTabs';
import WoWMoPCombatLogs from './gameVersionLogs/WoWMoPCombatLogs';
import WoWMidnightCombatLogs from './gameVersionLogs/WoWMidnightCombatLogs';

import './GameCombatLogs.scss';

const GameCombatLogs: React.FC = () => {
    const GAME_VERSION = {
        WoWMoP: 0,
        WoWMidnight: 1
    };

    const { t } = useTranslation('combatDetails/mainInformation');

    const user = useSelector((state: RootState) => state.user.value);

    const [selectedLogType, setSelectedLogType] = useState(0);

    return (
        <div className="main-information">
            <div className="main-information__title">
                <div>{t("Logs")}</div>
                <div className="log-types">
                    <div className={`log-types__item${selectedLogType === 0 ? '_selected' : ''}`} onClick={() => setSelectedLogType(0)}>{t("Public")}</div>
                    <div className={`log-types__item${selectedLogType === 1 ? '_selected' : ''} ${user === null ? 'not-allowed' : ''}`} onClick={user === null ? () => { } : () => setSelectedLogType(1)}>{t("Personal")}</div>
                </div>
            </div>
            <PersonalTabs
                tab={0}
                tabs={[
                    {
                        id: 0,
                        header: t("WoW MoP"),
                        content: <WoWMoPCombatLogs
                            selectedLogType={selectedLogType}
                            gameVersion={GAME_VERSION.WoWMoP}
                            t={t}
                        />
                    },
                    {
                        id: 1,
                        header: t("WoW Midnight"),
                        content: <WoWMidnightCombatLogs
                            selectedLogType={selectedLogType}
                            gameVersion={GAME_VERSION.WoWMidnight}
                            t={t}
                        />
                    }
                ]}
                tabsClassName={"charts"}
            />
        </div>
    );
}

export default GameCombatLogs;