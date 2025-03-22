using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace ManagementPias.Functions.Common;

public abstract class Abstraction
{
    private readonly IMediator mediator;

    public Abstraction(IMediator mediator)
    {
        this.mediator = mediator;
    }

    protected async Task<HttpResponseData> PostResponse(HttpRequestData req, IRequest request)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await mediator.Send(request);
        return response;
    }

    protected async Task<HttpResponseData> PostResponse<TResponse>(HttpRequestData req, IRequest<TResponse> request)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Request cannot be null.");
        }

        TResponse result = await mediator.Send(request);
        await response.WriteStringAsync(JsonConvert.SerializeObject(result));
        return response;
    }
}

