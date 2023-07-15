import { useState } from "react"

let dataState = {
    data: null,
    statusCode: 200,
    error: null
}

const useHttpClientAsync = (addressApi = "/api/v1") => {
    const [baseAddressApi, setBaseAddressApi] = useState(addressApi);

    const getAllAsync = async (requestUri) => {
        const response = await fetch(`${baseAddressApi}/${requestUri}`);
        if (response.status !== 200) {
            dataState = {
                data: null,
                statusCode: 200,
                error: response.statusText
            };

            return dataState;
        }

        const result = await response.json();
        dataState = {
            data: result,
            statusCode: 200,
            error: response.statusText
        };

        return dataState;
    }

    const getByIdAsync = async (requestUri, id) => {
        const response = await fetch(`${baseAddressApi}/${requestUri}/${id}`);
        if (response.status !== 200) {
            dataState = {
                data: null,
                statusCode: 200,
                error: response.statusText
            };

            return dataState;
        }

        const result = await response.json();
        dataState = {
            data: result,
            statusCode: 200,
            error: response.statusText
        };

        return dataState;
    }

    const postAsync = async (requestUri, content) => {
        const response = await fetch(`${baseAddressApi}/${requestUri}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(content)
        });

        if (response.status !== 200) {
            dataState = {
                data: null,
                statusCode: 200,
                error: response.statusText
            };

            return dataState;
        }

        const result = await response.json();
        dataState = {
            data: result,
            statusCode: 200,
            error: response.statusText
        };

        return dataState;
    }

    const putAsync = async (requestUri, data) => {

    }

    const deleteAsync = async (requestUri, id) => {
        const response = await fetch(`${baseAddressApi}/${requestUri}/${id}`, {
            method: 'DELETE'
        });

        if (response.status !== 200) {
            dataState = {
                data: null,
                statusCode: 200,
                error: response.statusText
            };

            return dataState;
        }

        const result = await response.json();
        dataState = {
            data: result,
            statusCode: 200,
            error: response.statusText
        };

        return dataState;
    }

    return [getAllAsync, getByIdAsync, postAsync, putAsync, deleteAsync, setBaseAddressApi]
}

export default useHttpClientAsync;