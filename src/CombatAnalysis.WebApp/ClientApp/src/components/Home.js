import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const Home = () => {
    const { t } = useTranslation("home");

    const navigate = useNavigate();

    const customer = useSelector((state) => state.customer.value);

    return (
        <div>
            <div>
                <div>{t("Communication")} <span style={{ display: customer !== null ? "none" : "flex" }}>({t("ShouldAuthorize")})</span></div>
                <button className="btn btn-info" onClick={() => navigate("/feed")} disabled={customer == null}>{t("Open")}</button>
            </div>
            <div>
                <div>{t("Analyzing")}</div>
                <button className="btn btn-info" onClick={() => navigate("/main-information")}>{t("Open")}</button>
            </div>
        </div>
    );
}

export default Home;