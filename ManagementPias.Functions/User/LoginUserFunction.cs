using ManagementPias.App.Features.Users.Commands.RegisterUser;
using ManagementPias.App.Features.Users.Queries.Login;
using ManagementPias.Functions.Common;
using ManagementPias.Functions.Utilities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Net;


namespace ManagementPias.Functions.User;

public class LoginUserFunction : Abstraction
{
    public LoginUserFunction(IMediator mediator) : base(mediator)
    {
    }

    [Function("LoginUser")]
    [OpenApiOperation(operationId: "selectAllJpbs", tags: new[] { "User" }, Summary = "Create token.", Description = "Create new token.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PostgresRegisterUserCommand), Required = true, Description = "Create new token for user.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Summary = "Create token.", Description = "Create Token")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "user/login")] HttpRequestData req)
    {
        //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //dynamic data = JsonConvert.DeserializeObject(requestBody);

        return await PostResponse(
            req, req.Convert<PostgresLoginUserQuery>()
        );
    }

}

