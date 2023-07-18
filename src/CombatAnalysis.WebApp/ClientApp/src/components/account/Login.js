import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLoginAsyncMutation } from '../../store/api/Account.api';

import "../../styles/account/login.scss";

const Login = () => {
    const [ loginAsync ] = useLoginAsyncMutation();

    const navigate = useNavigate();
    const { t, i18n } = useTranslation("login");

    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const email = useRef(null);
    const password = useRef(null);

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        setShowErrorMessage(false);

        const data = {
            email: email.current.value,
            password: password.current.value
        };

        const user = await loginAsync(data);
        if (user.error !== undefined) {
            setShowErrorMessage(true);
        }
        else {
            navigate('/');
        }
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