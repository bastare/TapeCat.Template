namespace TapeCat.Template.Api.Queries.Validators;

using Endpoints.v1.Contact.Contracts;
using FastEndpoints;
using FluentValidation;

public sealed class ContactForCreationRequestBodyValidator : Validator<ContactForCreationRequestBody>
{
    public ContactForCreationRequestBodyValidator()
    {
        RuleFor(contactForCreationRequestBody => contactForCreationRequestBody.FirstName)
            .NotEmpty();

        RuleFor(contactForCreationRequestBody => contactForCreationRequestBody.LastName)
            .NotEmpty();

        RuleFor(contactForCreationRequestBody => contactForCreationRequestBody.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(contactForCreationRequestBody => contactForCreationRequestBody.Phone)
            .NotEmpty();

        RuleFor(contactForCreationRequestBody => contactForCreationRequestBody.Title)
            .NotEmpty();
    }
}