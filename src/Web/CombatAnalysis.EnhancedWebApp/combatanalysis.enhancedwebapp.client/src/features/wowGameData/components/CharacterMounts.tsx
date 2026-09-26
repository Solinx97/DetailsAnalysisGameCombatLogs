import WoWGameDataContext from '@/context/WoWGameDataContext';
import { useContext, useEffect, useState } from 'react';
import { useGetUserMountsQuery } from '../api/WoWUser.api';
import type { WoWMountModel } from '../types/WoWMountModel';
import LoadMoreCollections from './helpers/LoadMoreCollections';
import { faLocationCrosshairs, faPlus, faQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { UserCollectionModel } from '../types/UserCollectionModel';
import { useLazyGetMountQuery } from '../api/WoWData.api';
import type { SelectedMountModel } from '../types/collections/SelectedMountModel';

const CharacterMounts: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;

    const [mounts, setMounts] = useState<WoWMountModel[]>([]);
    const [mount, setMount] = useState<SelectedMountModel | undefined>();
    const [accountMountCount, setAccountMountCount] = useState<number>(0);
    const [onlyNotReceived, setOnlyNotReceived] = useState<boolean>(false);

    const { data: allMounts, isLoading } = useGetUserMountsQuery({ regionName });
    
    const [getMount] = useLazyGetMountQuery();

    useEffect(() => {
        if (!allMounts) {
            return;
        }

        setMounts(allMounts);

        setAccountMountCount(allMounts.filter(x => x.isReceived).length);
    }, [allMounts]);

    useEffect(() => {
        if (!allMounts) {
            return;
        }

        if (onlyNotReceived) {
            setMounts(allMounts.filter(x => !x.isReceived));
        }
        else {
            setMounts(allMounts);
        }
    }, [onlyNotReceived, allMounts]);

    const getMountAsync = async (mountId: number) => {
        try {
            if (mount && mount.id === mountId) {
                setMount(undefined);
                return;
            }

            const receivedMount = await getMount({ mountId, regionName }).unwrap();
            setMount(receivedMount);
        } catch (error) {
            console.error("Failed to fetch mount by id:", error);
        }
    }

    const getMountInfo = (item: UserCollectionModel) => {
        if (!mount || mount.id !== item.id) {
            return (<></>);
        }

        return (
            <div className="details">
                <div>{mount.description}</div>
                <div className="how-receive">
                    <div className="btn-shadow">
                        <FontAwesomeIcon
                            icon={faQuestion}
                        />
                        <div>{mount.source?.name}</div>
                    </div>
                </div>
            </div>
        );
    }

    if (!mounts || isLoading) {
        return (<div>Loading...</div>);
    }

    const getItem = (item: UserCollectionModel) => {
        return (
            <div className="selected-user-collection">
                <div className="selected-user-collection__name">
                    <div className="action btn-shadow"
                        onClick={() => getMountAsync(item.id)}>
                        <FontAwesomeIcon
                            icon={mount && mount.id === item.id ? faLocationCrosshairs : faPlus}
                            color={mount && mount.id === item.id ? 'green' : 'white'}
                        />
                    </div>
                    <div className={`${item.isReceived ? 'received' : 'not-received'}`}>
                        <div className="item">{item.name}</div>
                    </div>
                </div>
                {getMountInfo(item)}
            </div>
        );
    }

    return (
        <div className="user-collection">
            <div className="user-collection__title">
                <h6>{t("Mounts")}:</h6>
                <h6 className="user-collection-count count">
                    <span>{accountMountCount}</span>
                    <span>/</span>
                    <span>{mounts.length}</span>
                </h6>
            </div>
            <div className="user-collection-details">
                <h6 className="count">{mounts.length - mounts.length}</h6>
                <div className="form-check">
                    <input className="form-check-input" type="checkbox" value="" id="checkIndeterminate"
                        defaultChecked={onlyNotReceived}
                        onChange={() => setOnlyNotReceived(prev => !prev)} />
                    <label className="form-check-label" htmlFor="checkIndeterminate">{t("OnlyNotReceived")}</label>
                </div>
            </div>
            <LoadMoreCollections
                collection={mounts}
                getItem={getItem}
            />
        </div>
    );
}

export default CharacterMounts;