import { UserManager } from 'oidc-client-ts';

// custom settings that work with our ouwn OAuth server
// const settings = {
//     authority: 'https://localhost:7000',
//     client_id: 'react-client',
//     client_secret: '901564A5-E7FE-42CB-B10D-61EF6A8F3654',
//     redirect_uri: 'http://localhost:3000/oauth/callback',
//     silent_redirect_uri: 'http://localhost:3000/oauth/callback',
//     post_logout_redirect_uri: 'http://localhost:3000/',
//     response_type: 'code',
//     // this is for getting user.profile data, when open id connect is implemented
//     //scope: 'api1 openid profile'
//     // this is just for OAuth2 flow
//     scope: 'api1'
// };

const settings = {
    authority: 'https://github.com/login/oauth/authorize',
    client_id: '',
    client_secret: '',
    redirect_uri: 'http://localhost:3000/oauth/callback',
    silent_redirect_uri: 'http://localhost:3000/oauth/callback',
    post_logout_redirect_uri: 'http://localhost:3000/',
    response_type: 'code',
    metadata: {
        issuer: 'https://github.com',
        authorization_endpoint: 'https://github.com/login/oauth/authorize',
        userinfo_endpoint: 'https://github.com/login/oauth/authorize',
        end_session_endpoint: 'https://github.com/login/oauth/authorize',
        token_endpoint: 'https://cors-anywhere.herokuapp.com/https://github.com/login/oauth/access_token'
    }
    // this is for getting user.profile data, when open id connect is implemented
    //scope: 'api1 openid profile'
    // this is just for OAuth2 flow
};

const userManager = new UserManager(settings);

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