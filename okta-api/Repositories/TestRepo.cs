using okta_api.Services;

namespace okta_api.Repositories
{
    public class TestRepo
    {
        private ExtendedConfiguration config;
        public TestRepo(ExtendedConfiguration config)
        {
            this.config = config;
        }

        public async Task<string> GetApps()
        {
            config.GetConnectionString(Databases.db1);

            return "heelo";
        }

        public async Task<string> GetThis()
        {
            config.GetConnectionString(Databases.db1);

            return "heelo";
        }

        public async Task<string> GetThat()
        {
            config.GetConnectionString(Databases.db1);

            return "heelo";
        }
    }
}
