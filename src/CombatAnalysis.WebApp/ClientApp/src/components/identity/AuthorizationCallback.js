import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const AuthorizationCallback = () => {
    const navigate = useNavigate();

    const navigateToTokenAsync = async (code) => {
        const codeVerifier = window.sessionStorage.getItem("codeVerifier");
        window.sessionStorage.removeItem("codeVerifier");

        const encodedAuthorizationCode = encodeURIComponent(code);
        const result = await fetch(`/api/v1/Identity?codeVerifier=${codeVerifier}&authorizationCode=${encodedAuthorizationCode}`);
        if (result.status === 200) {
            navigate("/");
        }
    }

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const code = queryParams.get("code");

        const naviagetToToken = async () => {
            await navigateToTokenAsync(code);
        }

        naviagetToToken();
    }, [])

    return (<div>Authorization...</div>);
}

export default AuthorizationCallback;