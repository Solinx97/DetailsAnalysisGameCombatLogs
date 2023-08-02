import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import useAuthentificationAsync from '../hooks/useAuthentificationAsync';

const Home = () => {
    const navigate = useNavigate();

    const { t } = useTranslation("home");

    const [, , isAuth] = useAuthentificationAsync();

    return (
        <div>
            <div>
                <div>{t("Communication")} <span style={{ display: isAuth ? "none" : "flex" }}>({t("ShouldAuthorize")})</span></div>
                <button className="btn btn-info" onClick={() => navigate("/communication")} disabled={!isAuth}>{t("Open")}</button>
            </div>
            <div>
                <div>{t("Analyzing")}</div>
                <button className="btn btn-info" onClick={() => navigate("/main-information")}>{t("Open")}</button>
            </div>
        </div>
    );
}

export default Home;