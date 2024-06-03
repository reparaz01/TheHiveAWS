using Amazon.S3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheHiveAWS.Data;
using TheHiveAWS.Helpers;
using TheHiveAWS.Models;
using TheHiveAWS.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddTransient<HelperCryptography>();




builder.Services.AddSingleton<IHttpContextAccessor
    , HttpContextAccessor>();

string jsonSecrets = await
    HelperSecretManager.GetSecretsAsync();
KeysModel keysModel =
    JsonConvert.DeserializeObject<KeysModel>(jsonSecrets);
builder.Services.AddSingleton<KeysModel>(x => keysModel);
// Add services to the container.
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<ServiceStorageAWS>();

builder.Services.AddTransient<ServiceApiTheHive>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services.AddControllersWithViews
    (options => options.EnableEndpointRouting = false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
    name: "default",
    template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();