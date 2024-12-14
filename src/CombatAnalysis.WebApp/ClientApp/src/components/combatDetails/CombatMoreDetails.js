import React, { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import useCombatDetailsData from '../../hooks/useCombatDetailsData';

import "../../styles/combatMoreDetails.scss";

const CombatMoreDetails = ({ combatPlayerId, detailsType }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const pageSize = 20;

    const [detailsDataRender, setDetailsDataRender] = useState(<></>);

    const { getComponentByDetailsTypeAsync } = useCombatDetailsData(combatPlayerId, pageSize, detailsType, t);

    useEffect(() => {
        const getHelperComponent = async () => {
            await getHelperComponentAsync();
        }

        getHelperComponent();
    }, []);

    const getHelperComponentAsync = async () => {
        const component = await getComponentByDetailsTypeAsync(detailsType);
        setDetailsDataRender(component);
    }

    return (
        <div className="details__container">
            {detailsDataRender}
        </div>
    );
}

export default memo(CombatMoreDetails);