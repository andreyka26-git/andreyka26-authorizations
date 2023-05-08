using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


//TODO refactor and try remove unnecessary, there should be a few.

builder.Services.AddControllers();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://localhost:7000";
                options.RequireHttpsMetadata = false;

                options.ApiName = "api1";
            });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
