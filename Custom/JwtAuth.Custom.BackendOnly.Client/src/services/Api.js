import axios from 'axios';

let refreshingFunc = undefined;

axios.interceptors.response.use(
    (res) =>  res,
    async (error) => {
        const originalConfig = error.config;
        const token = localStorage.getItem("token");
        
        // if we don't have token in local storage or error is not 401 just return error and break req.
        if (!token || !isUnauthorizedError(error)) {
            return Promise.reject(error);
        }

        try {
            // the trick here, that `refreshingFunc` is global, e.g. 2 expired requests will get the same function pointer and await same function.
            if (!refreshingFunc)
                refreshingFunc = renewToken();
                
            const [newToken, newRefreshToken] = await refreshingFunc;
            localStorage.setItem("token", newToken);
            localStorage.setItem("refreshToken", newRefreshToken);

            originalConfig.headers.Authorization = `Bearer ${newToken}`;

            // retry original request
            try {
                return await axios.request(originalConfig);
            } catch(innerError) {
                // if original req failed with 401 again - it means server returned not valid token for refresh request
                if (isUnauthorizedError(innerError)) {
                    throw innerError;
                }                  
            }
        } catch (err) {
            localStorage.removeItem("token");
            localStorage.removeItem("refreshToken");

            window.location = `${window.location.origin}/login`;
        } finally {
            refreshingFunc = undefined;
        }
    },
)

function isUnauthorizedError(error) {
    const {
        response: { status, statusText },
    } = error;

    return status === 401;
}

export async function authenticate(userName, password) {
    const loginPayload = {
        userName: userName,
        password: password
    };

    const response = await axios.post("https://localhost:7000/authorization/token", loginPayload);
    
    const token = response.data.authorizationToken;
    const refreshToken = response.data.refreshToken;

    return [token, refreshToken];
}

export async function renewToken() {
    const refreshToken = localStorage.getItem("refreshToken");

    if (!refreshToken)
        throw new Error('refresh token does not exist');

    const refreshPayload = {
        refreshToken: refreshToken
    };

    const response = await axios.post("https://localhost:7000/authorization/refresh", refreshPayload);
    const token = response.data.authorizationToken;
    const newRefreshToken = response.data.refreshToken;

    return [token, newRefreshToken];
}

export async function getResources() {
    const headers = withAuth();

    const options = {
        headers: headers
    }

    const response = await axios.get("https://localhost:7000/api/resources", options);
    const data = response.data;

    return data;
}

function withAuth(headers) {
    const token = localStorage.getItem("token");

    if (!token) {
        window.location = `${window.location.origin}/login`;
        return;
    }

    if (!headers) {
        headers = { };
    }
    
    headers.Authorization = `Bearer ${token}`

    return headers;
}
