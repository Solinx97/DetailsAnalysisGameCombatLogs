import HttpClient from '../helpers/HttpClient';

class CustomerService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("Customer");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`Customer/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async searchByUserIdAsync(id) {
        try {
            const result = await this._httpClient.getAsync(`Customer/searchByUserId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async createAsync(data) {
        try {
            const result = await this._httpClient.postAsync("Customer", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async updateAsync(data) {
        try {
            const result = await this._httpClient.putAsync("Customer", data);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async deleteAsync(id) {
        try {
            const result = await this._httpClient.deleteAsync(`Customer/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default CustomerService;