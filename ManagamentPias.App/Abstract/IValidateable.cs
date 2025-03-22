using FluentValidation;

namespace ManagementPias.App.Abstract;

public interface IValidateable<T>
{
    AbstractValidator<T> Validator { get; }
}
