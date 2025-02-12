import { TimeoutId } from '@reduxjs/toolkit/dist/query/core/buildMiddleware/types';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';
import { useLazyStateValidateQuery } from '../../store/api/core/User.api';
import { useLazyAuthorizationCodeExchangeQuery } from '../../store/api/user/Identity.api';

import '../../styles/identity/authorizationCallback.scss';

const unauthorizedTimeoutLimit = 40000;

const AuthorizationCallback: React.FC = () => {
    const { t } = useTranslation("identity/authorizationCallback");

    const navigate = useNavigate();
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);

    const { checkAuth } = useAuth();

    const [stateIsValid, setStateIsValid] = useState(true);
    const [accessRestored, setAcessRestored] = useState(false);
    const [verified, setVerified] = useState(false);

    const [stateValidate] = useLazyStateValidateQuery();
    const [authorizationCodeExchange] = useLazyAuthorizationCodeExchangeQuery();

    useEffect(() => {
        const accessRestored: any = queryParams.get("accessRestored");
        setAcessRestored(accessRestored);

        const verified: any = queryParams.get("verified");
        setVerified(verified);
    }, []);

    useEffect(() => {
        const code: any = queryParams.get("code");
        const state: any = queryParams.get("state");

        const validateState = async () => {
            await validateStateAsync(state, code);
        }

        if (code && state) {
            validateState();
        }
    }, []);

    useEffect(() => {
        let timeout: TimeoutId;
        if (!stateIsValid || accessRestored || verified) {
            timeout = setTimeout(() => {
                navigate("/");
            }, unauthorizedTimeoutLimit);
        }

        return () => {
            clearTimeout(timeout);
        }
    }, [stateIsValid, accessRestored, verified]);

    const navigateToTokenAsync = async (authorizationCode: string) => {
        const response: any = await authorizationCodeExchange(authorizationCode);
        if (response?.data !== undefined) {
            await checkAuth();

            navigate("/");
        }

        setStateIsValid(false);
    }

    const validateStateAsync = async (state: string, code: string) => {
        const response: any = await stateValidate(state);
        if (response?.data !== undefined) {
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
                ? <div className="successful21">{t("Authorization")}</div>
                : <div className="failed">{t("Unauthorized")}</div>
            }
        </div>
    );
}

export default memo(AuthorizationCallback);