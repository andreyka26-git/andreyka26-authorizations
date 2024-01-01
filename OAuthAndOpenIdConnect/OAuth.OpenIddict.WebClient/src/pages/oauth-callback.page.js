import { useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { handleOAuthCallback, isAuthenticated } from "../services/AuthService"

function OAuthCallback({setIsAuthenticated}) {
    // rerendering the components does not change isProcessed, but remounting the component does change.
    const isProcessed = useRef(false);
    const navigate = useNavigate();

    useEffect(() => {
        async function processOAuthResponse() {
            // this is needed, because React.StrictMode makes component to rerender
            // second time the auth code that is in req.url here is invalid,
            // so we want it to execute one time only.
            if (isProcessed.current) {
                return;
            }

            isProcessed.current = true;

            try {
                const currentUrl = window.location.href;
                await handleOAuthCallback(currentUrl);

                setIsAuthenticated(await isAuthenticated());
                navigate("/resources");
            } catch (error) {
                console.error("Error processing OAuth callback:", error);
            }
        }

        processOAuthResponse();
    }, [])
}

export default OAuthCallback;