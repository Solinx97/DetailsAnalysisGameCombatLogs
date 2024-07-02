
const useAuthorization = () => {
    const generateCodeVerifier = () => {
        const array = new Uint8Array(32);
        window.crypto.getRandomValues(array);

        const codeVerifier = base64UrlEncode(array);

        return codeVerifier;
    }

    const generateCodeChallengeAsync = async (verifier) => {
        const codeChallengeMethod = process.env.REACT_APP_CODE_CHALLENGE_METHOD;

        const buffer = await crypto.subtle.digest(codeChallengeMethod, new TextEncoder().encode(verifier));
        const codeChalenge = base64UrlEncode(buffer);

        return codeChalenge;
    }

    const base64UrlEncode = (buffer) => {
        const encodedCode = btoa(String.fromCharCode.apply(null, new Uint8Array(buffer)))
            .replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');

        return encodedCode;
    }

    const navigateToAuthorizationAsync = async () => {
        const identityServerAuthPath = process.env.REACT_APP_IDENTITY_SERVER_AUTH_PATH;

        const uri = await createIdentityServerUrlAsync(identityServerAuthPath);

        window.location.href = uri;
    }

    const navigateToRegistrationAsync = async () => {
        const identityServerRegistrationPath = process.env.REACT_APP_IDENTITY_SERVER_REGISTRY_PATH;

        const uri = await createIdentityServerUrlAsync(identityServerRegistrationPath);

        window.location.href = uri;
    }

    const createIdentityServerUrlAsync = async (identityServerPath) => {
        const codeVerifier = generateCodeVerifier();
        const state = generateCodeVerifier();
        const codeChallenge = await generateCodeChallengeAsync(codeVerifier);

        window.sessionStorage.setItem("codeVerifier", codeVerifier);
        window.sessionStorage.setItem("state", state);

        const codeChallengeMethod = process.env.REACT_APP_CODE_CHALLENGE_METHOD;
        const clientId = process.env.REACT_APP_CLIENT_ID;
        const clientScope = process.env.REACT_APP_CLIENT_SCOPE;
        const redirectUri = process.env.REACT_APP_REDIRECT_URI;
        const identityServer = process.env.REACT_APP_IDENTITY_SERVER;

        const uri = `${identityServer}/${identityServerPath}?grantType=code&clientTd=${clientId}&redirectUri=${redirectUri}&scope=${clientScope}&state=${state}&codeChallengeMethod=${codeChallengeMethod}&codeChallenge=${codeChallenge}`;

        return uri;
    }

    return { navigateToAuthorizationAsync, navigateToRegistrationAsync };
}

export default useAuthorization;