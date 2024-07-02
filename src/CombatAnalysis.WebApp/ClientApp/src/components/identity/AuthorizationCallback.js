import { memo, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';

import '../../styles/identity/authorizationCallback.scss';

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

    const navigateToTokenAsync = async (code) => {
        const codeVerifier = window.sessionStorage.getItem("codeVerifier");

        const encodedAuthorizationCode = encodeURIComponent(code);
        const result = await fetch(`/api/v1/Identity?codeVerifier=${codeVerifier}&authorizationCode=${encodedAuthorizationCode}`);
        if (result.status === 200) {
            window.sessionStorage.removeItem("codeVerifier");
            window.sessionStorage.removeItem("state");

            await checkAuthAsync();

            navigate("/");
        }
    }

    const validateState = (state) => {
        const storedState = window.sessionStorage.getItem("state");
        const stateIsValid = state === storedState;

        setStateIsValid(stateIsValid);

        return stateIsValid;
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