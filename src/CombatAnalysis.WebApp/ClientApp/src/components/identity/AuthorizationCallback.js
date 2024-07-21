import { memo, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';

import '../../styles/identity/authorizationCallback.scss';

const unauthorizedTimeout = 4000;

const AuthorizationCallback = () => {
    const navigate = useNavigate();

    const { checkAuthAsync } = useAuth();

    const [stateIsValid, setStateIsValid] = useState(true);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const code = queryParams.get("code");
        const state = queryParams.get("state");

        const naviagetToToken = async () => {
            const stateIsValid = validateState(state);
            if (!stateIsValid) {
                return;
            }

            await navigateToTokenAsync(code);
        }

        naviagetToToken();
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
        const codeVerifier = getCookie("codeVerifier");
        const encodedAuthorizationCode = encodeURIComponent(code);

        const result = await fetch(`/api/v1/Identity?codeVerifier=${codeVerifier}&authorizationCode=${encodedAuthorizationCode}`);
        if (result.status === 200) {
            document.cookie = "codeVerifier=; expires=Thu, 01 Jan 1970 00:00:00 UTC;";
            document.cookie = "state=; expires=Thu, 01 Jan 1970 00:00:00 UTC;";

            await checkAuthAsync();

            navigate("/");
        }

        console.log(await result.text());

        setStateIsValid(false);
    }

    const validateState = (state) => {
        const storedState = getCookie("state");
        const stateIsValid = state === storedState;

        setStateIsValid(stateIsValid);

        return stateIsValid;
    }

    const getCookie = (cookieName) => {
        let name = cookieName + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');

        for (let i = 0; i < ca.length; i++) {
            let targetCookie = ca[i];

            while (targetCookie.charAt(0) === ' ') {
                targetCookie = targetCookie.substring(1);
            }

            if (targetCookie.indexOf(name) === 0) {
                return targetCookie.substring(name.length, targetCookie.length);
            }
        }

        return "";
    }

    return (
        <div className="authorization-callback">
            {stateIsValid
                ? <div className="successful">Authorization</div>
                : <div className="failed">Unauthorized. Pls try authorization one more time</div>
            }
        </div>
    );
}

export default memo(AuthorizationCallback);