import HttpClient from '../helpers/HttpClient';

class UserPostService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("UserPost");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`UserPost/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async searchByUserId(id) {
        try {
            const result = await this._httpClient.getAsync(`UserPost/searchByUserId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("UserPost", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`UserPost/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default UserPostService;