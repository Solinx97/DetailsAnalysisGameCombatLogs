import { useTranslation } from 'react-i18next';

import './Loading.scss';

const CombatLogsBuild: React.FC = () => {
    const { t } = useTranslation("translate");

    return (
        <div className="center">
            <div className="ring"></div>
            <span>{t("CombatPlayerBuild")}</span>
        </div>
    );
}

export default CombatLogsBuild;