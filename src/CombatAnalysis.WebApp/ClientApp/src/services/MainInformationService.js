import HttpClient from '../helpers/HttpClient';

class MainInformationService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async getAllAsync() {
        try {
            const result = await this._httpClient.getAsync("MainInformation");
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default MainInformationService;