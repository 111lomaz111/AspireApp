using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace xUnit.Tests
{
    public class LoggingTest
    {
        [Fact]
        public async Task GetWebResourceRootReturnsOkStatusCodeWithLogging()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            var cancellationToken = cts.Token;

            var builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.AspireApp_AppHost>(cancellationToken);

            builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            builder.Services.AddLogging(logging => logging
                .AddConsole()
                .AddFilter("Default", LogLevel.Information)
                .AddFilter("Microsoft.AspNetCore", LogLevel.Warning)
                .AddFilter("Aspire.Hosting.Dcp", LogLevel.Warning));

            await using var app = await builder.BuildAsync(cancellationToken);
            await app.StartAsync(cancellationToken);

            var httpClient = app.CreateHttpClient("webfrontend");

            await app.ResourceNotifications.WaitForResourceHealthyAsync(
                "webfrontend",
                cancellationToken);

            var response = await httpClient.GetAsync("/", cancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
