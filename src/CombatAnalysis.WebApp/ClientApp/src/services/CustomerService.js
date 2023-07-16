import HttpClient from '../helpers/HttpClient'

class CustomerService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        const result = await this._httpClient.getAllAsync("Customer");
        return result;
    }

    async getByIdAsync(id) {
        const result = await this._httpClient.getAllAsync(`Customer/${id}`);
        return result;
    }

    async createAsync(data) {
        const result = await this._httpClient.postAsync("Customer", data);
        return result;
    }

    async deleteAsync(id) {
        const result = await this._httpClient.deleteAsync(`Customer/${id}`);
        return result;
    }
}

export default CustomerService;