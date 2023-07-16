import HttpClient from '../helpers/HttpClient'

class AccountService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async loginAsync(data) {
        const result = await this._httpClient.postAsync("Account", data);
        return result;
    }

    async registrationAsync(data) {
        const result = await this._httpClient.postAsync("Account/registration", data);
        return result;
    }
}

export default AccountService;