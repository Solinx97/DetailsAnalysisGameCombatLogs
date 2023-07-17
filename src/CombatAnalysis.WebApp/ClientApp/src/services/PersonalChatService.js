import HttpClient from '../helpers/HttpClient'

class PersonalChatService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("PersonalChat");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`PersonalChat/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async isExistAsync(userId, targetUserId) {
        try {
            const result = await this._httpClient.getAsync(`PersonalChat/isExist?initiatorId=${userId}&companionId=${targetUserId}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("PersonalChat", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`PersonalChat/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default PersonalChatService;