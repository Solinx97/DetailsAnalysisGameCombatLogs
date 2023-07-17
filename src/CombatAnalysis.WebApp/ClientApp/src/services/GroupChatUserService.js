import HttpClient from '../helpers/HttpClient'

class GroupChatUserService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("GroupChatUser");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`GroupChatUser/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async findByChatIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`GroupChatUser/findByChatId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("GroupChatUser", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`GroupChatUser/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default GroupChatUserService;