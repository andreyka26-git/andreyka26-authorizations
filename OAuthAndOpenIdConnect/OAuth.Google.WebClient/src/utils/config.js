// custom settings that work with our ouwn OAuth server
const googleSettings = {
    authority: 'https://accounts.google.com/',
    client_id: 'CLIENT_ID',
    client_secret: 'CLIENT_SECRET',
    redirect_uri: 'http://localhost:3000/oauth/callback',
    silent_redirect_uri: 'http://localhost:3000/oauth/callback',
    post_logout_redirect_uri: 'http://localhost:3000/',
    response_type: 'code'
};

export const googleConfig = {
    settings: googleSettings,
    flow: 'google'
};