namespace TapeCat.Template.Api.Endpoints.v1.Contact;

using Contracts;
using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Command.PatchContact;
using Domain.Shared.Common.Classes.HttpMessages.Error;
using Domain.Contracts.ContactContracts.Command.PatchContact.Dtos;

public sealed class PatchContactEndpoint ( IRequestClient<PatchContactContract> requestClient ) :
	Endpoint<ContactForPatchRequestBody , ContactFromPatchDto>
{
	private readonly IRequestClient<PatchContactContract> _requestClient = requestClient;

	public override void Configure ()
	{
		Verbs ( Http.PATCH );
		Routes ( "api/v1/contacts/{id}" );
		AllowAnonymous ();
		Description ( builder => builder
			.Produces<ContactFromPatchDto> ( StatusCodes.Status200OK , "application/json+custom" )
			.ProducesProblemFE<PageErrorMessage> ( StatusCodes.Status400BadRequest )
			.ProducesProblemFE<PageErrorMessage> ( StatusCodes.Status500InternalServerError ) );
	}

	public override async Task HandleAsync ( ContactForPatchRequestBody requestBody , CancellationToken cancellationToken = default )
	{
		var (response, fault) =
			await _requestClient.GetResponse<SubmitPatchedContactsContract , FaultContract> (
				new ( ContactForPatch: new ()
				{
					Id = ResolveIdFromRoute () ,
					FirstName = requestBody.FirstName ,
					LastName = requestBody.LastName ,
					Email = requestBody.Email ,
					Phone = requestBody.Phone ,
					Title = requestBody.Title ,
					MiddleInitial = requestBody.MiddleInitial
				} ) ,
				cancellationToken );

		if ( !response.IsCompletedSuccessfully )
			throw ( await fault ).Message.Exception;

		await SendAsync (
			response: ( await response ).Message.ContactFromPatch ,
			cancellation: cancellationToken );

		int ResolveIdFromRoute ()
		{
			HttpContext.Request.RouteValues.TryGetValue ( "id" , out var idRouteFragment );

			if ( string.IsNullOrEmpty ( idRouteFragment?.ToString () ) )
				throw new ArgumentException ( $"There is no `Id` fragment" );

			return int.TryParse ( idRouteFragment.ToString () , out var id )
				? id
				: throw new ArgumentException ( $"`Id` value with wrong type: {id.GetType ().Name}" );
		}
	}
}