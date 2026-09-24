import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import { useLazyAuthorizationQuery } from '@/features/user/api/User.api';
import logger from '@/utils/Logger';
import { faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

import './Home.scss';

const shouldBeAuthorizeTimeout = 10000;

const Home: React.FC = () => {
    const { t } = useTranslation('home');

    const navigate = useNavigate();
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);

    const myself = useSelector((state: RootState) => state.user.value);

    const auth = useAuth();

    const [authorization] = useLazyAuthorizationQuery();

    const [shouldBeAuthorize, setShouldBeAuthorize] = useState(false);

    useEffect(() => {
        (async () => {
            const shouldBeAuthorize = searchParams.get("shouldBeAuthorize") !== null;
            if (!shouldBeAuthorize) {
                return;
            }

            setShouldBeAuthorize(!auth?.isAuthenticated);
            if (auth?.isAuthenticated) {
                navigate("/");
            }
        })();
    }, [auth?.isAuthenticated]);

    useEffect(() => {
        let timeoutId: NodeJS.Timeout;

        if (shouldBeAuthorize) {
            timeoutId = setTimeout(() => {
                setShouldBeAuthorize(false);
            }, shouldBeAuthorizeTimeout);
        }

        return () => clearTimeout(timeoutId);
    }, [shouldBeAuthorize]);

    const loginHandle = async () => {
        try {
            const identityRedirect = await authorization(APP_CONFIG.identity.loginPath).unwrap();

            const uri = identityRedirect.uri;
            window.location.href = uri;
        } catch (e) {
            logger.error("Failed to redirect to the Login page", e);
        }
    }

    const navigateToFeed = () => navigate("/feed");
    const navigateToGameCombatLogs = () => navigate("/game-combat-logs");
    const navigateToWoWGameData = () => navigate("/wow-game-data");

    return (
        <div className="home">
            <div className="home__item">
                <div className="title">
                    <div>{t("Communication")}</div>
                    {!myself &&
                        <div className="btn-shadow authorize-alert" onClick={loginHandle} title={t("GoToLogin") || ""}>
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
                {myself !== null &&
                    <div className="go-to-communication" data-testid="go-to-communication" onClick={navigateToFeed}>{t("Open")}</div>
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
                <div className="go-to-combat-logs" data-testid="go-to-combat-logs" onClick={navigateToGameCombatLogs}>{t("Open")}</div>
            </div>
            <div className="home__item">
                <div className="title">{t("WoWExplorer")}</div>
                <div className="preview">
                    <div className="preview__title">{t("WoWExplorer")}</div>
                    <div className="preview__responsibilities">
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-1" id="combat-logs-reason-1" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-1">{t("ExplorerCollections")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-2" id="combat-logs-reason-2" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-2">{t("ExplorerCharacterGear")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-3" id="combat-logs-reason-3" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-3">{t("ExplorerCharacterReputations")}</label>
                        </div>
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" name="combat-logs-reason-3" id="combat-logs-reason-3" defaultChecked disabled />
                            <label className="form-check-label" htmlFor="combat-logs-reason-3">{t("OtherExplorer")}</label>
                        </div>
                    </div>
                </div>
                <div className="go-to-combat-logs" data-testid="go-to-combat-logs" onClick={navigateToWoWGameData}>{t("Open")}</div>
            </div>
            {shouldBeAuthorize &&
                <div className="should-be-authorize" data-testid="should-be-authorize">
                    <div className="alert alert-success" role="alert">
                        {t("YouNeed")} <span onClick={loginHandle}>{t("Login")}</span> {t("InApp")}
                    </div>
                </div>
            }
        </div>
    );
}

export default Home;