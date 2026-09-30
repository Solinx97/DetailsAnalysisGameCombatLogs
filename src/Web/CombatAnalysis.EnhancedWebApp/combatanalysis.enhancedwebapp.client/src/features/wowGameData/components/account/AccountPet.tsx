import WoWGameDataContext from '@/context/WoWGameDataContext';
import { useContext } from 'react';
import type { WoWAccountPetInfoModel } from '../../types/collections/WoWAccountPetInfoModel';

const AccountPet: React.FC<{ petInfo: WoWAccountPetInfoModel }> = ({ petInfo }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;

    return (
        <div className="pet-info">
            <div className="pet-info__category">
                <div className="special special-name">{t("Level")}</div>
                <div className="special">{petInfo.level}</div>
                <div className="special">{petInfo.quality?.name}</div>
            </div>
            <div className="pet-info__category">
                <div className="special special-name">{t("Health")}</div>
                <div className="special">{petInfo.stats.health}</div>
                <div className="special special-name">{t("Power")}</div>
                <div className="special">{petInfo.stats.power}</div>
                <div className="special special-name">{t("Speed")}</div>
                <div className="special">{petInfo.stats.speed}</div>
            </div>
        </div>
    );
}

export default AccountPet;