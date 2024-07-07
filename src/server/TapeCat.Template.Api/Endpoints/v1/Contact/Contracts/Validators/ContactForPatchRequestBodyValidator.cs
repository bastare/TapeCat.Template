namespace TapeCat.Template.Api.Queries.Validators;

using Endpoints.v1.Contact.Contracts;
using FastEndpoints;
using FluentValidation;

public sealed class ContactForPatchRequestBodyValidator : Validator<ContactForPatchRequestBody>
{
	public ContactForPatchRequestBodyValidator ()
	{
		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.FirstName )
			.NotEmpty ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.LastName )
			.NotEmpty ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.Email )
			.NotEmpty ()
			.EmailAddress ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.Phone )
			.NotEmpty ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.Title )
			.NotEmpty ();
	}
}