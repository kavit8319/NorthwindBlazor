using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Northwind.Interface.Server.Adaptors;

using Northwind.Interface.Server.Authentification;
using Northwind.Interface.Server.AutoMapperProfilers;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Resources;
using Syncfusion.Blazor;
using Northwind.Interface.Server.Pages.LoginPages.reCaptcha;
using Microsoft.AspNetCore.Authentication.Cookies;
using Northwind.Interface.Server.Components;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthentication("Cookies").AddCookie(el =>
//{
//    el.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//    el.SlidingExpiration = true;
//    el.Cookie.Name = "northwind";
//    el.Cookie.SameSite = SameSiteMode.Strict;
//    el.EventsType = typeof(Northwind.Interface.Server.Controllers.CookieAuthenticationEvents);

//});
//builder.Services.AddScoped<Northwind.Interface.Server.Controllers.CookieAuthenticationEvents>();
builder.Services.AddSignalR(options => { options.MaximumReceiveMessageSize = 10 * 1024 * 1024; });
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAutoMapProffilers();
builder.Services.AddControllers();
builder.Services.AddLocalization(); ;
builder.Services.AddServerSideBlazor();

builder.Services.AddLocalStorageServices();


Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU1NjQyNkAzMjMxMmUzMTJlMzMzN1Y3V0RjbzFDUWYvOVBTZVRud0Z2YWt6bi9hdTJmTyt6NVFSNnV4YUNGbnc9");
//load scrip for every element syncfusion if false
builder.Services.AddSyncfusionBlazor();

builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
builder.Services.Configure<CaptchConfiguration>(builder.Configuration.GetSection("reCAPTCHA"));
builder.Services.AddTransient<CaptchaApi>();
builder.Services.AddScoped<CustomToastService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddTransient<AuthenticationDelegatingHandler>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddScoped<IClient, Client>();
builder.Services.AddScoped<Client>();
builder.Services.AddScoped<BaseHttpClient>();

var httpWebApi = builder.Configuration.GetSection("WebApi").GetValue<string>("Http");
builder.Services.AddHttpClient("UserServiceSimple", el =>
{
    el.BaseAddress = new Uri(httpWebApi);
    el.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");


}).ConfigurePrimaryHttpMessageHandler(el => new HttpClientHandler
{
    ClientCertificateOptions = ClientCertificateOption.Manual,
    ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            }
}) ;
builder.Services.AddHttpClient("UserServiceAuth", el =>
{
    el.BaseAddress = new Uri(httpWebApi);
    el.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
}).AddHttpMessageHandler<AuthenticationDelegatingHandler>().ConfigurePrimaryHttpMessageHandler(el => new HttpClientHandler
{
    ClientCertificateOptions = ClientCertificateOption.Manual,
    ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            }
}); 


builder.Services.AddSingleton<IServiceCollectionProvider>(new ServiceCollectionProvider(builder.Services));
builder.Services.AddScoped<IReConfigurationHttpClinet, ReConfigurationHttpClinet>(el => new ReConfigurationHttpClinet(el.GetRequiredService<IServiceCollectionProvider>(), el.GetRequiredService<ILocalStorageService>()));
builder.Services.AddScoped<HttpSetingsForHttpClient>();


builder.Services.AddCustomSyncfusionAdaptors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();


var cultures = builder.Configuration.GetSection("Cultures").GetChildren().ToDictionary(x => x.Key, x => x.Value);
var supportedCultures = cultures.Keys.ToArray();
var localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures).SetDefaultCulture("uk-UA");

app.UseRequestLocalization(localizationOptions);
app.UseAuthentication();
app.UseAuthorization();
//app.UseCookiePolicy(new CookiePolicyOptions { });

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
