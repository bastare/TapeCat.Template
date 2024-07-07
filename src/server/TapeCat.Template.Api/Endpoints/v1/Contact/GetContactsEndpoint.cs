namespace TapeCat.Template.Api.Endpoints.v1.Contact;

using Contracts;
using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Query.GetContacts;
using Mapster;
using Domain.Contracts.Dtos.QueryDtos;
using Domain.Shared.Common.Classes.HttpMessages.Error;
using Domain.Contracts.Dtos.WrapDtos.Interfaces;

public sealed class GetContactsEndpoint ( IRequestClient<GetContactsContract> requestClient )
	: Endpoint<GetContactsQuery , IPaginationRowsDto>
{
	private readonly IRequestClient<GetContactsContract> _requestClient = requestClient;

	public override void Configure ()
	{
		Verbs ( Http.GET );
		Routes ( "api/v1/contacts" );
		AllowAnonymous ();
		Description ( builder => builder
			.Produces<IPaginationRowsDto> ( StatusCodes.Status200OK , "application/json+custom" )
			.ProducesProblemFE<PageErrorMessage> ( StatusCodes.Status400BadRequest )
			.ProducesProblemFE<PageErrorMessage> ( StatusCodes.Status500InternalServerError ) );
	}

	public override async Task HandleAsync ( GetContactsQuery requestQuery , CancellationToken cancellationToken = default )
	{
		var (response, fault) =
			await _requestClient.GetResponse<SubmitContactsContract , FaultContract> (
				new ( requestQuery?.Adapt<ExpressionQueryDto> () ,
					  requestQuery?.Adapt<OrderQueryDto> () ,
					  requestQuery?.Adapt<PaginationQueryDto> () ,
					  requestQuery?.Adapt<ProjectionQueryDto> () ) ,
				cancellationToken );

		if ( !response.IsCompletedSuccessfully )
			throw ( await fault ).Message.Exception;

		await SendAsync (
			response: ( await response ).Message.ContactsForQueryResponse ,
			cancellation: cancellationToken );
	}
}