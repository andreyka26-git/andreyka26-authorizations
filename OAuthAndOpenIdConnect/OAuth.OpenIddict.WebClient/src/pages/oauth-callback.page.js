import { useEffect, useRef } from "react";
import { handleOAuthCallback } from "../services/AuthService"

function OAuthCallback() {
    // rerendering the components does not change isProcessed, but remounting the component does change.
    const isProcessed = useRef(false);

    useEffect(() => {
        async function processOAuthResponse() {
            // this is needed, because React.StrictMode makes component to rerender
            // second time the auth code that is in req.url here is invalid,
            // so we want it to execute one time only.
            if (isProcessed.current) {
                return;
            }

            isProcessed.current = true;

            const currentUrl = window.location.href;

            await handleOAuthCallback(currentUrl);
            window.location = window.location.origin + '/resources';
        }

        processOAuthResponse();
    }, [])
}

export default OAuthCallback;