import WoWGameDataContext from '@/context/WoWGameDataContext';
import React, { useContext, useEffect, useState } from 'react';
import type { AuctionModel } from '../../types/auction/AuctionModel';

const LoadMoreAuctions: React.FC<{ auctions: AuctionModel[] }> = ({ auctions }) => {
    const pageSize = 50;

    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;

    const [size, setSize] = useState<number>(pageSize);
    const [filteredAuctions, setFilteredAuctions] = useState<AuctionModel[]>([]);

    useEffect(() => {
        setFilteredAuctions(auctions.slice(0, size));
    }, [auctions, size]);

    const getPrice = (price: number) => {
        const priceStr = price.toString();

        return (
            <div>{t("Price")}: {priceStr.slice(0, -4)}g {priceStr.slice(-4, -2)}s {priceStr.slice(-2)}c</div>
        );
    }

    return (
        <ul>
            {filteredAuctions.map(auction => (
                <li key={auction.id} className="auction__item">
                    <div>{getPrice(auction.unitPrice)}</div>
                    <div className="auction-count">
                        <div>{t("Count")}:</div>
                        <div>{auction.quantity}</div>
                    </div>
                </li>
            ))
            }
            {size < auctions.length &&
                <li className="load-more" onClick={() => setSize(prev => prev + pageSize)}>{t("LoadMore")}</li>
            }
        </ul>
    );
}

export default LoadMoreAuctions;