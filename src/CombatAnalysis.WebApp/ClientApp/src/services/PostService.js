import HttpClient from '../helpers/HttpClient'

class PostService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        const result = await this._httpClient.getAllAsync("Post");
        return result;
    }

    async getByIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`Post/${id}`);
        return result;
    }

    async createAsync(data) {
        const result = await this._httpClient.postAsync("Post", data);
        return result;
    }
}

export default PostService;