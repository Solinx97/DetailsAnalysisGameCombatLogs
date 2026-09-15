import { memo, useEffect, useState, type JSX } from 'react';
import { useTranslation } from 'react-i18next';
import useCombatDetailsData from '../hooks/useCombatDetailsData';

import './CombatMoreDetails.scss';

interface CombatMoreDetailsProps {
    unitId: string;
    detailsType: number;
}

const CombatMoreDetails: React.FC<CombatMoreDetailsProps> = ({ unitId, detailsType }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const pageSize = 50;

    const [detailsDataRender, setDetailsDataRender] = useState<JSX.Element>(<></>);

    const { getComponentByDetailsTypeAsync } = useCombatDetailsData(unitId, pageSize, detailsType, t);

    useEffect(() => {
        const getHelperComponent = async () => {
            await getHelperComponentAsync();
        }

        getHelperComponent();
    }, []);

    const getHelperComponentAsync = async (): Promise<void> => {
        const component = await getComponentByDetailsTypeAsync();
        setDetailsDataRender(component);
    }

    return (
        <div className="details__container">
            {detailsDataRender}
        </div>
    );
}

export default memo(CombatMoreDetails);