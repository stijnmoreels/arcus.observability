﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arcus.Observability.Tests.Integration.Serilog.Sinks.ApplicationInsights.Fixture;
using Microsoft.Extensions.Logging;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Arcus.Observability.Tests.Integration.Serilog.Sinks.ApplicationInsights
{
    public class EventHubsDependencyTests : ApplicationInsightsSinkTests
    {
        public EventHubsDependencyTests(ITestOutputHelper outputWriter) : base(outputWriter)
        {
        }

        [Fact]
        public async Task LogEventHubsDependency_SinksToApplicationInsights_ResultsInEventHubsDependencyTelemetry()
        {
            // Arrange
            string componentName = BogusGenerator.Commerce.ProductName();
            LoggerConfiguration.Enrich.WithComponentName(componentName);

            string dependencyType = "Azure Event Hubs";
            string eventHubName = BogusGenerator.Commerce.ProductName();
            string namespaceName = BogusGenerator.Finance.AccountName();
            string dependencyName = eventHubName;
            string dependencyId = "test-parent";

            bool isSuccessful = BogusGenerator.PickRandom(true, false);
            DateTimeOffset startTime = DateTimeOffset.Now;
            TimeSpan duration = BogusGenerator.Date.Timespan();
            Dictionary<string, object> telemetryContext = CreateTestTelemetryContext();

            // Act
            Logger.LogEventHubsDependency(namespaceName, eventHubName, isSuccessful, startTime, duration, dependencyId, telemetryContext);

            // Assert
            await RetryAssertUntilTelemetryShouldBeAvailableAsync(async client =>
            {
                DependencyResult[] results = await client.GetDependenciesAsync();
                AssertX.Any(results, result =>
                {
                    Assert.Equal(dependencyType, result.Type);
                    Assert.Equal(eventHubName, result.Target);
                    Assert.Equal(namespaceName, result.Data);
                    Assert.Equal(componentName, result.RoleName);
                    Assert.Equal(dependencyName, result.Name);
                    Assert.Equal(dependencyId, result.Id);
                });
            });
        }
    }
}
