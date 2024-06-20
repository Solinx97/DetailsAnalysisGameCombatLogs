class HttpClient {
    set baseAddressApi(value) {
        this._baseAddressApi = value;
    }

    constructor() {
        this._baseAddressApi = "/api/v1";
        this._maxRetries = 3;
        this._interval = 1000;
    }

    async getAsync(requestUri) {
        let attempts = 0;
        while (attempts < this._maxRetries) {
            try {
                const response = await fetch(`${this._baseAddressApi}/${requestUri}`);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const result = await response.json();
                return result;
            } catch (error) {
                attempts++;
                if (attempts >= this._maxRetries) {
                    throw new Error(`Request failed after ${this._maxRetries} attempts: ${error}`);
                }

                await this._sleep(this._initialBackoff * Math.pow(2, attempts - 1));
            }
        }
    }

    async postAsync(requestUri, content) {
        let attempts = 0;
        while (attempts < this._maxRetries) {
            try {
                const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(content)
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const result = await response.json();
                return result;
            } catch (error) {
                attempts++;
                if (attempts >= this._maxRetries) {
                    throw new Error(`Request failed after ${this._maxRetries} attempts: ${error}`);
                }

                await this._sleep(this._initialBackoff * Math.pow(2, attempts - 1));
            }
        }
    }

    async postEmptyAsync(requestUri) {
        let attempts = 0;
        while (attempts < this._maxRetries) {
            try {
                const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                return true;
            } catch (error) {
                attempts++;
                if (attempts >= this._maxRetries) {
                    throw new Error(`Request failed after ${this._maxRetries} attempts: ${error}`);
                }

                await this._sleep(this._initialBackoff * Math.pow(2, attempts - 1));
            }
        }
    }

    async putAsync(requestUri, content) {
        let attempts = 0;
        while (attempts < this._maxRetries) {
            try {
                const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(content)
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const result = await response.json();
                return result;
            } catch (error) {
                attempts++;
                if (attempts >= this._maxRetries) {
                    throw new Error(`Request failed after ${this._maxRetries} attempts: ${error}`);
                }

                await this._sleep(this._initialBackoff * Math.pow(2, attempts - 1));
            }
        }
    }

    async deleteAsync(requestUri) {
        let attempts = 0;
        while (attempts < this._maxRetries) {
            try {
                const response = await fetch(`${this._baseAddressApi}/${requestUri}`, {
                    method: 'DELETE'
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const result = await response.json();
                return result;
            } catch (error) {
                attempts++;
                if (attempts >= this._maxRetries) {
                    throw new Error(`Request failed after ${this._maxRetries} attempts: ${error}`);
                }

                await this._sleep(this._initialBackoff * Math.pow(2, attempts - 1));
            }
        }
    }

    _sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}

export default HttpClient;