import HttpClient from '../helpers/HttpClient'

class PostLikeService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("PostLike");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`PostLike/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async searchByPostIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`PostLike/searchByPostId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async updateAsync(data) {
        try {
            const result = await this._httpClient.putAsync("PostLike", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("PostLike", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`PostLike/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default PostLikeService;