import WoWGameDataContext from '@/context/WoWGameDataContext';
import React, { useContext, useEffect, useState } from 'react';
import type { UserCollectionModel } from '../../types/UserCollectionModel';

const LoadMoreCollections: React.FC<{ collection: UserCollectionModel[], getItem(item: UserCollectionModel): React.ReactElement }> = ({ collection, getItem }) => {
    const pageSize = 50;

    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;

    const [size, setSize] = useState<number>(pageSize);
    const [filteredCollection, setFilteredCollection] = useState<UserCollectionModel[]>([]);

    useEffect(() => {
        setFilteredCollection(collection.slice(0, size));
    }, [collection, size]);

    return (
        <ul className="user-collection__container">
            {filteredCollection.map((item, index) => (
                <li key={index} className="user-collection__item">
                    {getItem(item)}
                </li>
            ))
            }
            {size < collection.length &&
                <li className="load-more" onClick={() => setSize(prev => prev + pageSize)}>{t("LoadMore")}</li>
            }
        </ul>
    );
}

export default LoadMoreCollections;