// custom settings that work with OAuth server
const githubSettings = {
    authority: 'https://github.com/login/oauth/authorize',
    client_id: 'PUT_YOUR_CLIENT_ID_HERE',
    client_secret: 'PUT_YOUR_CLIENT_ID_HERE',
    redirect_uri: 'http://localhost:3000/oauth/callback',
    silent_redirect_uri: 'http://localhost:3000/oauth/callback',
    post_logout_redirect_uri: 'http://localhost:3000/',
    response_type: 'code',
    metadata: {
        issuer: 'https://github.com',
        authorization_endpoint: 'https://github.com/login/oauth/authorize',
        userinfo_endpoint: 'https://github.com/login/oauth/authorize',
        end_session_endpoint: 'https://github.com/login/oauth/authorize',
        token_endpoint: 'http://localhost:9999/https://github.com/login/oauth/access_token'
    }
    // this is for getting user.profile data, when open id connect is implemented
    //scope: 'api1 openid profile'
};

export const githubConfig = {
    settings: githubSettings,
    flow: 'github'
};