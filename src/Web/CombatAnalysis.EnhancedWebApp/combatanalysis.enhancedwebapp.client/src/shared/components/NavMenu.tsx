import type { RootState } from '@/app/Store';
import { APP_CONFIG } from "@/config/appConfig";
import Notification from '@/features/notification/components/Notification';
import { useLazyAuthorizationQuery, useLazyRegistrationQuery } from '@/features/user/api/User.api';
import logger from '@/utils/Logger';
import React, { useCallback, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import { useAuth } from '../hooks/useAuth';
import LanguageSelector from './LanguageSelector';
import Search from './Search';

import './NavMenu.scss';

const NavMenu: React.FC = () => {
    const { t } = useTranslation('translate');

    const me = useSelector((state: RootState) => state.user.value);

    const auth = useAuth();

    const [authorization] = useLazyAuthorizationQuery();
    const [registration] = useLazyRegistrationQuery();

    const navigate = useNavigate();

    useEffect(() => {
        const checkAuth = async () => {
            try {
                await auth?.checkAuthAsync();
            } catch (error) {
                logger.error("Authentication check failed", error);
            }
        }

        checkAuth();
    }, []);

    const loginAsync = async () => {
        const identityServerAuthPath = APP_CONFIG.identity.loginPath;

        try {
            const authUri = await authorization(identityServerAuthPath).unwrap();

            window.location.href = authUri.uri;
        } catch (e) {
            logger.error("Failed to redirect to Identity server", e);
        }
    }

    const registrationAsync = async () => {
        const identityServerRegistrationPath = APP_CONFIG.identity.registryPath;

        try {
            const registryUri = await registration(identityServerRegistrationPath).unwrap();

            window.location.href = registryUri.uri;
        } catch (e) {
            logger.error("Failed to redirect to Identity server", e);
        }
    }

    const handleLoginClick = useCallback(async () => await loginAsync(), [navigate]);
    const handleRegistrationClick = useCallback(async () => await registrationAsync(), [navigate]);
    const handleLogoutClick = useCallback(() => auth?.logoutAsync(), [auth]);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <div className="brand-container">
                        <LanguageSelector />
                        <div className="brand">
                            <NavbarBrand
                                tag={Link}
                                to="/"
                            >
                                Wow Analysis
                            </NavbarBrand>
                        </div>
                    </div>
                    {auth?.authInProgress
                        ? <div>{t("LoginInProgress")}</div>
                        : <div className="main-elements">
                            {auth?.isAuthenticated
                                ? <div className="authorized">
                                    <Search
                                        t={t}
                                    />
                                    <div className="information">
                                        <Notification />
                                        <div className="username">{me?.username}</div>
                                        <div className="authorized__logout" onClick={handleLogoutClick}>{t("Logout")}</div>
                                    </div>
                                </div>
                                : <div className="authorization">
                                    <div className="authorization__login" onClick={handleLoginClick}>{t("Login")}</div>
                                    <div className="authorization__registration" onClick={handleRegistrationClick}>{t("Registration")}</div>
                                </div>
                            }
                        </div>
                    }
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
