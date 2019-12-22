using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using FundamentalsOfDocker.ch06.Library;

namespace FundamentalsOfDocker.ch06
{
    internal class Hello
    {
        private readonly ITracer _tracer;
        private readonly ILogger<Hello> _logger;
        private readonly WebClient _webClient = new WebClient();

        public Hello(ITracer tracer)
        {
            _tracer = tracer;
            _logger = Tracing.CreateLogger<Hello>();
        }
        private string FormatString(string helloTo)
        {
            using(var scope = _tracer.BuildSpan("format-string").StartActive(true))
            {
                var url = $"http://localhost:5000/api/format/{helloTo}";
                var span = _tracer.ActiveSpan
                    .SetTag(Tags.SpanKind, Tags.SpanKindClient)
                    .SetTag(Tags.HttpMethod, "GET")
                    .SetTag(Tags.HttpUrl, url);

                var dictionary = new Dictionary<string, string>();
                _tracer.Inject(span.Context, BuiltinFormats.HttpHeaders, new TextMapInjectAdapter(dictionary));
                foreach (var entry in dictionary)
                    _webClient.Headers.Add(entry.Key, entry.Value);

                var helloString = _webClient.DownloadString(url);
                scope.Span.Log(new Dictionary<string, object>
                {
                    [LogFields.Event] = "string.Format",
                    ["value"] = helloString
                });
                return helloString;
            }
       }

        private void PrintHello(string helloString)
        {
            using(var scope = _tracer.BuildSpan("print-hello").StartActive(true))
            {
                _logger.LogInformation(helloString);
                scope.Span.Log("WriteLine");
            }
        }
        public void SayHello(string helloTo, string greeting)
        {
            using (var scope = _tracer.BuildSpan("say-hello").StartActive(true))
            {
                scope.Span.SetTag("hello-to", helloTo);
                scope.Span.SetBaggageItem("greeting", greeting);
                var helloString = FormatString(helloTo);
                PrintHello(helloString);
            }
        }
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Expecting two arguments, helloTo and greeting");
            }

            var helloTo = args[0];
            var greeting = args[1];
            using(var tracer = Tracing.Init("hello-world"))
            {
                new Hello(tracer).SayHello(helloTo, greeting);
            }
        }
    }
}