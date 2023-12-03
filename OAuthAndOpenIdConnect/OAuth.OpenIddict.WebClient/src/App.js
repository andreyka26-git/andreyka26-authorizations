import { useEffect, useState } from 'react';
import { getUser, logout } from './services/AuthService'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UnAuthenticated from './pages/unauthenticated.page';
import ProtectedRoute from './components/ProtectedRoute';
import OAuthCallback from './pages/oauth-callback.page';
import { getResources } from './services/Api';

function App() {
  const [authenticated, setAuthenticated] = useState(false);
  const [user, setUser] = useState('');
  const [rendering, setRendering] = useState(true);
  const [resource, setResource] = useState('')

  useEffect(() => {
    async function fetchData() {
      const user = await getUser();
      const accessToken = user?.access_token; 
      
      setAuthenticated(!!accessToken);
      setUser(user);
      
      if (!!accessToken) {
        const data = getResources();
        setResource(data);
      }

      setRendering(false);
    }

    fetchData();
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
            <div>Authenticated OAuth Server result: {JSON.stringify(user)}</div>
            
            <br></br>
            
            <div>Resource got with access token: {resource}</div>

            <button onClick={logout}>Log out</button>
          </ProtectedRoute>
        } />

        <Route path='/oauth/callback' element={<OAuthCallback />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
