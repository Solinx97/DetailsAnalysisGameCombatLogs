import React, { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../context/AuthProvider';
import LoginPreview from './LoginPreview';

import "../../styles/account/login.scss";

const Login = () => {
    const { t } = useTranslation("account/login");

    const { loginAsync } = useAuth();

    const email = useRef(null);
    const password = useRef(null);
    const dontLogout = useRef(null);

    const handleSubmitAsync = async (event) => {
        document.cookie = `dontLogout=${dontLogout.current.checked}`;
        event.preventDefault();

        await loginAsync(email.current.value, password.current.value);
    }

    return (
        <div className="login">
            <div className="login__title">Login</div>
            <div className="login__container">
                <LoginPreview />
                <form className="login__submit" onSubmit={handleSubmitAsync}>
                    <div className="mb-3">
                        <label htmlFor="inputEmail" className="form-label">{t("Email")}</label>
                        <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputPassword" className="form-label">{t("Password")}</label>
                        <input type="password" className="form-control" id="inputPassword" ref={password} />
                    </div>
                    <div className="form-check remember">
                        <label className="form-check-label" htmlFor="invalidCheck">{t("RememberMe")}</label>
                        <input className="form-check-input" type="checkbox" id="invalidCheck" ref={dontLogout} />
                    </div>
                    <input type="submit" className="btn-border-shadow finish-login" value={t("Login")} />
                </form>
            </div>
        </div>
    );
}

export default Login;