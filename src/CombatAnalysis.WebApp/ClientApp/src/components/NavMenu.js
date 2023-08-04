import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { useLogoutAsyncMutation } from '../store/api/Account.api';
import { useLazySearchByUserIdAsyncQuery } from '../store/api/Customer.api';
import { useAuthenticationAsyncQuery } from '../store/api/UserApi';
import { updateCustomer } from '../store/slicers/CustomerSlice';
import { updateUser } from '../store/slicers/UserSlice';

import '../styles/navMenu.scss';

const NavMenu = () => {
    const dispatch = useDispatch();
    const customer = useSelector((state) => state.customer.value);

    const auth = useAuthenticationAsyncQuery();

    const [getCustomerAsync] = useLazySearchByUserIdAsyncQuery();
    const [logoutAsyncMut] = useLogoutAsyncMutation();

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
        switch (auth.status) {
            case "rejected":
                dispatch(updateCustomer(null));
                navigate('/');
                break;
            case "fulfilled":
                const updateUserData = async () => {
                    await updateUserDataAsync();
                }

                updateUserData();
                break;
            default:
                dispatch(updateCustomer(null));
                break;
        }
    }, [auth])

    const updateUserDataAsync = async () => {
        dispatch(updateUser(auth.data));

        const customer = await getCustomerAsync(auth.data?.id);
        if (customer.data !== undefined) {
            dispatch(updateCustomer(customer.data));
        }
    }

    const changeLanguage = (language) => {
        i18n.changeLanguage(language);

        window.location.reload(true);
    }

    const logoutAsync = async () => {
        const res = await logoutAsyncMut();
        res.data !== undefined && navigate('/');
    }

    const toggleNavbar = () => {
        setCollapsed((item) => !item);
    }

    if (auth.isLoading) {
        return <></>;
    }

    return (
        <header>
            <Navbar
                className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
                light>
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
                    <NavbarBrand
                        tag={Link}
                        to="/"
                    >
                        Wow Analysis
                    </NavbarBrand>
                    <NavbarToggler
                        onClick={toggleNavbar} className="mr-2"
                    />
                    <Collapse
                        className="d-sm-inline-flex flex-sm-row-reverse"
                        isOpen={!collapsed}
                        navbar
                    />
                    {customer !== null
                        ? <div className="authorized">
                            <div>{t("Welcome")}, <strong>{customer?.username}</strong></div>
                            <button type="button" className="btn btn-primary" onClick={logoutAsync}>{t("Logout")}</button>
                        </div>
                        : <div className="authorization">
                            <button type="button" className="btn btn-primary" onClick={() => navigate('/registration')}>{t("Registration")}</button>
                            <button type="button" className="btn btn-primary" onClick={() => navigate('/login')}>{t("Login")}</button>
                        </div>
                    }
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
