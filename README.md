# andreyka26-authorizations

There is no good documentation about Authorization in general. It is even worse for OAuth OpenIdConnect doc. This repo includes top most popular ways of building authentication/authorization using libraries/projects like OpenIddict, IdentityServer and custom schemes. Backend is written in c#/.NET

Information related to the repo can be found here: https://andreyka26.com/

Some projects do have docker support, some of them do not.

Dotnet is required to run Authorization and Resource Servers.
Node is needed to run React clients.

Follow public documentation to set up environment.

The servers implementation uses self signed TLS, if you see HTTPS error in the logs - run this command on your PC: `dotnet dev-certs https --trust`

# Projects

## JwtAuth.Custom

Has docker support. Run `docker-compose up` from the JwtAuth.Custom folder with docker-compose.yml.

This is the project for `custom` jwt approach, also classified as Password Credentials in OAuth. It has SPA React client and .NET backend.
Sign in with Google is also supported.


## Cookie.BackendOnly

Has docker support. Run `docker-compose up` from the Cookie.BackendOnly.

In this project we are using default .Net cookies auth + our own/custom cookies auth. The client in this case is swagger itself.

To spin multiple projects, use cli: `dotnet run --urls "http://localhost:5002"` and `dotnet run --urls "http://localhost:5001"` from another console.

login: "andreyka26_"
pass: "Mypass1*"

Since cookies are per origin (in our case the origin is localhost), you might want to modify hosts file to something like that:

```
127.0.0.1 app1.localhost
127.0.0.1 app2.localhost
```

and then use this url:
http://app1.localhost:5001/swagger/index.html
http://app2.localhost:5002/swagger/index.html


# Misc

## To inject cookie in browser

```
document.cookie = ".AspNetCore.Cookies=CfDJ8J3GvWYh3VJOqVPI8DGFMzxzrXJKSEO9knpUg52VTBnIxHzW25Vv8KqImnd1F26aC6VeOzBzP9978FH3uaLaxU2mV-vCmCZECEH7oiZ8tv0JE1JupsVcbSDNtWKbbfXwx_1KCuv6HOUdhV1CLHCUB955hr8tvORbaUwcewBPGfxCsdVKy4PUP-yZWwPXsCMJpgdi92jhbCknTX5TLU9tAFNTgbabCMu0ue7mdvxc8SqhJeKpn7eZf5zv2MI5An3nI_SLbDxYg9C09sgN29jQMz4y8dks3mbIdBUIXrDVgnK249NbS4hKr6qugcnCmFdNCue5O1SwBsS1_vb5e0zRx9774tbi7OFgkC5-RHN7FTaM7kX4wHmKCYWQ7CQBbnBYFB1FTzUmkSMCp0zxu6q452vxn_UiQbkT6kJXuX_Xt3nnPS_-iU4GDqI0t_d2urDjPVDcbvqAEJFoKCTBr8j4cwg; path=/; expires=Fri, 31 Dec 9999 23:59:59 GMT";

.AspNetCore.Cookies:
CfDJ8J3GvWYh3VJOqVPI8DGFMzxzrXJKSEO9knpUg52VTBnIxHzW25Vv8KqImnd1F26aC6VeOzBzP9978FH3uaLaxU2mV-vCmCZECEH7oiZ8tv0JE1JupsVcbSDNtWKbbfXwx_1KCuv6HOUdhV1CLHCUB955hr8tvORbaUwcewBPGfxCsdVKy4PUP-yZWwPXsCMJpgdi92jhbCknTX5TLU9tAFNTgbabCMu0ue7mdvxc8SqhJeKpn7eZf5zv2MI5An3nI_SLbDxYg9C09sgN29jQMz4y8dks3mbIdBUIXrDVgnK249NbS4hKr6qugcnCmFdNCue5O1SwBsS1_vb5e0zRx9774tbi7OFgkC5-RHN7FTaM7kX4wHmKCYWQ7CQBbnBYFB1FTzUmkSMCp0zxu6q452vxn_UiQbkT6kJXuX_Xt3nnPS_-iU4GDqI0t_d2urDjPVDcbvqAEJFoKCTBr8j4cwg

---

or for jwt

document.cookie = "myAuthCookie=eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYW5kcmV5a2EyNl8iLCJGdWxsTmFtZSI6IkFuZHJpaSBCdWkiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNzQyODQxOTIzLCJpc3MiOiJhbmRyZXlrYTI2IiwiYXVkIjoiYXVkaWVuY2UifQ.oj9RA33k2rPuR08SC-kPq2rEp6CD5zMMBJujqnMHnzg; path=/; expires=Fri, 31 Dec 9999 23:59:59 GMT";

myAuthCookie:
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYW5kcmV5a2EyNl8iLCJGdWxsTmFtZSI6IkFuZHJpaSBCdWkiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNzQyODQxOTIzLCJpc3MiOiJhbmRyZXlrYTI2IiwiYXVkIjoiYXVkaWVuY2UifQ.oj9RA33k2rPuR08SC-kPq2rEp6CD5zMMBJujqnMHnzg

```