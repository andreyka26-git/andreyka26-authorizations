import { UserManager } from 'oidc-client-ts';
import { andreykaConfig, githubConfig } from '../config';

let userManager;

export function getAuthConfig() {
    console.log('auth-method')
    return localStorage.getItem('auth-method');
}

export function setAuthConfig() {
    const config = localStorage.getItem('auth-method');

    if (config === 'github') {
        userManager = new UserManager(githubConfig.settings);
    }

    if (config === 'andreyka') {
        userManager = new UserManager(andreykaConfig.settings);
    }
}

export async function isAuthenticated() {
    let token = await getAccessToken();

    if (!token)
        return false;

    return true;
}

export async function handleOAuthCallback(callbackUrl) {
    try {
        const user = await userManager.signinRedirectCallback(callbackUrl);
        return user;
    } catch(e) {
        alert(e);
        console.log(`error while handling oauth callback: ${e}`);
    }
}

export async function sendOAuthRequest() {
    await userManager.signinRedirect();
}

// renews token using refresh token
export async function renewToken() {
    const user = await userManager.signinSilent();

    return user;
}

export async function getAccessToken() {
    const user = await userManager.getUser();
    return user?.access_token;
}

export async function getUser() {
    const user = await userManager.getUser();
    return user;
}

export async function logout() {
    await userManager.clearStaleState()
    await userManager.signoutRedirect();
}

// This function is used to access token claims
// `.profile` is available in Open Id Connect implementations
// in simple OAuth2 it is empty, because UserInfo endpoint does not exist
// export async function getRole() {
//     const user = await userManager.getUser();
//     return user?.profile?.role;
// }

// This function is used to change account similar way it is done in Google
// export async function selectOrganization() {
//     const args = {
//         prompt: "select_account"
//     }
//     await userManager.signinRedirect(args);
// }