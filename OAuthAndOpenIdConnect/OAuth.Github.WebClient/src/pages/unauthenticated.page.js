import { sendOAuthRequest } from '../services/AuthService';
import { Navigate } from "react-router-dom";

function UnAuthenticated({ authenticated }) {
  if (authenticated) {
    return <Navigate to='/resources' replace />;
  }

  return (
    <>
      <span>You are not authenticated - Log In first</span>
      <br />
      <button onClick={sendOAuthRequest}>Login via github</button>
    </>
  )
}

export default UnAuthenticated;