import { andreykaConfig, githubConfig } from '../config';
import { sendOAuthRequest, setAuthConfig } from '../services/AuthService';
import { Navigate } from "react-router-dom";

function UnAuthenticated({ authenticated }) {
  if (authenticated) {
    return <Navigate to='/resources' replace />;
  }

  return (
    <>
      You are not authenticated - Log In first
      <br></br>
      <button onClick={async (_) => {
        localStorage.setItem('auth-method', 'andreyka');
        setAuthConfig();
        
        await sendOAuthRequest();
      }}>Login via andreyka26</button>

      <button onClick={async (_) => {
        localStorage.setItem('auth-method', 'github');
        setAuthConfig();

        await sendOAuthRequest();
      }}>Login via Github</button>
    </>
  )
}

export default UnAuthenticated;