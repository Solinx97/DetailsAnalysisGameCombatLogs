import React, { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { useAuth } from '../context/AuthProvider';
import useAuthorization from '../hooks/useAuthorization';
import LanguageSelector from './LanguageSelector';
import Search from './Search';

import '../styles/navMenu.scss';

const NavMenu = () => {
    const { t } = useTranslation("translate");

    const user = useSelector((state) => state.user.value);

    const { navigateToAuthorizationAsync, navigateToRegistrationAsync } = useAuthorization();
    const { isAuthenticated, checkAuthAsync, logoutAsync } = useAuth();

    const navigate = useNavigate();

    const [collapsed, setCollapsed] = useState(true);

    useEffect(() => {
        const checkAuth = async () => {
            try {
                await checkAuthAsync();
            } catch (error) {
                console.error("Authentication check failed:", error);
            }
        }

        checkAuth();
    }, []);

    const toggleNavbar = useCallback(() => {
        setCollapsed((item) => !item);
    }, []);

    const handleLoginClick = useCallback(async () => await navigateToAuthorizationAsync(), [navigate]);
    const handleRegistrationClick = useCallback(async () => await navigateToRegistrationAsync(), [navigate]);
    const handleLogoutClick = useCallback(() => logoutAsync(), [logoutAsync]);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <LanguageSelector />
                <Container>
                    <NavbarBrand
                        tag={Link}
                        to="/"
                    >
                        Wow Analysis
                    </NavbarBrand>
                    <NavbarToggler
                        onClick={toggleNavbar} className="mr-2"
                    />
                    {user !== null &&
                        <Search
                            me={user}
                        />
                    }
                    <Collapse
                        className="d-sm-inline-flex flex-sm-row-reverse"
                        isOpen={!collapsed}
                        navbar
                    />
                    {isAuthenticated
                        ? <div className="authorized">
                            <div className="username">{user?.username}</div>
                            <div className="authorized__logout" onClick={handleLogoutClick}>{t("Logout")}</div>
                          </div>
                        : <div className="authorization">
                            <div className="authorization__login" onClick={handleLoginClick}>{t("Login")}</div>
                            <div className="authorization__registration" onClick={handleRegistrationClick}>{t("Registration")}</div>
                        </div>
                    }
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
