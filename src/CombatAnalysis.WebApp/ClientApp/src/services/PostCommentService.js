import HttpClient from '../helpers/HttpClient'

class PostCommentService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("PostComment");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`PostComment/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async searchByPostIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`PostComment/searchByPostId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async updateAsync(data) {
        try {
            const result = await this._httpClient.putAsync("PostComment", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("PostComment", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default PostCommentService;