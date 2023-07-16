import HttpClient from '../helpers/HttpClient'

class CommunityService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        const result = await this._httpClient.getAllAsync("Community");
        return result;
    }

    async getByIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`Community/${id}`);
        return result;
    }
}

export default CommunityService;