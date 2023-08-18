using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Runtime.InteropServices;

namespace okta_api.Services
{
    public enum Databases
    {
        db1,
        db2,
        db3,
        db4,
    }

    public interface IExtendedConfiguration
    {
        public string GetRequiredValue(string variableName);
        public string? GetOptionalValue(string variableName);
        public string GetConnectionString(Databases db);
        public string Environment { get; }
        public bool isDevelopment();
        public bool isDevelopmentOrLocalhost();
        public bool isProduction();
    }

    public class ExtendedConfiguration : IExtendedConfiguration
    {

        private Dictionary<string, string> databaseConnectionStrings;

        public IConfiguration IConfiguration { get; init; }
        public string Environment { get; init; }
        public bool isDevelopment() => Environment == "dev";
        public bool isDevelopmentOrLocalhost() => Environment == "dev" || Environment == "localhost";
        public bool isProduction() => Environment == "prod";

        public ExtendedConfiguration(IConfiguration config)
        {
            IConfiguration = config;

            var keys = config.GetChildren().Select(e => (e.Key, e.Value));

            Environment = config["APP_ENVIRONMENT"]?.ToLower() ?? config["ASPNETCORE_ENVIRONMENT"]?.ToLower() ?? "none";
            if (Environment == "none") throw new Exception("Environment must be set");

            CreateDBConnectionStrings();
        }

        private void CreateDBConnectionStrings()
        {
            databaseConnectionStrings = IConfiguration
                .GetSection("APP_DATABASES")
                .GetChildren()
                .Select(e =>
                {
                    var server = GetRequiredValue($"APP_DATABASES:{e.Key}:server");
                    var database = GetRequiredValue($"APP_DATABASES:{e.Key}:database");
                    var uid = GetRequiredValue("APP_DB_UID");
                    var pwd = GetRequiredValue("APP_DB_PWD");
                    var ConnectionString = $"SERVER={server};DATABASE={database};UID={uid};PWD={pwd};Encrypt=Yes;TrustServerCertificate=Yes;";

                    return (e.Key, ConnectionString);
                })
                .ToDictionary(e => e.Key, e => e.ConnectionString);
        }

        public string GetConnectionString(Databases db) => databaseConnectionStrings[db.ToString()];

        public string GetRequiredValue(string variableName)
        {
            var temp = GetOptionalValue(variableName);
            if (string.IsNullOrWhiteSpace(temp)) throw new Exception($"<{variableName}> was not found in appsettings or environment variable");

            return temp;
        }

        public string? GetOptionalValue(string variableName)
        {
            return IConfiguration[variableName];
        }
   
    }
}
