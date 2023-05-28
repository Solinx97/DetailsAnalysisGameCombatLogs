import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

import "../../styles/account/login.scss";

const Login = () => {
    const navigate = useNavigate();
    const { t, i18n } = useTranslation("login");

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [showErrorMessage, setShowErrorMessage] = useState(false);

    const loginAsync = async () => {
        setShowErrorMessage(false);

        const data = {
            email: email,
            password: password
        };

        const response = await fetch('api/v1/Account', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.status === 404) {
            setShowErrorMessage(true);
        }
        else {
            navigate('/');
        }
    }

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
    }

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await loginAsync();
    }

    const render = () => {
        return (<form className="login" onSubmit={handleSubmitAsync}>
            <div className="mb-3">
                <label htmlFor="exampleInputEmail1" className="form-label">{t("Email")}</label>
                <input type="email" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" onChange={handleEmailChange} />
            </div>
            <div className="mb-3">
                <label htmlFor="exampleInputPassword1" className="form-label">{t("Password")}</label>
                <input type="password" className="form-control" id="exampleInputPassword1" onChange={handlePasswordChange} />
            </div>
            <input type="submit" className="btn btn-primary" value="Login" />
            <div className="login__error-message" style={{ display: showErrorMessage ? "flex" : "none" }}>Incorrect email/password. Try again</div>
        </form>);
    }

    return render();
}

export default Login;