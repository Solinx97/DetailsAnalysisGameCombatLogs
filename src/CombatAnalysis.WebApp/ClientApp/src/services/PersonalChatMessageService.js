import HttpClient from '../helpers/HttpClient';

class PersonalChatMessageService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("PersonalChatMessage");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`PersonalChatMessage/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async findByChatIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`PersonalChatMessage/findByChatId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("PersonalChatMessage", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async updateAsync(data) {
        try {
            const result = await this._httpClient.putAsync("PersonalChatMessage", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`PersonalChatMessage/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteByChatIdAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`PersonalChatMessage/deleteByChatId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default PersonalChatMessageService;