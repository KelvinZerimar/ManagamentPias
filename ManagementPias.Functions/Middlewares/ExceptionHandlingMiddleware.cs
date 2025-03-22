using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ManagementPias.Functions.Middlewares;

public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong");

            List<string> trace = new();
            Exception? tracer = ex;
            while (tracer is not null)
            {
                trace.Add(tracer!.Message);
                tracer = tracer!.InnerException;
            }

            // return this response with status code 500
            var httpReqData = await context.GetHttpRequestDataAsync();
            if (httpReqData != null)
            {
                var newHttpResponse = httpReqData.CreateResponse(HttpStatusCode.InternalServerError);
                await newHttpResponse.WriteAsJsonAsync(new
                {
                    success = false,
                    errors = JsonSerializer.Serialize(trace.ToArray())
                });
                context.GetInvocationResult().Value = newHttpResponse;
            }
        }
    }
}
