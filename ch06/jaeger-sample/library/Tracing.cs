using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FundamentalsOfDocker.ch06.Library
{
    public static class Tracing
    {
        public static Tracer Init(string serviceName)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddConsole());
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var tracer = Init(serviceName, loggerFactory);
                return tracer;
            }
        }

        private static Tracer Init(string serviceName, ILoggerFactory loggerFactory)
        {
            var samplerConfiguration = new Configuration.SamplerConfiguration(loggerFactory)
                .WithType(ConstSampler.Type)
                .WithParam(1);

            var reporterConfiguration = new Configuration.ReporterConfiguration(loggerFactory)
                .WithLogSpans(true);

            return (Tracer)new Configuration(serviceName, loggerFactory)
                .WithSampler(samplerConfiguration)
                .WithReporter(reporterConfiguration)
                .GetTracer();
        }

        public static ILogger<T> CreateLogger<T>()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddConsole());
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                return loggerFactory.CreateLogger<T>();
            }
        }
    }
}
