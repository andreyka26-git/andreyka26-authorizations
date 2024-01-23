// custom settings that work with OAuth server
const githubSettings = {
    authority: 'https://github.com/login/oauth/authorize',
    client_id: 'PUT_YOUR_CLIENT_ID_HERE',
    client_secret: 'PUT_YOUR_CLIENT_SECRET_HERE',
    redirect_uri: 'http://localhost:3000/oauth/callback',
    silent_redirect_uri: 'http://localhost:3000/oauth/callback',
    post_logout_redirect_uri: 'http://localhost:3000/',
    response_type: 'code',
    metadata: {
        authorization_endpoint: 'https://github.com/login/oauth/authorize',
        token_endpoint: 'http://localhost:9999/https://github.com/login/oauth/access_token',
    },
    // this is for getting user.profile data, when open id connect is implemented
    scope: 'repo'
};

export const githubConfig = {
    settings: githubSettings,
    flow: 'github'
};