import ResponseInformation from '@/shared/components/ResponseInformation';
import type { OptionModel } from '@/shared/types/OptionModel';
import { faArrowsSpin } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import Select from 'react-select';
import { useGetAccountCharactersListQuery } from '../api/WoWAccount.api';
import AccountDashboardItems from './AccountDashboardItems';

interface AccountDashboardProps {
    regionName: string;
    getAuthorizationTokenAsync: () => Promise<void>;
    isAuthorized: boolean;
    t: (key: string) => string;
}

const AccountDashboard: React.FC<AccountDashboardProps> = ({ regionName, getAuthorizationTokenAsync, isAuthorized, t }) => {
    const { data: characters, isLoading, error } = useGetAccountCharactersListQuery({ regionName });

    const [charactersOptions, setCharactersOptions] = useState<OptionModel[]>([]);
    const [characterValue, setCharacterValue] = useState<OptionModel | null>(charactersOptions[0]);

    useEffect(() => {
        if (!characters || characters.length === 0) {
            return;
        }

        const options = characters.map(
            (item) => ({
                value: `${item.name ? item.name : "0"}#${item.realm.slug}`,
                label: `${item.name ? item.name : "0"} (${item.level})`
            })
        )

        setCharactersOptions(options);
    }, [characters]);

    if (!isAuthorized) {
        return (
            <div className="account-data">
                <div className="auth">
                    <div className="btn-shadow" onClick={getAuthorizationTokenAsync}>
                        <FontAwesomeIcon
                            icon={faArrowsSpin}
                            color="orange"
                        />
                        <div>{t("MustConnectBattleNet")}</div>
                    </div>
                </div>
            </div>
        );
    }

    if (!characters || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!characters || isLoading}
        />);
    }

    return (
        <div className="account-dashboard">
            <div className="filters">
                <div className="filter-item">
                    <div>{t("Character")}</div>
                    <Select<OptionModel>
                        className="options"
                        options={charactersOptions}
                        value={characterValue}
                        onChange={(selected) => setCharacterValue(selected)}
                    />
                </div>
            </div>
            {characterValue &&
                <AccountDashboardItems
                    regionName={regionName}
                    characterName={characterValue.value.split('#')[0]}
                    serverName={characterValue.value.split('#')[1]}
                    t={t}
                />
            }
        </div>
    );
}

export default AccountDashboard;