import { useEffect, useState } from 'react';
import { getUser, logout } from './services/AuthService'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UnAuthenticated from './pages/unauthenticated.page';
import ProtectedRoute from './components/ProtectedRoute';
import OAuthCallback from './pages/oauth-callback.page';

function App() {
  const [authenticated, setAuthenticated] = useState(false);
  const [user, setUser] = useState('');
  const [rendering, setRendering] = useState(true);
  const [resource, setResource] = useState('')

  useEffect(() => {
    async function fetchData() {
      const user = await getUser();

      setAuthenticated(!!user?.access_token);
      setUser(user);
      setRendering(false);

      if (!!user?.access_token) {
        const url = 'https://localhost:7002/resources';
        const options = {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${user?.access_token}`
          }
        };

        try {
          const response = await fetch(url, options);
          if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
          }

          const data = await response.text();
          setResource(data);
        } catch (error) {
          console.error('There was an error fetching the data', error);
        }
      }
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
            <div>Authenticated {JSON.stringify(user)}</div>
            
            <br></br>
            
            <div>Resource: {resource}</div>

            <button onClick={logout}>Log out</button>
          </ProtectedRoute>
        } />

        <Route path='/oauth/callback' element={<OAuthCallback />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
