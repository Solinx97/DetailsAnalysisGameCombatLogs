import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';
import { useLazyAuthorizationCodeExchangeQuery } from '../../store/api/user/Identity.api';
import { useLazyStateValidateQuery } from '../../store/api/core/User.api';

import '../../styles/identity/authorizationCallback.scss';

const unauthorizedTimeoutLimit = 4000;
const verificationTimeoutLimit = 3000;

const AuthorizationCallback = () => {
    const { t } = useTranslation("identity/authorizationCallback");

    const navigate = useNavigate();

    const { checkAuthAsync } = useAuth();

    const [stateIsValid, setStateIsValid] = useState(true);
    const [accessRestored, setAcessRestored] = useState(false);
    const [verified, setVerified] = useState(false);

    const [stateValidateQuery] = useLazyStateValidateQuery();
    const [authorizationCodeExchangeQuery] = useLazyAuthorizationCodeExchangeQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const code = queryParams.get("code");
        const state = queryParams.get("state");

        const accessRestored = queryParams.get("accessRestored");
        setAcessRestored(accessRestored);

        const verified = queryParams.get("verified");
        setVerified(verified);

        let verificationTimeout;
        if (accessRestored || verified) {
            verificationTimeout = setTimeout(() => {
                navigate("/");
            }, verificationTimeoutLimit);

            return;
        }

        const validateState = async () => {
            await validateStateAsync(state, code);
        }

        validateState();

        return () => {
            clearTimeout(verificationTimeout);
        }
    }, []);

    useEffect(() => {
        let timeout;
        if (!stateIsValid) {
            timeout = setTimeout(() => {
                navigate("/");
            }, unauthorizedTimeoutLimit);
        }

        return () => {
            clearTimeout(timeout);
        }
    }, [stateIsValid]);

    const navigateToTokenAsync = async (code) => {
        const encodedAuthorizationCode = encodeURIComponent(code);

        const response = await authorizationCodeExchangeQuery(encodedAuthorizationCode);
        if (response.data !== undefined) {
            await checkAuthAsync();

            navigate("/");
        }

        setStateIsValid(false);
    }

    const validateStateAsync = async (state, code) => {
        const response = await stateValidateQuery(state);

        if (response.data !== undefined) {
            await navigateToTokenAsync(code);

            return;
        }

        setStateIsValid(false);
    }

    if (accessRestored) {
        return (
            <div className="authorization-callback">
                <div className="successful">{t("AccessRestored")}</div>
            </div>
        );
    }

    if (verified) {
        return (
            <div className="authorization-callback">
                <div className="successful">{t("Verified")}</div>
            </div>
        );
    }

    return (
        <div className="authorization-callback">
            {stateIsValid
                ? <div className="successful">{t("Authorization")}</div>
                : <div className="failed">{t("Unauthorized")}</div>
            }
        </div>
    );
}

export default memo(AuthorizationCallback);