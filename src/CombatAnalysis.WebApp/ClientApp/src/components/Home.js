import { faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink, useNavigate } from 'react-router-dom';

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
    }, []);

    useEffect(() => {
        if (!shouldBeAuthorize) {
            return;
        }

        setTimeout(() => {
            setShouldBeAuthorize(false);
        }, shouldBeAutorizeTimeout)
    }, [shouldBeAuthorize]);

    return (
        <div className="home">
            <div className="home__item">
                <div className="title">
                    <div>{t("Communication")}</div>
                    {customer === null &&
                        <div className="btn-shadow authorize-alert" onClick={() => navigate("/login")} title={t("GoToLogin")}>
                            <FontAwesomeIcon
                                icon={faTriangleExclamation}
                            />
                            <div>{t("ShouldAuthorize")}</div>
                        </div>
                    }
                </div>
                <div className="preview">
                    <div className="preview__title">Communication</div>
                    <div className="preview__responsibilities">
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-1" id="communication-reason-1" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-1">Explore your feed</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-2" id="communication-reason-2" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-2">Chatting with friends in chats</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-3" id="communication-reason-3" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-3">Create or join to community</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="communication-reason-4" id="communication-reason-4" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="communication-reason-4">Add users in friends and see friends activities</label>
                        </div>
                    </div>
                </div>
                {customer !== null &&
                    <div className="go-to-communication" onClick={() => navigate("/feed")}>{t("Open")}</div>
                }
            </div>
            <div className="home__item">
                <div className="title">{t("Analyzing")}</div>
                <div className="preview">
                    <div className="preview__title">Combat logs</div>
                    <div className="preview__responsibilities">
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-1" id="combat-logs-reason-1" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-1">Save Combat logs</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-2" id="combat-logs-reason-2" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-2">Explore your or other users Combat logs</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-3" id="combat-logs-reason-3" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-3">Share link to Combat logs with friends</label>
                        </div>
                    </div>
                </div>
                <div className="go-to-combat-logs" onClick={() => navigate("/main-information")}>{t("Open")}</div>
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