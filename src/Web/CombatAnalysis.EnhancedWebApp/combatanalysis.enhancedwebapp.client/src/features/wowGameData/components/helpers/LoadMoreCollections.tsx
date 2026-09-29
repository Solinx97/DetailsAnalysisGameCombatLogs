import WoWGameDataContext from '@/context/WoWGameDataContext';
import React, { useContext, useEffect, useState } from 'react';
import type { WoWAccountCollectionItemModel } from '../../types/collections/WoWAccountCollectionItemModel';

const LoadMoreCollections: React.FC<{ collection: WoWAccountCollectionItemModel[], getItem(item: WoWAccountCollectionItemModel): React.ReactElement }> = ({ collection, getItem }) => {
    const pageSize = 50;

    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;

    const [size, setSize] = useState<number>(pageSize);
    const [filteredCollection, setFilteredCollection] = useState<WoWAccountCollectionItemModel[]>([]);

    useEffect(() => {
        setFilteredCollection(collection.slice(0, size));
    }, [collection, size]);

    return (
        <ul className="character-collection__container">
            {filteredCollection.map((item, index) => (
                <li key={index} className="character-collection__item">
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