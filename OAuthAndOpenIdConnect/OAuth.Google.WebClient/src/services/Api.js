export async function getResources(token) {
  const url = 'https://www.googleapis.com/oauth2/v3/userinfo';

  const options = {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  };

  try {
    const response = await fetch(url, options);

    if (!response.ok) {
      throw new Error(`Error: ${response.status}`);
    }

    const data = await response.text();
    return data;
  } catch (error) {
    console.error('There was an error fetching the data', error);
  }
}