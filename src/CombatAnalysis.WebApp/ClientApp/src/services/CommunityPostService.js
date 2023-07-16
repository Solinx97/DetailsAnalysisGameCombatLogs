import HttpClient from '../helpers/HttpClient'

class CommunityPostService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        const result = await this._httpClient.getAllAsync("CommunityPost");
        return result;
    }

    async getByIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`CommunityPost/${id}`);
        return result;
    }

    async searchByCommunityIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`CommunityPost/searchByCommunityId/${id}`);
        return result;
    }

    async createAsync(data) {
        const result = await this._httpClient.postAsync("CommunityPost", data);
        return result;
    }
}

export default CommunityPostService;