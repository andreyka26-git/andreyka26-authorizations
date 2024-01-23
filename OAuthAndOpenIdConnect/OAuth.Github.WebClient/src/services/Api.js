export async function getGithubResources(token) {
  const url = 'https://api.github.com/user/repos';

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

    const data = await response.json();
    return data.map(d => d.name).join(', ');
  } catch (error) {
    console.error('There was an error fetching the data', error);
  }
}