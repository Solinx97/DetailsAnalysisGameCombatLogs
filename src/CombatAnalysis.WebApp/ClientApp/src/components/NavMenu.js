import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { checkAuth } from '../features/AuthenticationReducer';
import { userUpdate } from '../features/UserReducer';
import { customerUpdate } from '../features/CustomerReducer';

import '../styles/navMenu.scss';

const NavMenu = () => {
    const isAuth = useSelector((state) => state.authentication.value);
    const user = useSelector((state) => state.user.value);
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const { t, i18n } = useTranslation("translate");

    const [languageName, setLanguageName] = useState("English");
    const [collapsed, setCollapsed] = useState(true);

    useEffect(() => {
        switch (i18n.language) {
            case "ru":
                setLanguageName(t("RU"));
                break;
            case "en":
                setLanguageName(t("EN"));
                break;
            default:
                setLanguageName(t("EN"));
                break;
        }
    }, [])

    useEffect(() => {
        if (user !== null) {
            return;
        }

        const checkAuthResult = async () => {
            await checkAuthAsync();
        }

        checkAuthResult().catch(console.error);
    });

    const changeLanguage = (language) => {
        i18n.changeLanguage(language);

        window.location.reload(true);
    }

    const checkAuthAsync = async () => {
        const response = await fetch("api/v1/Authentication");

        if (response.status === 200) {
            const currentUser = await response.json();

            dispatch(userUpdate(currentUser));
            await getCustomerByUserIdAsync(currentUser.id);
        }
        else {
            dispatch(checkAuth(false));
        }
    }

    const getCustomerByUserIdAsync = async (userId) => {
        const response = await fetch(`api/v1/Customer/searchByUserId/${userId}`);

        if (response.status === 200) {
            const customer = await response.json();

            dispatch(customerUpdate(customer));
            dispatch(checkAuth(true));
        }
        else {
            dispatch(checkAuth(false));
        }
    }

    const logoutAsync = async () => {
        const response = await fetch("api/v1/Account/logout", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.status === 200) {
            dispatch(checkAuth(false));
            dispatch(userUpdate(null));

            navigate("/");
        }
    }

    const toggleNavbar = () => {
        setCollapsed(!collapsed);
    }

    const render = () => {
        return (<header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <div className="language dropdown">
                    <div className="language__title">{t("Langugae")}</div>
                    <button className="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                        {languageName}
                    </button>
                    <ul className="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                        <li><div className="dropdown-item" onClick={() => changeLanguage("ru")}>{t("RU")}</div></li>
                        <li><div className="dropdown-item" onClick={() => changeLanguage("en")}>{t("EN")}</div></li>
                    </ul>
                </div>
                <Container>
                    <NavbarBrand tag={Link} to="/">Wow Analysis</NavbarBrand>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar />
                    {!isAuth
                        ? <div className="authorization">
                            <button type="button" className="btn btn-primary" onClick={() => navigate('/registration')}>{t("Registration")}</button>
                            <button type="button" className="btn btn-primary" onClick={() => navigate('/login')}>{t("Login")}</button>
                        </div>
                        : <div className="authorized">
                            <div>{t("Welcome")}, <strong>{user.email}</strong></div>
                            <button type="button" className="btn btn-primary" onClick={logoutAsync}>{t("Logout")}</button>
                          </div>
                    }
                </Container>
            </Navbar>
        </header>);
    }

  return render();
}

export default NavMenu;
