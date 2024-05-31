namespace TapeCat.Template.Infrastructure.Bus.Brokers.Home.Consumers.Query;

using Contracts;
using Contracts.HomeContracts.Query;
using MassTransit;
using System.Threading.Tasks;

public sealed class GetHomeConsumer : IConsumer<GetHomeContract>
{
	public async Task Consume ( ConsumeContext<GetHomeContract> context )
	{
		try
		{
			await context.RespondAsync<SubmitHomeContract> (
				new ( context.Message.Message ) );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}
	}
}