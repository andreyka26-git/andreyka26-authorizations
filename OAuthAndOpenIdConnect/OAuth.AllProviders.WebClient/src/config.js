// custom settings that work with our ouwn OAuth server
const andreykaSettings = {
    authority: 'https://localhost:7000',
    client_id: 'react-client',
    client_secret: '901564A5-E7FE-42CB-B10D-61EF6A8F3654',
    redirect_uri: 'http://localhost:3000/oauth/callback',
    silent_redirect_uri: 'http://localhost:3000/oauth/callback',
    post_logout_redirect_uri: 'http://localhost:3000/',
    response_type: 'code',
    // this is for getting user.profile data, when open id connect is implemented
    //scope: 'api1 openid profile'
    // this is just for OAuth2 flow
    scope: 'api1'
};

const githubSettings = {
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
        token_endpoint: 'http://localhost:9999/https://github.com/login/oauth/access_token'
    }
    // this is for getting user.profile data, when open id connect is implemented
    //scope: 'api1 openid profile'
    // this is just for OAuth2 flow
};

export const githubConfig = {
    settings: githubSettings,
    flow: 'github'
};

export const andreykaConfig = {
    settings: andreykaSettings,
    flow: 'andreyka'
};