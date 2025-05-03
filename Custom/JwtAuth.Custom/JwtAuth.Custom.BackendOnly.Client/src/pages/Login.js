import React, { useState } from "react";
import axios from "axios";
import { authenticate, authServerDomain } from "../services/Api";
import { useNavigate } from "react-router-dom";

function LoginPage() {
  const [userName, setUserName] = useState("andreyka26_");
  const [password, setPassword] = useState("Mypass1*");
  const navigate = useNavigate();

  const googleReturnUrl = "http://localhost:3000/callback";
  const provider = "Google";
  const actionUrl = `${authServerDomain}/authorization/external-login`;

  async function handleSubmit(event) {
    event.preventDefault();

    const [token, refreshToken] = await authenticate(userName, password);
    console.log(token, refreshToken);

    localStorage.setItem("token", token);
    localStorage.setItem("refreshToken", refreshToken);

    navigate("/");
    // window.location = `${window.location.origin}/`;
  }

  function handleGoogleSubmit(event) {
    event.preventDefault();

    let url = `${actionUrl}?provider=${provider}&returnUrl=${googleReturnUrl}`;
    return axios.get(url);
  }

  function handleUserNameChange(event) {
    setUserName({ value: event.target.value });
  }

  function handlePasswordhange(event) {
    setPassword({ value: event.target.value });
  }

  return (
    <div>
      Login Page
      <form onSubmit={handleSubmit}>
        <label>
          User Name:
          <input type="text" value={userName} onChange={handleUserNameChange} />
        </label>
        <label>
          Password:
          <input type="text" value={password} onChange={handlePasswordhange} />
        </label>
        <input type="submit" value="Submit" />
      </form>
      <form action={actionUrl} method="get">
        <input type="hidden" name="returnUrl" value={googleReturnUrl} />
        <input type="hidden" name="provider" value={provider} />
        <button type="submit">Google Working</button>
      </form>
      <form onSubmit={handleGoogleSubmit}>
        <input type="hidden" name="returnUrl" value={googleReturnUrl} />
        <input type="hidden" name="provider" value={provider} />
        <button type="submit">Google not Working</button>
      </form>
    </div>
  );
}
export default LoginPage;
