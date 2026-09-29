import WoWGameDataContext from '@/context/WoWGameDataContext';
import { useGetWoWTokenQuery } from '../api/WoWData.api';
import { useContext } from 'react';
import ResponseInformation from '@/shared/components/ResponseInformation';
import type { WoWTokenModel } from '../types/WoWTokenModel';
import useFormatting from '@/shared/hooks/useFormatting';

const WoWToken: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, regionName } = context;
    const { getDate } = useFormatting();
    
    const { data: token, isLoading, error } = useGetWoWTokenQuery({ regionName });

    const getPrice = (token: WoWTokenModel) => {
        const price = token.price;
        const onlyGoldPrice = price / 10000;

        return (
            <div>{t("Price")}: {onlyGoldPrice / 1000} {t("Gold")}</div>
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
            <div>{getPrice(token)}</div>
        </div>
    );
}

export default WoWToken;