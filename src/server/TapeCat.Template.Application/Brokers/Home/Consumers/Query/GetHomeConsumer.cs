namespace TapeCat.Template.Application.Brokers.Home.Consumers.Query;

using Contracts.HomeContracts.Query;
using MassTransit;
using System.Threading.Tasks;

public sealed class GetHomeConsumer : IConsumer<GetHomeContract>
{
	public async Task Consume ( ConsumeContext<GetHomeContract> context )
	{
		await context.RespondAsync<SubmitHomeContract> (
			new ( context.Message.Message ) );
	}
}