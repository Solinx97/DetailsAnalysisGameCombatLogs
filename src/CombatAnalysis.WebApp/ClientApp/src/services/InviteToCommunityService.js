import HttpClient from '../helpers/HttpClient'

class InviteToCommunityService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        const result = await this._httpClient.getAllAsync("InviteToCommunity");
        return result;
    }

    async getByIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`InviteToCommunity/${id}`);
        return result;
    }

    async createAsync(data) {
        const result = await this._httpClient.postAsync("InviteToCommunity", data);
        return result;
    }

    async deleteAsync(id) {
        const result = await this._httpClient.deleteAsync(`InviteToCommunity/${id}`);
        return result;
    }
}

export default InviteToCommunityService;