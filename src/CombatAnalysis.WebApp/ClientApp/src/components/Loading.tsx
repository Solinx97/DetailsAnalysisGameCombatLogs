import { useTranslation } from 'react-i18next';

import "../styles/loading.scss";

const Loading: React.FC = () => {
    const { t } = useTranslation("translate");

    return (
        <div className="center">
            <div className="ring"></div>
            <span>{t("Loading")}</span>
        </div>
    );
}

export default Loading;