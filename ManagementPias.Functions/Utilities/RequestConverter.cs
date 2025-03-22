using FluentValidation;
using ManagementPias.App.Abstract;
using Microsoft.Azure.Functions.Worker.Http;

namespace ManagementPias.Functions.Utilities;

public static class RequestConverter
{
    public static T Convert<T>(this HttpRequestData req) where T : class
    {
        var dto = req.ReadFromJsonAsync<T>().Result;
        // Validate with FluentValidation
        if (dto is IValidateable<T>) (dto as IValidateable<T>)!.Validator.ValidateAndThrow(dto);
        return dto!;
    }
}
