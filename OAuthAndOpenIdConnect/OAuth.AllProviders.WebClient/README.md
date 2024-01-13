# Issues

## CORS error when sending request to https://github.com/login/oauth/access_token to exchange auth_code for token.
We will use docker container "cors-anywhere" to fix it:
- https://github.com/Rob--W/cors-anywhere
- https://github.com/testcab/docker-cors-anywhere/tree/master

It is just server that will send this request to omit CORS problem, because CORS are treated in browser only.
We will trigger this proxy and it will send request with auth_code to token endpoint.

`docker run -p 9999:8080 --name cors-anywhere -d testcab/cors-anywhere`