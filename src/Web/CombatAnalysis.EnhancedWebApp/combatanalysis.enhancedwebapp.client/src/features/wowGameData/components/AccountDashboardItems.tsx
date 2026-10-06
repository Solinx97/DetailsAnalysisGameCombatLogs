import ResponseInformation from '@/shared/components/ResponseInformation';
import { useGetDashboardQuery } from '../api/WoWAccount.api';

const AccountDashboardItems: React.FC<{ regionName: string, serverName: string, characterName: string, t: (key: string) => string }> = ({ regionName, serverName, characterName, t }) => {
    const { data: dashboard, isLoading, isFetching, error } = useGetDashboardQuery({ regionName, serverName, characterName },
        {
            skip: characterName.trim().length === 0
        }
    );

    if (!dashboard || isLoading || isFetching || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!dashboard || isLoading || isFetching}
        />);
    }

    return (
        <ul className="account-dashboard">
            <li className="item center-content">
                <div className="content">
                    <div className="special special-name">{t("Character")}</div>
                    <div className="special">{characterName}</div>
                </div>
                <div className="content">
                    <div className="special special-name">{t("ILvl")}</div>
                    <div className="special">{dashboard.summary.equippedItemLevel}</div>
                </div>
                <div className="content">
                    <div className="special special-name">{t("KeystoneRaiting")}</div>
                    <div className="special">{dashboard.mythicKeystoneRating}</div>
                </div>
                <div className="content">
                    <div className="special special-name">{t("AccountCharacters")}</div>
                    <div className="special">{dashboard.charactersCount} ({dashboard.maxLevelCharactersCount})</div>
                </div>
            </li>
            <li className="item" style={{ "--i": 0 } as React.CSSProperties}>
                <div className="content">
                    <div className="special special-name">{t("Achievements")}</div>
                    <div className="special">{dashboard.achievementsReceived} / {dashboard.achievementsCount}</div>
                </div>
            </li>
            <li className="item" style={{ "--i": 1 } as React.CSSProperties}>
                <div className="content">
                    <div className="special special-name">{t("Mounts")}</div>
                    <div className="special">{dashboard.mountsReceived} / {dashboard.mountsCount}</div>
                </div>
            </li>
            <li className="item" style={{ "--i": 2 } as React.CSSProperties}>
                <div className="content">
                    <div className="special special-name">{t("Pets")}</div>
                    <div className="special">{dashboard.petsReceived} / {dashboard.petsCount}</div>
                </div>
            </li>
            <li className="item" style={{ "--i": 3 } as React.CSSProperties}>
                <div className="content">
                    <div className="special special-name">{t("Toys")}</div>
                    <div className="special">{dashboard.toysReceived} / {dashboard.toysCount}</div>
                </div>
            </li>
            <li className="item" style={{ "--i": 4 } as React.CSSProperties}>
                <div className="content">
                    <div className="special special-name">{t("Decors")}</div>
                    <div className="special">{dashboard.decorsReceived} / {dashboard.decorsCount}</div>
                </div>
            </li>
            <li className="item" style={{ "--i": 5 } as React.CSSProperties}>
                <div className="content">
                    <div className="special special-name">{t("SetTransmogs")}</div>
                    <div className="special">{dashboard.setTransmogsReceived} / {dashboard.setTransmogsCount}</div>
                </div>
            </li>
            <li className="item" style={{ "--i": 6 } as React.CSSProperties}>
                <div className="content">
                    <div className="special special-name">{t("Transmogs")}</div>
                    <div className="special">{dashboard.transmogsReceived} / {dashboard.transmogsCount}</div>
                </div>
            </li>
        </ul>
    );
}

export default AccountDashboardItems;