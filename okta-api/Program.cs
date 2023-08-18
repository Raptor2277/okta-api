using Microsoft.AspNetCore.Authentication.JwtBearer;
using Okta.AspNetCore;
using okta_api.Services;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IExtendedConfiguration, ExtendedConfiguration>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
})
.AddOktaWebApi(new OktaWebApiOptions()
{
    OktaDomain = Configuration["Okta:OktaDomain"],
    AuthorizationServerId = Configuration["Okta:AuthorizationServerId"],
    Audience = Configuration["Okta:Audience"]
});

Console.WriteLine(Databases.db1.ToString());


var app = builder.Build();

var config = app.Services.GetRequiredService<IExtendedConfiguration>();


Console.WriteLine($"env is {config.Environment}");
Console.WriteLine($"showsup is {config.GetOptionalValue("showsUpInAll")}");
Console.WriteLine($"onlyindev is {config.GetOptionalValue("onlyindev")}");
Console.WriteLine($"onlyinprod is {config.GetOptionalValue("onlyinprod")}");
Console.WriteLine($"APP_SERVE_URL is {config.GetRequiredValue("APP_SERVE_URL")}");

// Configure the HTTP request pipeline.
if (config.isDevelopmentOrLocalhost())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapGet("/", () => "hello world");

app.Run(config.GetRequiredValue("APP_SERVE_URL"));
