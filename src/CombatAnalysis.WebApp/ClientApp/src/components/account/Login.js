import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { userUpdate } from '../../features/UserReducer';
import AccountService from '../../services/AccountService';

import "../../styles/account/login.scss";

const Login = () => {
    const accountService = new AccountService();

    const navigate = useNavigate();
    const { t, i18n } = useTranslation("login");
    const dispatch = useDispatch();

    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const email = useRef(null);
    const password = useRef(null);

    const loginAsync = async () => {
        setShowErrorMessage(false);

        const data = {
            email: email.current.value,
            password: password.current.value
        };

        const user = await accountService.loginAsync(data);
        dispatch(userUpdate(user));

        navigate('/');
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await loginAsync();
    }

    const render = () => {
        return (
            <form className="login" onSubmit={handleSubmitAsync}>
                <div className="mb-3">
                    <label htmlFor="inputEmail" className="form-label">{t("Email")}</label>
                    <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputPassword" className="form-label">{t("Password")}</label>
                    <input type="password" className="form-control" id="inputPassword" ref={password} />
                </div>
                <input type="submit" className="btn btn-primary" value="Login" />
                <div className="login__error-message" style={{ display: showErrorMessage ? "flex" : "none" }}>Incorrect email/password. Try again</div>
            </form>
        );
    }

    return render();
}

export default Login;