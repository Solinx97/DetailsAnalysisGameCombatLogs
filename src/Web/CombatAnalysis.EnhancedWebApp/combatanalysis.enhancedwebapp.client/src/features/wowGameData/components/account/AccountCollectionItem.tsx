import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { WoWAccountCollectionType } from '@/shared/helpers/EnumHelper';
import { faQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext } from 'react';
import { useGetMountQuery, useGetPetQuery } from '../../api/WoWData.api';

const AccountCollectionItem: React.FC<{ colelctionType: number, colelctionId: number }> = ({ colelctionType, colelctionId }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { regionName } = context;

    const { data: collectionItemInfo, isLoading, error } = colelctionType === WoWAccountCollectionType["MOUNT"]
        ? useGetMountQuery({ mountId: colelctionId, regionName })
        : useGetPetQuery({ petId: colelctionId, regionName });

    if (!collectionItemInfo || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!collectionItemInfo || isLoading}
        />);
    }

    return (
        <div className="details">
            <div>{collectionItemInfo.description}</div>
            <div className="how-receive">
                <div className="btn-shadow">
                    <FontAwesomeIcon
                        icon={faQuestion}
                    />
                    <div>{collectionItemInfo.source?.name}</div>
                </div>
            </div>
        </div>
    );
}

export default AccountCollectionItem;