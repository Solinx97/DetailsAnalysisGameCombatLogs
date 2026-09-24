import { useAuth } from '@/shared/hooks/useAuth';
import logger from '@/utils/Logger';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyAuthorizationCodeExchangeQuery, useLazyRefreshTokenQuery } from '../../api/Identity.api';
import { useLazyCancelAuthorizationQuery, useLazyStateValidateQuery } from '../../api/User.api';

import './AuthorizationCallback.scss';

const AuthorizationCallback: React.FC = () => {
    const unauthorizedTimeoutLimit = 4000;

    const { t } = useTranslation('identity/authorizationCallback');

    const navigate = useNavigate();
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);

    const auth = useAuth();

    const [stateIsValid, setStateIsValid] = useState(true);
    const [accessRestored, setAcessRestored] = useState(false);
    const [verified, setVerified] = useState(false);

    const [cancelAuthorization] = useLazyCancelAuthorizationQuery();
    const [stateValidate] = useLazyStateValidateQuery();
    const [authorizationCodeExchange] = useLazyAuthorizationCodeExchangeQuery();
    const [refreshJWT] = useLazyRefreshTokenQuery();

    useEffect(() => {
        const authorizationCanceled = queryParams.get("cancel") === "true";

        const accessRestored = queryParams.get("accessRestored") === "true";
        setAcessRestored(accessRestored);

        const verified = queryParams.get("verified") === "true";
        setVerified(verified);

        const code = queryParams.get("code") ?? "";
        const state = queryParams.get("state") ?? "";

        if (!code && !state && !accessRestored && !verified) {
            navigate("/");
        }

        if (code && state) {
            (async () => {
                await validateStateAsync(state, code);
            })();
        }

        if (accessRestored) {
            (async () => {
                await refreshJWTAsync();
            })();
        }


        if (authorizationCanceled) {
            (async () => {
                await cancelAuthorizationAsync();

                navigate("/");
            })();
        }
    }, []);

    useEffect(() => {
        let timeout: NodeJS.Timeout;
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
        try {
            await authorizationCodeExchange(authorizationCode).unwrap();
            await auth?.checkAuthAsync();

            navigate("/");
        } catch (e) {
            logger.error("Failed to exchange the authorization code", e);

            setStateIsValid(false);
        }
    }

    const validateStateAsync = async (state: string, code: string) => {
        try {
            await stateValidate(state).unwrap();
            await navigateToTokenAsync(code);
        } catch (e) {
            logger.error("Failed to validate the authorzation state", e);

            setStateIsValid(false);
        }
    }

    const refreshJWTAsync = async () => {
        try {
            await refreshJWT().unwrap();
        } catch (e) {
            logger.error("Failed to refresh JWT token", e);
        }
    }

    const cancelAuthorizationAsync = async () => {
        try {
            await cancelAuthorization().unwrap();
        } catch (e) {
            logger.error("Failed to cancel authorization", e);
        }
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