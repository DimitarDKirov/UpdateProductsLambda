using UpdateProductsLambda.Models;

namespace UpdateProductsLambda.Helpers
{
    internal static class EnvironmentHelper
    {
        public static Settings GetEnvironmentVariables()
        {
            return new Settings
            {
                AwsProfile = Environment.GetEnvironmentVariable("AWS_PROFILE"),
                IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            };
        }
    }
}
