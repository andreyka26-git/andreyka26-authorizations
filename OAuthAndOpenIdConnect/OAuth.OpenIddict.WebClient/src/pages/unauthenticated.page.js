import { sendOAuthRequest } from '../services/AuthService';
import { Navigate } from "react-router-dom";

function UnAuthenticated({ authenticated }) {
  if (authenticated) {
    return <Navigate to='/resources' replace />;
  }

  return (
    <>
      You are not authenticated - Log In first
      <br></br>
      <button onClick={(_) => sendOAuthRequest()}>Login via andreyka26</button>
    </>
  )
}

export default UnAuthenticated;