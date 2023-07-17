import HttpClient from '../helpers/HttpClient';

class CommunityUserService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("CommunityUser");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`CommunityUser/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async searchByCommunityIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`CommunityUser/searchByCommunityId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async searchByUserId(id) {
        try {
            const result = await this._httpClient.getAsync(`CommunityUser/searchByUserId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`CommunityUser/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default CommunityUserService;