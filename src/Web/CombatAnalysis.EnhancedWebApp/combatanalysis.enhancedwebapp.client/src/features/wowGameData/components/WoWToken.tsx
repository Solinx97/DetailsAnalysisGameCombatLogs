import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import useFormatting from '@/shared/hooks/useFormatting';
import { useContext } from 'react';
import { useGetWoWTokenQuery } from '../api/WoWData.api';

const WoWToken: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;
    const { getDate } = useFormatting();
    
    const { data: token, isLoading, error } = useGetWoWTokenQuery({ regionName });

    const getPrice = (price: number) => {
        const onlyGoldPrice = price / 10000;

        return (
            <div>{t("Price")}: {onlyGoldPrice / 1000}g {price.toString().slice(-4, -2)}s {price.toString().slice(-2)}c</div>
        );
    }

    if (!token || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!token || isLoading}
        />);
    }

    return (
        <div>
            <h6>{t("WoWToken")} ({regionName}):</h6>
            <div>{t("LastUpdatedAt")}: {getDate(token.lastUpdatedTime)}</div>
            <div>{getPrice(token.price)}</div>
        </div>
    );
}

export default WoWToken;