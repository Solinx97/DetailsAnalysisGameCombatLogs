import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { useContext } from 'react';
import { useGetAuctionQuery } from '../../api/WoWData.api';
import LoadMoreAuctions from '../helpers/LoadMoreAuctions';

const WoWAuctionHouse: React.FC<{ itemId: number }> = ({ itemId }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;

    const { data: auctions, isLoading, isFetching, error } = useGetAuctionQuery({ regionName, itemId }, {
        skip: itemId === 0
    });

    console.log(isLoading);

    if (!auctions || isLoading || isFetching || error) {
        return (<ResponseInformation
            error={error}
            watchNumberParams={[itemId]}
            isLoading={!auctions || isLoading || isFetching}
        />);
    }

    return (
        <div className="auction">
            <h6>{t("AuctionHouse")} ({auctions.length})</h6>
            <LoadMoreAuctions
                auctions={auctions}
            />
        </div>
    );
}

export default WoWAuctionHouse;