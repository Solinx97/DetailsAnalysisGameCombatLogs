import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useGetAccountMountsQuery, useGetAccountPetsQuery } from '../../api/WoWAccount.api';
import type { WoWAccountCollectionItemModel } from '../../types/collections/WoWAccountCollectionItemModel';
import LoadMoreCollections from '../helpers/LoadMoreCollections';
import { WoWAccountCollectionType } from '@/shared/helpers/EnumHelper';
import AccountCollectionItem from './AccountCollectionItem';
import AccountPet from './AccountPet';
import type { WoWAccountPetInfoModel } from '../../types/collections/WoWAccountPetInfoModel';

const AccountCollections: React.FC<{ collectionType: number }> = ({ collectionType }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;

    const [collectionItems, setCollectionItems] = useState<WoWAccountCollectionItemModel[]>([]);
    const [accountMountCount, setAccountMountCount] = useState<number>(0);
    const [onlyNotReceived, setOnlyNotReceived] = useState<boolean>(false);
    const [collectionItemInfoId, seCollectionItemInfoId] = useState<number>(0);

    const mountsQuery = useGetAccountMountsQuery(
        { regionName },
        {
            skip: collectionType !== WoWAccountCollectionType.MOUNT,
        }
    );

    const petsQuery = useGetAccountPetsQuery(
        { regionName },
        {
            skip: collectionType !== WoWAccountCollectionType.PET,
        }
    );

    const allCollectionItems =
        collectionType === WoWAccountCollectionType.MOUNT
            ? mountsQuery.data
            : petsQuery.data;

    const isLoading =
        collectionType === WoWAccountCollectionType.MOUNT
            ? mountsQuery.isLoading
            : petsQuery.isLoading;

    const error =
        collectionType === WoWAccountCollectionType.MOUNT
            ? mountsQuery.error
            : petsQuery.error;

    useEffect(() => {
        if (!allCollectionItems) {
            return;
        }

        setCollectionItems(allCollectionItems);

        setAccountMountCount(allCollectionItems.filter(x => x.info !== null).length);
    }, [allCollectionItems]);

    useEffect(() => {
        if (!allCollectionItems) {
            return;
        }

        if (onlyNotReceived) {
            setCollectionItems(allCollectionItems.filter(x => x.info === null));
        }
        else {
            setCollectionItems(allCollectionItems);
        }
    }, [onlyNotReceived, allCollectionItems]);

    if (!allCollectionItems || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!allCollectionItems || isLoading}
        />);
    }

    const getItem = (item: WoWAccountCollectionItemModel) => {
        return (
            <div className="selected-collection-item">
                <div className="selected-collection-item__name">
                    <div className="action btn-shadow"
                        onClick={() => seCollectionItemInfoId(prev => prev === item.item.id ? 0 : item.item.id)}>
                        <FontAwesomeIcon
                            icon={collectionItemInfoId === item.item.id ? faLocationCrosshairs : faPlus}
                            color={collectionItemInfoId === item.item.id ? 'green' : 'white'}
                        />
                    </div>
                    <div className={`${item.info !== null ? 'received' : 'not-received'}`}>
                        <div className="item">{item.item.name}</div>
                    </div>
                </div>
                {collectionItemInfoId === item.item.id &&
                    <>
                        <AccountCollectionItem
                            colelctionType={collectionType}
                            colelctionId={item.item.id}
                        />
                        {(item.info && item.info.type === WoWAccountCollectionType.PET) &&
                            <AccountPet
                                petInfo={item.info as WoWAccountPetInfoModel}
                            />
                        }
                    </>
                }
            </div>
        );
    }

    return (
        <div className="account-collection">
            <div className="account-collection__title">
                <h6>{t("Mounts")}:</h6>
                {onlyNotReceived
                    ? <h6 className="account-collection-count count">
                        <span>{collectionItems.length}</span>
                    </h6>
                    : <h6 className="account-collection-count count">
                        <span>{accountMountCount}</span>
                        <span>/</span>
                        <span>{collectionItems.length}</span>
                    </h6>
                }
            </div>
            <div className="account-collection-details">
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" value="" id="checkIndeterminate"
                        defaultChecked={onlyNotReceived}
                        onChange={() => setOnlyNotReceived(prev => !prev)} />
                    <label className="form-check-label" htmlFor="checkIndeterminate">{t("OnlyNotReceived")}</label>
                </div>
            </div>
            <LoadMoreCollections
                collection={collectionItems}
                getItem={getItem}
            />
        </div>
    );
}

export default AccountCollections;