import HttpClient from '../helpers/HttpClient'

class AccountService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async loginAsync(data) {
        try {
            const result = await this._httpClient.postAsync("Account", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async registrationAsync(data) {
        try {
            const result = await this._httpClient.postAsync("Account/registration", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async logoutAsync() {
        try {
            const result = await this._httpClient.postEmptyAsync("Account/logout");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getUserById(id) {
        try {
            const result = await this._httpClient.getAsync(`Account/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async updateAsync(data) {
        try {
            const result = await this._httpClient.putAsync("Account", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default AccountService;