import HttpClient from '../helpers/HttpClient'

class CommunityPostService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("CommunityPost");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`CommunityPost/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async searchByCommunityIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`CommunityPost/searchByCommunityId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("CommunityPost", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default CommunityPostService;