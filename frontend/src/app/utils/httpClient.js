"use client"
const baseUrl = "";

const httpClient = {

    get: (endpoint) => {

        let config = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        }

        endpoint = baseUrl + endpoint;
        return fetch(endpoint, config);
    },
    post: (endpoint, body) => {
        let config = {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        }

        endpoint = baseUrl + endpoint;
        return fetch(endpoint, config);
    },

    delete: (endpoint) => {
        let config = {
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json'
            }
        }

        endpoint = baseUrl + endpoint;
        return fetch(endpoint, config);
    },

    put: (endpoint, body) => {
        let config = {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        }
        endpoint = baseUrl + endpoint;
        return fetch(endpoint, config)
    }
}

export default httpClient; 