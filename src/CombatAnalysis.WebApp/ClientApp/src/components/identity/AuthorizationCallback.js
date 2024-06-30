import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthProvider';

const AuthorizationCallback = () => {
    const navigate = useNavigate();

    const { getCustomerDataAsync } = useAuth();

    const base64UrlEncode = (buffer) => {
        const encodedCode = btoa(String.fromCharCode.apply(null, new Uint8Array(buffer)))
            .replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');

        return encodedCode;
    }

    const generateCodeVerifier = () => {
        const array = new Uint8Array(32);
        window.crypto.getRandomValues(array);

        const codeVerifier = base64UrlEncode(array);
        return codeVerifier;
    }

    const naviagetToTokenAsync = async (code) => {
        const codeVerifier = generateCodeVerifier();

        const encodedCode = encodeURIComponent(code);
        const result = await fetch(`/api/v1/CombatAnalysisIdentity?codeVerifier=${codeVerifier}&authorizationCode=${encodedCode}`);
        if (result.status === 200) {
            await getCustomerDataAsync();

            navigate("/");
        }
    }

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const code = queryParams.get("code");

        const naviagetToToken = async () => {
            await naviagetToTokenAsync(code);
        }

        naviagetToToken();
    }, [])

    return (<div>Authorization...</div>);
}

export default AuthorizationCallback;