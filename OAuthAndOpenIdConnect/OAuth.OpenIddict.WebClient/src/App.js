import { useEffect, useState } from 'react';
import { getUser, logout } from './services/AuthService'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UnAuthenticated from './pages/unauthenticated.page';
import ProtectedRoute from './components/ProtectedRoute';
import { getResources as getAndreykaResources } from './services/Api';
import OAuthCallback from './pages/oauth-callback.page';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [resource, setResource] = useState(null)

  async function fetchData() {
    const user = await getUser();
    const accessToken = user?.access_token;

    setUser(user);

    if (accessToken) {
      const data = await getAndreykaResources(accessToken);;
      setResource(data);
      setIsAuthenticated(true);
    }

    setIsLoading(false);
  }

  useEffect(() => {
    fetchData();
  }, [isAuthenticated]);

  if (isLoading) {
    return (<>Loading...</>)
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route path={'/'} element={<UnAuthenticated authenticated={authenticated} />} />

        <Route path={'/resources'} element={
          <ProtectedRoute authenticated={authenticated} redirectPath='/'>
            <span>Authenticated OAuth Server result: {JSON.stringify(user)}</span>
            <br />
            <span>Resource got with access token: {resource}</span>
            <button onClick={logout}>Log out</button>
          </ProtectedRoute>
        } />

        <Route path='/oauth/callback' element={<OAuthCallback />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
