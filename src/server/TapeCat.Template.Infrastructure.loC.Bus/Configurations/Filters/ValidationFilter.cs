namespace TapeCat.Template.Infrastructure.loC.Bus.Configurations.Filters;

using Domain.Shared.Helpers.AssertGuard.Common.Exceptions;
using Domain.Contracts;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

public sealed class ValidationFilter<TMessage> ( IServiceProvider serviceProvider ) : IFilter<ConsumeContext<TMessage>>
	where TMessage : class
{
	private readonly IServiceProvider _serviceProvider = serviceProvider;

	public async Task Send ( ConsumeContext<TMessage> context , IPipe<ConsumeContext<TMessage>> next )
	{
		if ( !TryValidateContract ( context , out var sendContractException ) )
		{
			await RespondFaultAsync ( context , sendContractException! );

			return;
		}

		await next.Send ( context );

		bool TryValidateContract ( ConsumeContext<TMessage> context , out Exception? exception )
		{
			var validationResult = ValidateContract ( context );

			if ( validationResult.IsValid )
			{
				exception = default;

				return true;
			}

			exception = new AssertValidationException (
				errorMessages: validationResult.Errors
					.Select ( error => error.ErrorMessage ) );

			return false;

			FluentValidation.Results.ValidationResult ValidateContract ( ConsumeContext<TMessage> context )
			{
				var setupValidator =
					_serviceProvider.GetService<IValidator<TMessage>> ();

				return setupValidator is not null
					? setupValidator.Validate ( context.Message )
					: InlineValidate ( context.Message );

				FluentValidation.Results.ValidationResult InlineValidate ( TMessage message )
					=> new InlineValidator<TMessage> ()
						.Validate (
							message ,
							( options ) =>
							  {
								  options.IncludeAllRuleSets ();
							  } );
			}
		}

		static async Task RespondFaultAsync ( ConsumeContext<TMessage> context , Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );

			await context.NotifyConsumed ( context , TimeSpan.Zero , "Filtered" );
		}
	}

	public void Probe ( ProbeContext _ )
	{ }
}