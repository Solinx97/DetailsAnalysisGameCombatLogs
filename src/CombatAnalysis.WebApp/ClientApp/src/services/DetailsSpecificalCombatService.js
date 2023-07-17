import HttpClient from '../helpers/HttpClient';

class DetailsSpecificalCombatService {
    _httpClient = null;

    constructor() {
        this._httpClient = new HttpClient();
    }

    async combatPlayersByCombatIdAsync(id) {
        try {
            const result = await this._httpClient.postAsync(`DetailsSpecificalCombat/combatPlayersByCombatId/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }

    async combatByIdAsync(id) {
        try {
            const result = await this._httpClient.postAsync(`DetailsSpecificalCombat/combatById/${id}`);
            return result;
        } catch (e) {
            console.log(e.message);
            return null;
        }
    }
}

export default DetailsSpecificalCombatService;