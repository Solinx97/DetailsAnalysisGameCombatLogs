import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import { useGetAccountCharactersQuery } from '../../api/WoWAccount.api';
import RealmCharacters from './RealmCharacters';

import './AccountCharacters.scss';

const AccountCharacters: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;

    const [selectedAccountId, setSelectedAccountId] = useState(0);

    const { data: characters, isLoading, error } = useGetAccountCharactersQuery({ regionName });

    if (!characters || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!characters || isLoading}
        />);
    }

    return (
        <div className="account-characters">
            <div className="account-characters__title">
                <h6>{t("Characters")}</h6>
            </div>
            <ul className="accounts">
                {characters.wowAccounts.map((account, index) => (
                    <li className="accounts__account" key={index}>
                        <div className="btn-shadow"
                            onClick={() => setSelectedAccountId(prev => prev === account.id ? 0 : account.id)}>
                            <FontAwesomeIcon
                                icon={faLocationCrosshairs}
                            />
                            <div>{t("Account")}: {index + 1}</div>
                        </div>
                        {selectedAccountId === account.id &&
                            <RealmCharacters
                                characters={account.characters}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default AccountCharacters;