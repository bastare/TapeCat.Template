namespace TapeCat.Template.Infrastructure.loC.Bus.Injectors.Common.Extensions;

using MassTransit;
using MoreLinq;
using Domain.Contracts.Common.Attributes;

public static class MassTransitExtensions
{
	public static void AddRequestClient ( this IMediatorRegistrationConfigurator mediatorRegistrationConfigurator , Assembly[] assembliesWithRequestClients )
	{
		assembliesWithRequestClients
			.SelectMany ( GetAllAssemblyTypes )
			.Where ( IsRequestClientContract )
			.Select ( FormRequestClientMetadata )
			.ForEach ( requestClientTypeMetadata =>
			  {
				  var (requestClientType, requestTimeout) = requestClientTypeMetadata;

				  if ( requestTimeout is not null )
				  {
					  mediatorRegistrationConfigurator.AddRequestClient (
						requestClientType ,
						timeout: requestTimeout.Value );

					  return;
				  }

				  mediatorRegistrationConfigurator.AddRequestClient ( requestClientType );
			  } );

		static IEnumerable<Type> GetAllAssemblyTypes ( Assembly assembly )
			=> assembly.GetTypes ();

		static bool IsRequestClientContract ( Type type )
			=> ResolveRequestClientAttribute ( type ) is not null;

		static (Type RequestClientType, RequestTimeout? RequestTimeout) FormRequestClientMetadata ( Type type )
			=> (type, ResolveRequestClientAttribute ( type )!.RequestTimeout);

		static RequestClientContractAttribute? ResolveRequestClientAttribute ( Type type )
			=> type.GetCustomAttribute<RequestClientContractAttribute> ();
	}
}