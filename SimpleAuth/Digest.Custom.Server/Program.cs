using Digest.Custom.Server.Application;
using Digest.Custom.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IUsernameHashedSecretProvider, UsernameHashedSecretProvider>();
builder.Services.AddSingleton<IHashService, HashService>();
builder.Services.AddTransient<DigestAuthService>();
builder.Services.AddSingleton<HeaderService>();

builder.Services.AddAuthentication("Digest")
    .AddScheme<DigestAuthenticationOptions, DigestAuthenticationHandler>("Digest", "Digest", options => 
    {
        options.ServerNonceSecret = "VerySecret";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
