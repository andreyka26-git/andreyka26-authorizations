import { useEffect, useState } from 'react';
import { getAccessToken, getUserName, isAuthenticated, logout } from './services/AuthService'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UnAuthenticated from './pages/unauthenticated.page';
import ProtectedRoute from './components/ProtectedRoute';
import OAuthCallback from './pages/oauth-callback.page';

function App() {
  const [authenticated, setAuthenticated] = useState(false);
  const [userName, setUserName] = useState('');
  const [rendering, setRendering] = useState(true);

  useEffect(() => {
    async function getToken() {
      setAuthenticated(await isAuthenticated());
      setUserName(await getUserName());
      setRendering(false);
    }

    getToken();
  }, [authenticated]);

  if (rendering) {
    return (<>Loading...</>)
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route path={'/'} element={<UnAuthenticated authenticated={authenticated} />} />

        <Route path={'/resources'} element={
          <ProtectedRoute authenticated={authenticated} redirectPath='/'>
            <div>Authenticated {JSON.stringify(userName)}</div>
            <button onClick={logout}>Log out</button>
          </ProtectedRoute>
        } />

        <Route path='/oauth/callback' element={<OAuthCallback />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
