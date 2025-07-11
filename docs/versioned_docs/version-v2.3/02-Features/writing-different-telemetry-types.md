---
title: "Write different telemetry types"
layout: default
---

# Write different telemetry types

Logs are a great way to gain insights, but sometimes they are not the best approach for the job.

We provide the capability to track the following telemetry types on top of ILogger with good support on Serilog:

- [Write different telemetry types](#write-different-telemetry-types)
  - [Installation](#installation)
  - [Dependencies](#dependencies)
    - [Measuring Azure Blob Storage dependencies](#measuring-azure-blob-storage-dependencies)
    - [Measuring Azure Cosmos DB dependencies](#measuring-azure-cosmos-db-dependencies)
    - [Measuring Azure Event Hubs dependencies](#measuring-azure-event-hubs-dependencies)
    - [Measuring Azure IoT Hub dependencies](#measuring-azure-iot-hub-dependencies)
    - [Measuring Azure Key Vault dependencies](#measuring-azure-key-vault-dependencies)
    - [Measuring Azure Search dependencies](#measuring-azure-search-dependencies)
    - [Measuring Azure Service Bus dependencies](#measuring-azure-service-bus-dependencies)
    - [Measuring Azure Table Storage Dependencies](#measuring-azure-table-storage-dependencies)
    - [Measuring HTTP dependencies](#measuring-http-dependencies)
    - [Measuring SQL dependencies](#measuring-sql-dependencies)
    - [Measuring custom dependencies](#measuring-custom-dependencies)
    - [Making it easier to measure dependencies](#making-it-easier-to-measure-dependencies)
  - [Events](#events)
    - [Security Events](#security-events)
  - [Metrics](#metrics)
  - [Requests](#requests)

For most optimal output, we recommend using our [Azure Application Insights sink](./sinks/azure-application-insights.md).

**We highly encourage to provide contextual information to all your telemetry** to make it more powerful and support this for all telemetry types.

> :bulb: For sake of simplicity we have not included how to track contextual information, for more information see [our documentation](./making-telemetry-more-powerful.md).

## Installation

This feature requires to install our NuGet package

```shell
PM > Install-Package Arcus.Observability.Telemetry.Core
```

## Dependencies

Dependencies allow you to track how your external dependencies are doing to give you insights on performance and error rate.

Since measuring dependencies can add some noise in your code, we've introduced `DependencyMeasurement` to make it simpler. ([docs](#making-it-easier-to-measure-dependencies))

### Measuring Azure Blob Storage dependencies

We allow you to measure Azure Blob Storage dependencies.

Here is how you can report a dependency call:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.LogBlobStorageDependency(accountName: "multimedia", containerName: "images", isSuccessful: true, startTime, durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure blob",  "DependencyName": "images", "TargetName": "multimedia", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring Azure Cosmos DB dependencies

We allow you to measure Azure Cosmos dependencies.

Here is how you can report a dependency call:

**Cosmos SQL**

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.LogCosmosSqlDependency(accountName: "administration", database: "docs", container: "purchases", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure DocumentDB", "DependencyData": "docs/purchases", "TargetName": "administration", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful" true, "Context": {}}
```

### Measuring Azure Event Hubs dependencies

We allow you to measure Azure Event Hubs dependencies.

Here is how you can report a dependency call:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.LogEventHubsDependency(namespaceName: "be.sensors.contoso", eventHubName: "temperature", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure Event Hubs", "DependencyData": "be.sensors.contoso", "TargetName": "temerature", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring Azure IoT Hub dependencies

We allow you to measure Azure IoT Hub dependencies.

**Example**

Here is how you can report a dependency call:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.LogIotHubDependency(iotHubName: "sensors", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure IoT Hub", "TargetName": "sensors", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

Or, alternatively you can pass along the IoT connection string itself so the host name will be selected for you.

**Installation**

This feature requires to install our NuGet package

```shell
PM > Install-Package Arcus.Observability.Telemetry.IoT
```

**Example**

Here is how you can report a dependency call:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.LogIotHubDependency(iotHubConnectionString: "Hostname=sensors;", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure IoT Hub", "TargetName": "sensors", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring Azure Key Vault dependencies

We allow you to measure Azure Key vault dependencies.

**Example**

Here is how you can report a dependency call:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new StopWatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.AzureKeyVaultDependency(vaultUri: "https://my-secret-store.vault.azure.net", secretName: "ServiceBus-ConnectionString", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure key vault", "DependencyData": "ServiceBus-ConnectionString", "TargetName": "https://my-secret-store.vault.azure.net", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring Azure Search dependencies

We allow you to measure Azure Search dependencies for cognitive services.

Here is how you can report an Azure Search dependency:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.LogAzureSearchDependency(searchServiceName: "orders-search", operationName: "get-orders", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure Search", "DependencyData": "get-orders", "TargetName": "orders-search", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring Azure Service Bus dependencies

We allow you to measure Azure Service Bus dependencies for both queues & topics.

Here is how you can report an Azure Service Bus Queue dependency:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow

logger.LogServiceBusQueueDependency(queueName: "ordersqueue", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure Service Bus", "TargetName": "ordersqueue", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {"EntityType": "Queue"}}
```

Note that we have an `LogServiceBusTopicDependency` to log dependency logs for an Azure Service Bus Topic and an `LogServiceBusDependency` to log Azure Service Bus logs where the entity type is not known.

### Measuring Azure Table Storage Dependencies

We allow you to measure Azure Table Storage dependencies.

Here is how you can report a dependency call:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;

logger.LogTableStorageDependency(accountName: "orderAccount", tableName: "orders", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Azure table", "DependencyData": "orders", "TargetName": "orderAccount", "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring HTTP dependencies

Here is how you can report a HTTP dependency:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Create request
var request = new HttpRequestMessage(HttpMethod.Post, "http://requestbin.net/r/ujxglouj")
{
    Content = new StringContent("{\"message\":\"Hello World!\"")
};

// Start measuring
durationMeasurement.Start();
var startTime = DateTimeOffset.UtcNow;
// Send request to dependant service
var response = await httpClient.SendAsync(request);

logger.LogHttpDependency(request, statusCode: response.StatusCode, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType" "Http", "DependencyName": "POST /r/ujxglouj", "TargetName": "requestbin.net", "ResultCode": 200, "Duration": "00:00:00.2521801", "StartTime": "03/23/2020 09:56:31 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring SQL dependencies

Here is how you can report a SQL dependency:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
var startTime = DateTimeOffset.UtcNow;
durationMeasurement.Start();

// Interact with database
var products = await _repository.GetProducts();

logger.LogSqlDependency("sample-server", "sample-database", "my-table", "get-products", isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "Sql", "DependencyName": "sample-database/my-table", "DependencyData": "get-products", "TargetName": "sample-server", "Duration": "00:00:01.2396312", "StartTime": "03/23/2020 09:32:02 +00:00", "IsSuccessful": true, "Context": {}}
```

Or alternatively, when one already got the SQL connection string, you can use the overload that takes this directly:

**Installation**

This feature requires to install our NuGet package

```shell
PM > Install-Package Arcus.Observability.Telemetry.Sql
```

**Example**

```csharp
using Microsoft.Extensions.Logging;

string connectionString = "Server=sample-server;Database=sample-database;User=admin;Password=123";
var durationMeasurement = new Stopwatch();

// Start measuring
var startTime = DateTimeOffset.UtcNow;
durationMeasurement.Start();

// Interact with database
var products = await _repository.GetProducts();

logger.LogSqlDependency(connectionString, "my-table", "get-products", isSuccessful: true, measurement: measurement);
// Output: {"DependencyType": "Sql", "DependencyName": "sample-database/my-table", "DependencyData": "get-products", "TargetName": "sample-server", "Duration": "00:00:01.2396312", "StartTime": "03/23/2020 09:32:02 +00:00", "IsSuccessful": true, "Context": {}}
```

### Measuring custom dependencies

Here is how you can measure a custom dependency:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();

// Start measuring
var startTime = DateTimeOffset.UtcNow;
durationMeasurement.Start();

string dependencyName = "SendGrid";
object dependencyData = "http://my.sendgrid.uri/"

logger.LogDependency("SendGrid", dependencyData, isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed);
// Output: {"DependencyType": "SendGrid", "DependencyData": "http://my.sendgrid.uri/", "Duration": "00:00:01.2396312", "StartTime": "03/23/2020 09:32:02 +00:00", "IsSuccessful": true, "Context": {}}
```

### Making it easier to measure dependencies

Measuring dependencies means you need to keep track of how long the action took and when it started.

Here's a small example:

```csharp
using Microsoft.Extensions.Logging;

var durationMeasurement = new Stopwatch();
var startTime = DateTimeOffset.UtcNow;
durationMeasurement.Start();

// Do action

/// Track dependency
string dependencyName = "SendGrid";
object dependencyData = "https://my.sendgrid.uri/";
logger.LogDependency("SendGrid", dependencyData, isSuccessful: true, startTime: startTime, duration: durationMeasurement.Elapsed, context: telemetryContext);
```

However, by using `DependencyMeasurement.Start()` we take care of the measuring aspect:

```csharp
using Arcus.Observability.Telemetry.Core;
using Microsoft.Extensions.Logging;

// Start measuring
using (var measurement = DependencyMeasurement.Start())
{
    // Do Action

    // Track dependency
    string dependencyName = "SendGrid";
    object dependencyData = "https://my.sendgrid.uri/";
    logger.LogDependency(dependencyName, dependencyData, isSuccessful: true, startTime: measurement, context: telemetryContext);
}
```

Failures during the interaction with the tracked dependency can be controlled by passing `isSuccessful`:

```csharp
string dependencyName = "SendGrid";
object dependencyData = "https://my.sendgrid.uri";

try
{
    // Interact with SendGrid...
    // Done!

    logger.LogDependency(dependencyName, dependencyData, isSuccessful: true, startTime: measurement, context: telemetryContext);
}
catch (Exception exception)
{
    logger.LogError(exception, "Failed to interact with SendGrid");
    logger.LogDependency(dependencyName, dependencyData, isSuccessful: false, startTime: measurement, context: telemetryContext);
}
```

## Events

Events allow you to report custom events which are a great way to track business-related events.

Here is how you can report an `Order Created` event:

```csharp
using Microsoft.Extensions.Logging;

logger.LogEvent("Order Created");
// Output: {"EventName": "Order Created", "Context": {}}
```

### Security Events

Some events are considered "security events" when they relate to possible malicious activity, authentication, input validation...

Here is how an invalid `Order` can be reported:

```csharp
using Microsoft.Extensions.Logging;

logger.LogSecurityEvent("Invalid Order");
// Output: {"EventName": "Invalid Order", "Context": {"EventType": "Security"}}
```

## Metrics

Metrics allow you to report custom metrics which allow you to give insights on application-specific metrics.

Here is how you can report an `Invoice Received` metric:

```csharp
using Microsoft.Extensions.Logging;

logger.LogMetric("Invoice Received", 133.37, telemetryContext);
// Output: {"MetricName": "Invoice Received", "MetricValue": 133.37, "Timestamp": "03/23/2020 09:32:02 +00:00", "Context: {}}
```

## Requests

Requests allow you to keep track of the HTTP requests that are performed against your API and what the response was that was sent out.

**Installation**

If you want to track the `HttpRequest` and `HttpResponse` of an ASP.NET Core project, you'll have to install an additional package to include these ASP.NET Core dependencies:

```shell
PM > Install-Package Arcus.Observability.Telemetry.AspNetCore
```

**Example**

Here is how you can keep track of requests:

```csharp
using Microsoft.Extensions.Logging;

// Determine calling tenant
string tenantName = "Unknown";
if (httpContext.Request?.Headers?.ContainsKey("X-Tenant") == true)
{
    tenantName = httpContext.Request.Headers["X-Tenant"];
}

var stopWatch = Stopwatch.StartNew();

// Perform action that creates a response, in this case call next middleware in the chain.
await _next(httpContext);

logger.LogRequest(httpContext.Request, httpContext.Response, stopWatch.Elapsed);
// Output: {"RequestMethod": "GET", "RequestHost": "http://localhost:5000/", "RequestUri": "http://localhost:5000/weatherforecast", "ResponseStatusCode": 200, "RequestDuration": "00:00:00.0191554", "RequestTime": "03/23/2020 10:12:55 +00:00", "Context": {}}
```


