using Arcus.Testing;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Arcus.Observability.Tests.Integration
{
    public class IntegrationTest
    {
        public IntegrationTest(ITestOutputHelper testOutput)
        {
            Logger = new XunitTestLogger(testOutput);
            Configuration = TestConfig.Create();
        }

        protected TestConfig Configuration { get; }

        protected ILogger Logger { get; }
    }
}