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
    public class IoTHubTests : ApplicationInsightsSinkTests
    {
        public IoTHubTests(ITestOutputHelper outputWriter) : base(outputWriter)
        {
        }

        [Fact]
        public async Task LogIoTHubDependency_SinksToApplicationInsights_ResultsIEventHubsDependencyTelemetry()
        {
            // Arrange
            string componentName = BogusGenerator.Commerce.ProductName();
            LoggerConfiguration.Enrich.WithComponentName(componentName);

            string iotHubName = BogusGenerator.Commerce.ProductName();
            string dependencyName = iotHubName;
            string dependencyId = BogusGenerator.Random.Guid().ToString();

            bool isSuccessful = BogusGenerator.PickRandom(true, false);
            DateTimeOffset startTime = DateTimeOffset.Now;
            TimeSpan duration = BogusGenerator.Date.Timespan();
            Dictionary<string, object> telemetryContext = CreateTestTelemetryContext();

            // Act
            Logger.LogIotHubDependency(iotHubName, isSuccessful, startTime, duration, dependencyId, telemetryContext);

            // Assert
            await RetryAssertUntilTelemetryShouldBeAvailableAsync(async client =>
            {
                EventsDependencyResult[] results = await client.GetDependenciesAsync();
                AssertX.Any(results, result =>
                {
                    Assert.Equal("Azure IoT Hub", result.Type);
                    Assert.Equal(iotHubName, result.Target);
                    Assert.Equal(componentName, result.RoleName);
                    Assert.Equal(dependencyName, result.Name);
                    Assert.Equal(dependencyId, result.Id);
                });
            });
        }

        [Fact]
        public async Task LogIoTHubDependencyWithConnectionString_SinksToApplicationInsights_ResultsIEventHubsDependencyTelemetry()
        {
            // Arrange
            string componentName = BogusGenerator.Commerce.ProductName();
            LoggerConfiguration.Enrich.WithComponentName(componentName);

            string hostName = $"{BogusGenerator.Commerce.ProductName()}.azure-devices.net";
            string dependencyName = hostName;
            string connectionString = $"HostName={hostName};DeviceId={Guid.NewGuid()};SharedAccessKey={Guid.NewGuid()}";
            string dependencyId = BogusGenerator.Random.Guid().ToString();

            bool isSuccessful = BogusGenerator.PickRandom(true, false);
            DateTimeOffset startTime = DateTimeOffset.Now;
            TimeSpan duration = BogusGenerator.Date.Timespan();
            Dictionary<string, object> telemetryContext = CreateTestTelemetryContext();

            // Act
            Logger.LogIotHubDependencyWithConnectionString(connectionString, isSuccessful, startTime, duration, dependencyId, telemetryContext);

            // Assert
            await RetryAssertUntilTelemetryShouldBeAvailableAsync(async client =>
            {
                EventsDependencyResult[] results = await client.GetDependenciesAsync();
                AssertX.Any(results, result =>
                {
                    Assert.Equal("Azure IoT Hub", result.Type);
                    Assert.Equal(hostName, result.Target);
                    Assert.Equal(componentName, result.RoleName);
                    Assert.Equal(dependencyName, result.Name);
                    Assert.Equal(dependencyId, result.Id);
                });
            });
        }
    }
}
