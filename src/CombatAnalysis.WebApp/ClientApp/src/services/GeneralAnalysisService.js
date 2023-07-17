import HttpClient from '../helpers/HttpClient';

class GeneralAnalysisService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async combatPlayersByCombatIdAsync(id) {
        try {
            const result = await this._httpClient.postAsync(`GeneralAnalysis/combatPlayersByCombatId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async getByIdAsync(id) {
        try {
            const result = await this._httpClient.postAsync(`GeneralAnalysis/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default GeneralAnalysisService;