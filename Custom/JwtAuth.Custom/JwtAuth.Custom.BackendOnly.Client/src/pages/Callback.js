import { useEffect } from 'react';

function CallbackPage() {
    useEffect(() => {
        try {
            const query = new URLSearchParams(window.location.search);
            const tokenBase64 = query.get('token');

            const tokenJson = atob(tokenBase64);
            const token = JSON.parse(tokenJson);

            if (token) {
                localStorage.setItem("token", token.AuthorizationToken);
            }
        }
        catch(e) {
            console.log(e.message);
        }

        window.location = `${window.location.origin}/`;
    });
}

export default CallbackPage;