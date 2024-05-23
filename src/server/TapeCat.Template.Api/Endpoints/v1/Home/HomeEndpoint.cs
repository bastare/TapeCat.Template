namespace TapeCat.Template.Api.Endpoints.v1.Home;

using FastEndpoints;
using MassTransit;
using TapeCat.Template.Contracts;
using TapeCat.Template.Contracts.HomeContracts.Query;

public sealed class HomeEndpoint ( IRequestClient<GetHomeContract> getHomeRequestClient )
    : Endpoint<string>
{
    private readonly IRequestClient<GetHomeContract> _getHomeRequestClient = getHomeRequestClient;

    public override void Configure ()
    {
        Verbs ( Http.POST );
        Routes ( "api/v1/home" );
    }

    public override async Task HandleAsync ( string request , CancellationToken cancellationToken = default )
    {
        var (response, fault) =
              await _getHomeRequestClient.GetResponse<GetHomeContract , FaultContract> (
                new ( request ) ,
                cancellationToken );

        if ( !response.IsCompletedSuccessfully )
            throw ( await fault ).Message.Exception;

        await SendAsync (
            response: null ,
            statusCode: StatusCodes.Status201Created ,
            cancellation: cancellationToken );
    }
}