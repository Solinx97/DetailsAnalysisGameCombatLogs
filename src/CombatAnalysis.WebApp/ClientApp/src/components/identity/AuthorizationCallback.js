import { memo, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const AuthorizationCallback = () => {
    const navigate = useNavigate();

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
        window.sessionStorage.removeItem("state");
    }, [stateIsValid]);

    const navigateToTokenAsync = async (code) => {
        const codeVerifier = window.sessionStorage.getItem("codeVerifier");

        const encodedAuthorizationCode = encodeURIComponent(code);
        const result = await fetch(`/api/v1/Identity?codeVerifier=${codeVerifier}&authorizationCode=${encodedAuthorizationCode}`);
        if (result.status === 200) {
            window.sessionStorage.removeItem("codeVerifier");

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
        stateIsValid
            ? <div>Authorization...</div>
            : <div>Unauthorized. Pls try authorization one more time</div>
    );
}

export default memo(AuthorizationCallback);