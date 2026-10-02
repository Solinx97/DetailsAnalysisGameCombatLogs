import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useRef, useState } from 'react';
import { useSearchItemQuery } from '../../api/WoWData.api';
import WoWAuctionHouse from './WoWAuctionHouse';
import type { SearchItemModel } from '../../types/SearchItemModel';

const SearchItem: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;

    const itenNameRef = useRef<HTMLInputElement | null>(null);

    const [itemName, setItemName] = useState<string>("");
    const [selectedItemName, setSelectedItemName] = useState<string>("");
    const [selectedItemId, setSelectedItemId] = useState<number>(0);
    const [page, setPage] = useState<number>(1);
    const [showAuctionHouse, setShowAuctionHouse] = useState<boolean>(false);

    const { data: item, isLoading, error } = useSearchItemQuery({ name: itemName, orderBy: 'id', page, regionName },
        {
            skip: itemName.trim().length === 0
        }
    );

    const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            setItemName(event.currentTarget.value);
            setPage(1);
        }
    }

    const handleItemSelect = (item: SearchItemModel) => {
        setSelectedItemName(item.data.name);
        setSelectedItemId(item.data.id);
    }

    const getItemName = () => {
        return (
            <div className="search-item__search">
                <div className="search">
                    <div>{t("ItemName")}</div>
                    <input type="text" className="form-control" placeholder="Username" aria-label="Item name"
                        ref={itenNameRef}
                        onKeyDown={handleKeyDown}
                        defaultValue={itemName} />
                    <div className="search-item__selected">
                        <div>{t("Search")}:</div>
                        <h6>{itemName}</h6>
                    </div>
                    <div className="search-item__selected">
                        <div>{t("Selected")}:</div>
                        <h6>{selectedItemName}</h6>
                    </div>
                </div>
            </div>
        );
    }

    if (itemName.trim().length === 0) {
        return (getItemName());
    }

    if (!item || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!item || isLoading}
        />);
    }

    return (
        <div className="search-item">
            {getItemName()}
            <div className="search-item__content">
                <div>
                    <div>{t("Pages")}: {item.pageCount}</div>
                    <ul className="search-item__result">
                        {Array.from({ length: item.pageCount }).map((_, index) => (
                            <li className="special" onClick={() => setPage(index + 1)}>{index + 1}</li>
                        ))
                        }
                    </ul>
                    <h6>{t("ItemResults")} ({item.page})</h6>
                    <ul className="item-names">
                        {item.results.map(item => (
                            <li key={item.data.id} className="item-names__name">
                                <FontAwesomeIcon
                                    icon={faPlus}
                                    className="select"
                                    onClick={() => handleItemSelect(item)}
                                />
                                <div>{item.data.name}</div>
                            </li>
                        ))
                        }
                    </ul>
                </div>
                <div className="auction">
                    <div className="auction__name">
                        <div className="btn-shadow"
                            onClick={() => setShowAuctionHouse(prev => !prev)}>
                            <FontAwesomeIcon
                                icon={faLocationCrosshairs}
                            />
                            <div>{t("AuctionHouse")}</div>
                        </div>
                    </div>
                    {showAuctionHouse &&
                        <WoWAuctionHouse
                            itemId={selectedItemId}
                        />
                    }
                </div>
            </div>
        </div>
    );
}

export default SearchItem;