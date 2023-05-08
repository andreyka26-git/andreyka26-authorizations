using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServerWIthIdentity;
using IdentityServerWIthIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

//TODO refactor and try remove unnecessary, there should be a few.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services
    .AddIdentityServer(o =>
    {
        o.UserInteraction.LoginUrl = "/Identity/Account/Login";
    })
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<IdentityUser>()
    .AddDeveloperSigningCredential();

builder.Services
    .AddAuthentication()
    .AddCookie()
    .AddGoogle(options =>
    {
        // register your IdentityServer with Google at https://console.developers.google.com
        // enable the Google+ API
        // set the redirect URI to https://localhost:5001/signin-google
        options.ClientId = builder.Configuration["ClientId"];
        options.ClientSecret = builder.Configuration["ClientSecret"];
    });


var app = builder.Build();

app.UseCookiePolicy(
            new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always
            });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
using (var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
{
    db.Database.Migrate();
}

app.Run();
