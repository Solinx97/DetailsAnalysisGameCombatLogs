class HttpClient {
    _baseAddressApi = "";

    set baseAddressApi(value) {
        this._baseAddressApi = value;
    }

    constructor() {
        this._baseAddressApi = "/api/v1";
    }

    async getAsync(requestUri) {
        const response = await fetch(`${this._baseAddressApi}/${requestUri}`);
        if (response.status !== 200 && response.status !== 204) {
            throw new Error(`Could not fetch ${requestUri}, received ${response.status}`);
        }

        const result = await response.json();
        return result;
    }

    async postAsync(requestUri, content) {
        const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(content)
        });

        if (response.status !== 200) {
            throw new Error(`Could not fetch ${requestUri}, received ${response.status}`);
        }

        const result = await response.json();
        return result;
    }

    async postEmptyAsync(requestUri) {
        const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.status !== 200) {
            throw new Error(`Could not fetch ${requestUri}, received ${response.status}`);
        }

        return true;
    }

    async putAsync(requestUri, content)  {
        const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(content)
        });

        if (response.status !== 200) {
            throw new Error(`Could not fetch ${requestUri}, received ${response.status}`);
        }

        const result = await response.json();
        return result;
    }

    async deleteAsync(requestUri) {
        const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
            method: 'DELETE'
        });

        if (response.status !== 200) {
            throw new Error(`Could not fetch ${requestUri}, received ${response.status}`);
        }

        const result = await response.json();
        return result;
    }
}

export default HttpClient;