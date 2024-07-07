namespace TapeCat.Template.Domain.Shared.Helpers.AssertGuard.Interfaces;

using FluentValidation;
using System.Runtime.CompilerServices;

public interface IGuard
{
    TValue Validate<TValidator, TValue, TException>(TValue value, [CallerArgumentExpression(nameof(value))] string? variableName = default)
        where TValidator : IValidator<TValue>
        where TException : ArgumentException;
}