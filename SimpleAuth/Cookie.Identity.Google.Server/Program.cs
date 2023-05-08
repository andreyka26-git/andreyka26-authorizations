using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cookie.Identity.Google.Server.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AuthContextConnection");

builder.Services.AddDbContext<AuthContext>(options => options.UseNpgsql(connectionString));

// This line adds Cookies
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AuthContext>();

builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       options.ClientId = builder.Configuration["ClientId"];
       options.ClientSecret = builder.Configuration["ClientSecret"];
   });

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
using (var db = scope.ServiceProvider.GetRequiredService<AuthContext>())
{
    db.Database.Migrate();
}

app.Run();
