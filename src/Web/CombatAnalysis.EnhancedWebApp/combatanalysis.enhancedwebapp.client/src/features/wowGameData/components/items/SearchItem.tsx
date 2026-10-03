import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import type { OptionNumberModel } from '@/shared/types/OptionNumberModel';
import { faCoins } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useRef, useState } from 'react';
import Select from 'react-select';
import { useGetItemClassesQuery, useGetItemSubClassesQuery, useSearchItemQuery } from '../../api/WoWData.api';
import WoWAuctionHouse from './WoWAuctionHouse';

const SearchItem: React.FC = () => {
    const defaultPageSize = 50;

    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;

    const itenNameRef = useRef<HTMLInputElement | null>(null);

    const [itemClassesOptions, setItemClassesOptions] = useState<OptionNumberModel[]>([]);
    const [itemClassValue, setItemClassValue] = useState<OptionNumberModel | null>(itemClassesOptions[0]);

    const [itemSubClassesOptions, setItemSubClassesOptions] = useState<OptionNumberModel[]>([]);
    const [itemSubClassValue, setItemSubClassValue] = useState<OptionNumberModel | null>(itemSubClassesOptions[0]);

    const [itemName, setItemName] = useState<string>("");
    const [selectedItemId, setSelectedItemId] = useState<number>(0);
    const [page, setPage] = useState<number>(1);

    const { data: item, isLoading, isFetching, error } = useSearchItemQuery({
        name: itemName,
        regionName,
        orderBy: 'id',
        itemClassId: itemClassValue ? itemClassValue.value : -1,
        itemSubClassId: itemSubClassValue ? itemSubClassValue.value : -1,
        page,
        pageSize: defaultPageSize
    },
        {
            skip: itemName.trim().length === 0
        }
    );
    const { data: itemClasses } = useGetItemClassesQuery({ regionName });
    const { data: itemSubClasses } = useGetItemSubClassesQuery({ regionName, itemClassId: itemClassValue ? itemClassValue.value : -1 },
        {
            skip: !itemClassValue || itemClassValue.value === -1
        }
    );

    useEffect(() => {
        if (!itemClasses || itemClasses.length === 0) {
            return;
        }

        const options = itemClasses.map(
            (item) => ({
                value: item.id,
                label: item.name ? item.name : ""
            })
        )

        options.unshift({ value: -1, label: t("All") });
        setItemClassesOptions(options);
    }, [itemClasses]);

    useEffect(() => {
        if (!itemSubClasses || itemSubClasses.length === 0) {
            return;
        }

        const options = itemSubClasses.map(
            (item) => ({
                value: item.id,
                label: item.name ? item.name : ""
            })
        )

        options.unshift({ value: -1, label: t("All") });
        setItemSubClassesOptions(options);
    }, [itemSubClasses]);

    useEffect(() => {
        if (!itemClassValue || itemClassValue.value === -1) {
            setItemSubClassesOptions([]);
        }
    }, [itemClassValue]);

    useEffect(() => {
        setPage(1);
    }, [itemClassValue, itemSubClassValue]);

    const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            setItemName(event.currentTarget.value);
            setPage(1);
        }
    }

    const getItemName = () => {
        return (
            <div className="search-item__search">
                <div className="search">
                    <div className="filters">
                        <div className="filter-item select-item">
                            <div>{t("ItemName")}</div>
                            <input type="text" className="form-control" placeholder="Item name" aria-label="ItemName"
                                ref={itenNameRef}
                                onKeyDown={handleKeyDown}
                                defaultValue={itemName} />
                        </div>
                        <div className="filter-item select-item-class">
                            <div>{t("ItemClass")}</div>
                            <Select<OptionNumberModel>
                                className="options"
                                options={itemClassesOptions}
                                value={itemClassValue}
                                onChange={(selected) => setItemClassValue(selected)}
                            />
                        </div>
                        <div className="filter-item select-item-class">
                            <div>{t("ItemSubClass")}</div>
                            <Select<OptionNumberModel>
                                className="options"
                                options={itemSubClassesOptions}
                                value={itemSubClassValue}
                                onChange={(selected) => setItemSubClassValue(selected)}
                            />
                        </div>
                    </div>
                    <div className="search-item__selected">
                        <div>{t("Search")}:</div>
                        <h6>{itemName}</h6>
                    </div>
                </div>
            </div>
        );
    }

    if (itemName.trim().length === 0) {
        return (getItemName());
    }

    if (!item || isLoading || isFetching || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!item || isLoading || isFetching}
        />);
    }

    return (
        <div className="search-item">
            {getItemName()}
            <div className="search-item__content">
                <div>{t("Count")}: {item.pageSize * item.pageCount}, {t("Pages")}: {item.pageCount}</div>
                {item.pageCount > 1 &&
                    <ul className="search-item__result">
                        {Array.from({ length: item.pageCount }).map((_, index) => (
                            <li key={index} className="special" onClick={() => setPage(index + 1)}>{index + 1}</li>
                        ))
                        }
                    </ul>
                }
                <h6>{t("ItemResults")} ({item.page})</h6>
                <ul className="item-names">
                    {item.results.map(item => (
                        <li key={item.data.id} className="item-names__name">
                            <div className="name">
                                <div className="details">
                                    <div className="category">
                                        <div className="special special-name">{t("iLvl")}</div>
                                    </div>
                                    <div className="category">
                                        <div className="special">{item.data.level}</div>
                                    </div>
                                    <div className="category">
                                        <div className="special">{item.data.itemClass.name}</div>
                                    </div>
                                    <div className="category">
                                        <div className="special">{item.data.itemSubclass.name}</div>
                                    </div>
                                    <div className="category">
                                        <div className="special">{item.data.quality.name}</div>
                                    </div>
                                </div>
                                <div>{item.data.name}</div>
                            </div>
                            <FontAwesomeIcon
                                icon={faCoins}
                                className="select"
                                color={selectedItemId === item.data.id ? 'green' : 'white'}
                                onClick={() => setSelectedItemId(prev => prev === item.data.id ? 0 : item.data.id)}
                            />
                            {selectedItemId === item.data.id &&
                                <WoWAuctionHouse
                                    itemId={selectedItemId}
                                />
                            }
                        </li>
                    ))
                    }
                </ul>
            </div>
        </div>
    );
}

export default SearchItem;