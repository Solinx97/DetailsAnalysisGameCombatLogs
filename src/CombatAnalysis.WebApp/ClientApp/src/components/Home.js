import { faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink, useLocation, useNavigate } from 'react-router-dom';
import { useLazyIdentityQuery } from '../store/api/UserApi';

import '../styles/home.scss';

const shouldBeAuthorizeTimeout = 5000;

const Home = () => {
    const { t } = useTranslation("home");

    const navigate = useNavigate();
    const location = useLocation();

    const customer = useSelector((state) => state.customer.value);

    const [identityAsyncQuery] = useLazyIdentityQuery();

    const [shouldBeAuthorize, setShouldBeAuthorize] = useState(false);

    useEffect(() => {
        const checkAuth = () => {
            const searchParams = new URLSearchParams(location.search);
            return searchParams.get("shouldBeAuthorize") !== null;
        }

        setShouldBeAuthorize(checkAuth());
    }, []);

    useEffect(() => {
        let timeoutId;

        if (shouldBeAuthorize) {
            timeoutId = setTimeout(() => {
                setShouldBeAuthorize(false);
            }, shouldBeAuthorizeTimeout);
        }

        return () => clearTimeout(timeoutId);
    }, [shouldBeAuthorize]);

    const loginAsync = async () => {
        const identityServerAuthPath = process.env.REACT_APP_IDENTITY_SERVER_AUTH_PATH;

        const response = await identityAsyncQuery(identityServerAuthPath);

        if (response.data !== undefined) {
            const uri = response.data.uri;
            window.location.href = uri;
        }
    }

    const navigateToFeed = () => navigate("/feed");
    const navigateToMainInformation = () => navigate("/main-information");

    return (
        <div className="home">
            <div className="home__item">
                <div className="title">
                    <div>{t("Communication")}</div>
                    {customer === null &&
                        <div className="btn-shadow authorize-alert" onClick={async () => await loginAsync()} title={t("GoToLogin")}>
                            <FontAwesomeIcon
                                icon={faTriangleExclamation}
                            />
                            <div>{t("ShouldAuthorize")}</div>
                        </div>
                    }
                </div>
                <div className="preview">
                    <div className="preview__title">{t("Communication")}</div>
                    <div className="preview__responsibilities">
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-1" id="communication-reason-1" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-1">{t("ExploreFeed")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-2" id="communication-reason-2" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-2">{t("ChattingWithFriends")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-3" id="communication-reason-3" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-3">{t("CreateJoinCommunity")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-4" id="communication-reason-4" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-4">{t("AddFriedns")}</label>
                        </div>
                    </div>
                </div>
                {customer !== null &&
                    <div className="go-to-communication" onClick={navigateToFeed}>{t("Open")}</div>
                }
            </div>
            <div className="home__item">
                <div className="title">{t("Analyzing")}</div>
                <div className="preview">
                    <div className="preview__title">{t("Analyzing")}</div>
                    <div className="preview__responsibilities">
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-1" id="combat-logs-reason-1" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-1">{t("SaveAnalyzing")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-2" id="combat-logs-reason-2" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-2">{t("ExploreAnalyzing")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-3" id="combat-logs-reason-3" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-3">{t("ShareAnalyzing")}</label>
                        </div>
                    </div>
                </div>
                <div className="go-to-combat-logs" onClick={navigateToMainInformation}>{t("Open")}</div>
            </div>
            {shouldBeAuthorize &&
                <div className="should-be-authorize">
                    <div className="alert alert-success" role="alert">
                        {t("YouNeed")} <NavLink onClick={async () => await loginAsync()}>{t("Login")}</NavLink> {t("InApp")}
                    </div>
                </div>
            }
        </div>
    );
}

export default Home;