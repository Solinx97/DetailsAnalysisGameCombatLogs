import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import { useGetAccountSlotTransmogsQuery } from '../../api/WoWAccount.api';
import AccountTransmogSlot from './AccountTransmogSlot';

const AccountTransmogSlots: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { regionName } = context;

    const [collectionItemType, seCollectionItemType] = useState<string>("0");

    const { data: allCollectionItems, isLoading, error } = useGetAccountSlotTransmogsQuery({ regionName });

    if (!allCollectionItems || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!allCollectionItems || isLoading}
        />);
    }

    return (
        <ul className="transmog-slots">
            {Object.entries(allCollectionItems).map(([key, collection]) => (
                <li>
                    <div className="btn-shadow"
                        onClick={() => seCollectionItemType(prev => prev === key ? "0" : key)}>
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{key}</div>
                    </div>
                    {collectionItemType === key &&
                        <AccountTransmogSlot
                            allCollectionItems={collection}
                        />
                    }
                </li>
            ))
            }
        </ul>
    );
}

export default AccountTransmogSlots;