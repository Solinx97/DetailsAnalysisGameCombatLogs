import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { WoWAccountCollectionType } from '@/shared/helpers/EnumHelper';
import { faQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext } from 'react';
import { useGetMountQuery, useGetPetQuery, useGetToyQuery } from '../../api/WoWData.api';

const AccountCollectionItem: React.FC<{ collectionType: number, colelctionId: number }> = ({ collectionType, colelctionId }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { regionName } = context;

    const mountCollectionItemInfoQuery = useGetMountQuery({ mountId: colelctionId, regionName },
        {
            skip: collectionType !== WoWAccountCollectionType.MOUNT,
        }
    );
    const petCollectionItemInfoQuery = useGetPetQuery({ petId: colelctionId, regionName },
        {
            skip: collectionType !== WoWAccountCollectionType.PET,
        }
    );
    const toyCollectionItemInfoQuery = useGetToyQuery({ toyId: colelctionId, regionName },
        {
            skip: collectionType !== WoWAccountCollectionType.TOY,
        }
    );

    const collectionItemInfoItems =
        collectionType === WoWAccountCollectionType.MOUNT
            ? mountCollectionItemInfoQuery.data
            : collectionType === WoWAccountCollectionType.PET
                ? petCollectionItemInfoQuery.data
                : toyCollectionItemInfoQuery.data;

    const isLoading =
        collectionType === WoWAccountCollectionType.MOUNT
            ? mountCollectionItemInfoQuery.isLoading
            : collectionType === WoWAccountCollectionType.PET
                ? petCollectionItemInfoQuery.isLoading
                : toyCollectionItemInfoQuery.isLoading;

    const error =
        collectionType === WoWAccountCollectionType.MOUNT
            ? mountCollectionItemInfoQuery.error
            : collectionType === WoWAccountCollectionType.PET
                ? petCollectionItemInfoQuery.error
                : toyCollectionItemInfoQuery.error;

    if (!collectionItemInfoItems || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!collectionItemInfoItems || isLoading}
        />);
    }

    return (
        <div className="details">
            <div>{collectionItemInfoItems.description}</div>
            <div className="how-receive">
                <div className="btn-shadow">
                    <FontAwesomeIcon
                        icon={faQuestion}
                    />
                    <div>{collectionItemInfoItems.source?.name}</div>
                </div>
            </div>
        </div>
    );
}

export default AccountCollectionItem;