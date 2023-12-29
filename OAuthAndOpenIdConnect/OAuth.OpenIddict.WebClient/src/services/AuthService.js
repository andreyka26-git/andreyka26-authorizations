import { UserManager } from 'oidc-client-ts';
import { andreykaConfig } from '../utils/config';

const userManager = new UserManager(andreykaConfig.settings);

export async function getUser() {
    const user = await userManager.getUser();
    return user;
}

export async function isAuthenticated() {
    let token = await getAccessToken();

    return !!token;
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
    return await userManager.signinRedirect();
}

// renews token using refresh token
export async function renewToken() {
    const user = await userManager.signinSilent();

    return user;
}

export async function getAccessToken() {
    const user = await getUser();
    return user?.access_token;
}

export async function logout() {
    await userManager.clearStaleState()
    await userManager.signoutRedirect();
}

// This function is used to access token claims
// `.profile` is available in Open Id Connect implementations
// in simple OAuth2 it is empty, because UserInfo endpoint does not exist
// export async function getRole() {
//     const user = await getUser();
//     return user?.profile?.role;
// }

// This function is used to change account similar way it is done in Google
// export async function selectOrganization() {
//     const args = {
//         prompt: "select_account"
//     }
//     await userManager.signinRedirect(args);
// }