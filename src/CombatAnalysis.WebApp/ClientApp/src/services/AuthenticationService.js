import HttpClient from '../helpers/HttpClient'

class AuthenticationService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async authenticationAsync() {
        try {
            const result = await this._httpClient.getAsync("Authentication");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default AuthenticationService;