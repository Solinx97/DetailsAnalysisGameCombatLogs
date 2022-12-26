import React, { useState, } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const Registration = () => {
    const navigate = useNavigate();
    const { t, i18n } = useTranslation("registration");

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const registrationAsync = async () => {
        const data = {
            email: email,
            password: password
        };

        const response = await fetch('api/v1/Account/registration', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        const statusCode = await response.status;
        if (statusCode === 200) {
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

        await registrationAsync();
    }

    const render = () => {
        return (<form onSubmit={handleSubmitAsync}>
            <div className="mb-3">
                <label htmlFor="exampleInputEmail1" className="form-label">{t("Email")}</label>
                <input type="email" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" onChange={handleEmailChange} />
            </div>
            <div className="mb-3">
                <label htmlFor="exampleInputPassword1" className="form-label">{t("Password")}</label>
                <input type="password" className="form-control" id="exampleInputPassword1" onChange={handlePasswordChange} />
            </div>
            <div className="mb-3 form-check">
                <input type="checkbox" className="form-check-input" id="exampleCheck1" />
                <label className="form-check-label" htmlFor="exampleCheck1">{t("CheckMeOut")}</label>
            </div>
            <input type="submit" className="btn btn-primary" value="Registration" />
        </form>);
    }

    return render();
}

export default Registration;