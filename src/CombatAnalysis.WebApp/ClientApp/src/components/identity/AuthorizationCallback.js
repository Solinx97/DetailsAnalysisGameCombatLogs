import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';
import { useLazyAuthorizationCodeExchangeQuery } from '../../store/api/Identity.api';
import { useLazyStateValidateQuery } from '../../store/api/UserApi';

import '../../styles/identity/authorizationCallback.scss';

const unauthorizedTimeout = 4000;

const AuthorizationCallback = () => {
    const { t } = useTranslation("identity/authorizationCallback");

    const navigate = useNavigate();

    const { checkAuthAsync } = useAuth();

    const [stateIsValid, setStateIsValid] = useState(true);

    const [stateValidateQuery] = useLazyStateValidateQuery();
    const [authorizationCodeExchangeQuery] = useLazyAuthorizationCodeExchangeQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const code = queryParams.get("code");
        const state = queryParams.get("state");

        const validateState = async () => {
            await validateStateAsync(state, code);
        }

        validateState();
    }, []);

    useEffect(() => {
        let timeout;
        if (!stateIsValid) {
            timeout = setTimeout(() => {
                navigate("/");
            }, unauthorizedTimeout);
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