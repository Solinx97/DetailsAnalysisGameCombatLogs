import { faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink, useLocation, useNavigate } from 'react-router-dom';

import '../styles/home.scss';

const shouldBeAuthorizeTimeout = 5000;

const Home = () => {
    const { t } = useTranslation("home");

    const navigate = useNavigate();
    const location = useLocation();

    const customer = useSelector((state) => state.customer.value);

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

    const generateCodeVerifier = () => {
        const array = new Uint8Array(32);
        window.crypto.getRandomValues(array);

        const codeVerifier = base64UrlEncode(array);
        return codeVerifier;
    }

    const generateCodeChallengeAsync = async (verifier) => {
        const buffer = await crypto.subtle.digest("SHA-256", new TextEncoder().encode(verifier));
        const codeChalenge = base64UrlEncode(buffer);

        return codeChalenge
    }

    const base64UrlEncode = (buffer) => {
        const encodedCode = btoa(String.fromCharCode.apply(null, new Uint8Array(buffer)))
            .replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');

        return encodedCode;
    }

    const naviagetToLoginAsync = async () => {
        const codeVerifier = generateCodeVerifier();
        const codeChallenge = await generateCodeChallengeAsync(codeVerifier);

        window.location.href = `https://localhost:7064/login?grantType=code&clientTd=clientId&redirectUri=https://localhost:44479/callback&scope=api1&codeChallengeMethod=S256&codeChallenge=${codeChallenge}`;
    }

    const naviagetToFeed = () => navigate("/feed");
    const naviagetToMainInformation = () => navigate("/main-information");

    return (
        <div className="home">
            <div className="home__item">
                <div className="title">
                    <div>{t("Communication")}</div>
                    {customer === null &&
                        <div className="btn-shadow authorize-alert" onClick={async () => await naviagetToLoginAsync()} title={t("GoToLogin")}>
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
                    <div className="go-to-communication" onClick={naviagetToFeed}>{t("Open")}</div>
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
                <div className="go-to-combat-logs" onClick={naviagetToMainInformation}>{t("Open")}</div>
            </div>
            {shouldBeAuthorize &&
                <div className="should-be-authorize">
                    <div className="alert alert-success" role="alert">
                        {t("YouNeed")} <NavLink to="/login">{t("Login")}</NavLink> {t("InApp")}
                    </div>
                </div>
            }
        </div>
    );
}

export default Home;