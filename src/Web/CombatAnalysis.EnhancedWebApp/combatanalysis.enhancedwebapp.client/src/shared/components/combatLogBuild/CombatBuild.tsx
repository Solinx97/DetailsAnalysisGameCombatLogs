import { useTranslation } from 'react-i18next';

import '../Loading.scss';

const CombatBuild: React.FC = () => {
    const { t } = useTranslation("translate");

    return (
        <div className="center">
            <div className="ring"></div>
            <span>{t("CombatBuild")}</span>
        </div>
    );
}

export default CombatBuild;