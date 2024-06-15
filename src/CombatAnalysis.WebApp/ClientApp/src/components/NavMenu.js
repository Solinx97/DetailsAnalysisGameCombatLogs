import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { useLogoutAsyncMutation } from '../store/api/Account.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/Customer.api';
import { useLazyAuthenticationAsyncQuery } from '../store/api/UserApi';
import { updateCustomer } from '../store/slicers/CustomerSlice';
import { updateUser } from '../store/slicers/UserSlice';
import Search from './Search';

import '../styles/navMenu.scss';

const supportedLanguages = [
    "EN",
    "RU"
];

const NavMenu = () => {
    const { t, i18n } = useTranslation("translate");

    const dispatch = useDispatch();
    const customer = useSelector((state) => state.customer.value);

    const [getAuthAsync] = useLazyAuthenticationAsyncQuery();

    const [getCustomerAsync] = useLazySearchByUserIdAsyncQuery();
    const [logoutAsyncMut] = useLogoutAsyncMutation();

    const navigate = useNavigate();

    const [collapsed, setCollapsed] = useState(true);

    const selectedLang = i18n.language;

    useEffect(() => {
        const checkAuth = async () => {
            await checkAuthAsync();
        }

        checkAuth();

        console.log(i18n);
    }, [])

    const checkAuthAsync = async () => {
        const auth = await getAuthAsync();
        if (auth.error !== undefined) {
            dispatch(updateCustomer(null));
            return;
        }

        await getUserDataAsync(auth.data);
    }

    const getUserDataAsync = async (user) => {
        dispatch(updateUser(user));

        const customer = await getCustomerAsync(user?.id);
        if (customer.data !== undefined) {
            dispatch(updateCustomer(customer.data));
        }
    }

    const changeLanguage = (lang) => {
        i18n.changeLanguage(lang);

        window.location.reload(true);
    }

    const logoutAsync = async () => {
        dispatch(updateCustomer(null));
        navigate("/");

        await logoutAsyncMut();
    }

    const toggleNavbar = () => {
        setCollapsed((item) => !item);
    }

    return (
        <header>
            <Navbar
                className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
                light>
                <div className="language dropdown">
                    <ul className="language__select">
                        {supportedLanguages.map((lang, index) => (
                            <li key={index} className={`${lang === selectedLang ? 'selected-lang' : ''}`}
                                onClick={() => changeLanguage(lang)}>{lang}</li>
                        ))}
                    </ul>
                </div>
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
                    {customer !== null &&
                        <Search
                            me={customer}
                        />
                    }
                    <Collapse
                        className="d-sm-inline-flex flex-sm-row-reverse"
                        isOpen={!collapsed}
                        navbar
                    />
                    {customer !== null
                        ? <div className="authorized">
                            <div className="username">{customer?.username}</div>
                            <button type="button" className="btn btn-primary" onClick={logoutAsync}>{t("Logout")}</button>
                          </div>
                        : <div className="authorization">
                            <div className="authorization__login" onClick={() => navigate('/login')}>{t("Login")}</div>
                            <div className="authorization__registration" onClick={() => navigate('/registration')}>{t("Registration")}</div>
                        </div>
                    }
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
