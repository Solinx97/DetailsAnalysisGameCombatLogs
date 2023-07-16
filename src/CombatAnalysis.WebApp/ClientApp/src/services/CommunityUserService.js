import HttpClient from '../helpers/HttpClient'

class CommunityUserService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        const result = await this._httpClient.getAllAsync("CommunityUser");
        return result;
    }

    async getByIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`CommunityUser/${id}`);
        return result;
    }

    async searchByCommunityIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`CommunityUser/searchByCommunityId/${id}`);
        return result;
    }

    async deleteAsync(id) {
        const result = await this._httpClient.deleteAsync(`CommunityUser/${id}`);
        return result;
    }
}

export default CommunityUserService;