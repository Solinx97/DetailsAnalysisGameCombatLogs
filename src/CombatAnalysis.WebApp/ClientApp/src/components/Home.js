import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';

import '../styles/home.scss';

const shouldBeAutorizeTimeout = 5000;

const Home = () => {
    const { t } = useTranslation("home");

    const navigate = useNavigate();

    const customer = useSelector((state) => state.customer.value);

    const [shouldBeAuthorize, setShouldBeAuthorize] = useState(false);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setShouldBeAuthorize(queryParams.get("shouldBeAuthorize") !== null);
    }, [])

    useEffect(() => {
        if (!shouldBeAuthorize) {
            return;
        }

        setTimeout(() => {
            setShouldBeAuthorize(false);
        }, shouldBeAutorizeTimeout)
    }, [shouldBeAuthorize])

    return (
        <div className="home">
            <div>
                <div>{t("Communication")} <span style={{ display: customer !== null ? "none" : "flex" }}>({t("ShouldAuthorize")})</span></div>
                <button className="btn btn-info" onClick={() => navigate("/feed")} disabled={customer == null}>{t("Open")}</button>
            </div>
            <div>
                <div>{t("Analyzing")}</div>
                <button className="btn btn-info" onClick={() => navigate("/main-information")}>{t("Open")}</button>
            </div>
            {shouldBeAuthorize &&
                <div className="should-be-authorize">
                    <div className="alert alert-success" role="alert">
                        You need <NavLink to="/login">Login</NavLink> in application to continue use "Communication"
                    </div>
                </div>
            }
        </div>
    );
}

export default Home;