using Aspire.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace xUnit.Tests
{
    public class EnvVarTests
    {
        [Fact]
        public async Task WebResourceEnvVarsResolveToApiService()
        {
            // Arrange
            var builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.AspireApp_AppHost>();

            var frontend = builder.CreateResourceBuilder<ProjectResource>("webfrontend");

            // Act
            var executionConfiguration = await ExecutionConfigurationBuilder.Create(frontend.Resource)
                .WithEnvironmentVariablesConfig()
                .BuildAsync(new(DistributedApplicationOperation.Publish), NullLogger.Instance, CancellationToken.None)
                .ConfigureAwait(true);

            var envVars = executionConfiguration.EnvironmentVariables.ToDictionary();

            // Assert
            Assert.Contains(envVars, static (kvp) =>
            {
                var (key, value) = kvp;

                return key is "APISERVICE_HTTPS"
                    && value is "{apiservice.bindings.https.url}";
            });
        }
    }
}
