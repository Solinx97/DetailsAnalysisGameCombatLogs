import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const Home = () => {
    const navigate = useNavigate();

    const { t } = useTranslation("home");

    const customer = useSelector((state) => state.customer.value);

    return (
        <div>
            <div>
                <div>{t("Communication")} <span style={{ display: customer !== null ? "none" : "flex" }}>({t("ShouldAuthorize")})</span></div>
                <button className="btn btn-info" onClick={() => navigate("/communication")} disabled={customer == null}>{t("Open")}</button>
            </div>
            <div>
                <div>{t("Analyzing")}</div>
                <button className="btn btn-info" onClick={() => navigate("/main-information")}>{t("Open")}</button>
            </div>
        </div>
    );
}

export default Home;