namespace TapeCat.Template.Domain.Shared.Helpers.AssertGuard.Interfaces;

using FluentValidation;
using System;

public interface IGuard
{
	protected const string DefaultVariableName = "variable";

	void Validate<TValidator, TValue, TException> ( TValue value , string variableName = DefaultVariableName )
		where TValidator : IValidator<TValue>
		where TException : ArgumentException;
}